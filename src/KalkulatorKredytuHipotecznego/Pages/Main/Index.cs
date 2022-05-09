using Calculator.Schedule;
using Domain;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using KalkulatorKredytuHipotecznego.Store.Features.CalculationFeature.Actions;
using KalkulatorKredytuHipotecznego.Store.States;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace KalkulatorKredytuHipotecznego.Pages.Main
{
    public partial class Index : FluxorComponent
    {
        [Inject]
        public IState<CalculationState> State { get; set; }

        [Inject]
        private IDispatcher Dispatcher { get; set; }

        [Inject]
        private ScheduleCalculator ScheduleCalculator { get; set; }

        protected override void OnParametersSet()
        {
            BaseState<CalculationState>.Dispatcher = Dispatcher;
        }

        private string selectedTab = "basicDetails";

        private void OnSelectedTabChanged(string name)
        {
            selectedTab = name;
        }

        public void OnCalculateButtonClicked()
        {
            var strategy = new Domain.InstallmentType((Domain.InstallmentType.InstallmentTypeValues)State.Value.InstallmentType).Strategy;
            CreditAmount creditAmount = State.Value.CreditAmount;
            CreditPeriods creditPeriods = State.Value.CreditPeriodType == PeriodType.Months ? new Months(State.Value.CreditPeriods) : new Years(State.Value.CreditPeriods);

            Margin margin = new Margin(State.Value.Margin / 100M);
            InstallmentDate installmentDate = State.Value.FirstInstallmentDate;

            WarsawInterbankOfferedRate warsawInterbankOfferedRate = State.Value.WarsawInterbankOfferedRate / 100M;
            WarsawInterbankOfferedRatePeriod wiborInterbankOfferedRatePeriodRatePeriod = WarsawInterbankOfferedRatePeriod.Create(State.Value.WarsawInterbankOfferedRatePeriod);

            var scheduleDetails = ScheduleCalculator.Calculate(strategy, creditAmount, State.Value.CreditOpening, creditPeriods, installmentDate, margin, warsawInterbankOfferedRate, wiborInterbankOfferedRatePeriodRatePeriod);
            Dispatcher.Dispatch(new ScheduleCalculated(scheduleDetails));
        }
    }
}