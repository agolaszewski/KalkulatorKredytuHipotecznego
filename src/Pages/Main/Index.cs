using Fluxor;
using Fluxor.Blazor.Web.Components;
using KalkulatorKredytuHipotecznego.Domain;
using KalkulatorKredytuHipotecznego.Store.States;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace KalkulatorKredytuHipotecznego.Pages.Main
{
    public partial class Index : FluxorComponent
    {
        [Inject]
        public IState<CalculationState> State { get; set; }

        [Inject]
        private IDispatcher Dispatcher { get; set; }

        protected override void OnParametersSet()
        {
            BaseState<CalculationState>.Dispatcher = Dispatcher;
        }

        private string selectedTab = "basicDetails";

        private void OnSelectedTabChanged(string name)
        {
            selectedTab = name;
        }

        private List<InstallmentDetails> Installments = new();

        private void OnCalculateButtonClicked()
        {
            IInstallmentCalculationStrategy strategy = new Domain.InstallmentType(State.Value.InstallmentType).Strategy;
            CreditAmount creditAmount = State.Value.CreditAmount;
            CreditPeriods creditPeriods = State.Value.CreditPeriodType == PeriodType.Months ? new Months(State.Value.CreditPeriods) : new Years(State.Value.CreditPeriods);

            Interest interest = new Interest(State.Value.Margin / 100, State.Value.WarsawInterbankOfferedRate / 100);
            InstallmentDate installmentDate = State.Value.FirstInstallmentDate;

            WarsawInterbankOfferedRatePeriod wiborInterbankOfferedRatePeriodRatePeriod = WarsawInterbankOfferedRatePeriod.Create(State.Value.WarsawInterbankOfferedRatePeriod);

            var schedule = new ScheduleCalculator();
            Installments = schedule.Calculate(strategy, creditAmount, State.Value.CreditOpening, creditPeriods, installmentDate, interest, wiborInterbankOfferedRatePeriodRatePeriod);
        }
    }

    public class ScheduleCalculator
    {
        public List<InstallmentDetails> Calculate(IInstallmentCalculationStrategy strategy, CreditAmount creditAmount,
            CreditOpening creditOpening, CreditPeriods creditPeriods, InstallmentDate installmentDate,
            Interest interest, WarsawInterbankOfferedRatePeriod wiborInterbankOfferedRatePeriodRatePeriod)
        {
            var installments = new List<InstallmentDetails>();
            Installment baseInstallment = strategy.Execute(creditAmount, creditPeriods, interest);

            DateTime from = creditOpening.Value;
            DateTime to = installmentDate.Value;

            int index = 0;
            while (creditPeriods.Value > 1)
            {
                if (index % wiborInterbankOfferedRatePeriodRatePeriod.Value == 0)
                {
                    baseInstallment = strategy.Execute(creditAmount, creditPeriods, interest);
                }

                var installmentPeriod = Days.Difference(from, to);
                var installment = new InstallmentDetails(++index, to, baseInstallment, interest, creditAmount, installmentPeriod);

                installments.Add(installment);

                creditPeriods = creditPeriods.Value - 1;
                creditAmount = creditAmount.Value - installment.Principal;

                from = to;
                to = from.AddMonths(1);
            }

            var lastInstallmentDetails = InstallmentDetails.LastInstallment(++index, to, interest, creditAmount, Days.Difference(from, to));
            installments.Add(lastInstallmentDetails);

            return installments;
        }
    }
}