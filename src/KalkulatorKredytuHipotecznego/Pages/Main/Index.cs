using Calculator.Schedule;
using Domain;
using Fluxor;
using Fluxor.Blazor.Web.Components;
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

        protected override void OnParametersSet()
        {
            BaseState<CalculationState>.Dispatcher = Dispatcher;
        }

        private string selectedTab = "basicDetails";

        private void OnSelectedTabChanged(string name)
        {
            selectedTab = name;
        }

        public async Task OnCalculateButtonClicked()
        {
            var strategy = new Domain.InstallmentType((Domain.InstallmentType.InstallmentTypeValues)State.Value.InstallmentType).Strategy;
            CreditAmount creditAmount = State.Value.CreditAmount;
            CreditPeriods creditPeriods = State.Value.CreditPeriodType == PeriodType.Months ? new Months(State.Value.CreditPeriods) : new Years(State.Value.CreditPeriods);

            Interest interest = new Interest(State.Value.Margin / 100, State.Value.WarsawInterbankOfferedRate / 100);
            InstallmentDate installmentDate = State.Value.FirstInstallmentDate;

            WarsawInterbankOfferedRatePeriod wiborInterbankOfferedRatePeriodRatePeriod = WarsawInterbankOfferedRatePeriod.Create(State.Value.WarsawInterbankOfferedRatePeriod);

            var schedule = new ScheduleCalculator();
            var scheduleDetails = schedule.Calculate(strategy, creditAmount, State.Value.CreditOpening, creditPeriods, installmentDate, interest, wiborInterbankOfferedRatePeriodRatePeriod);
        }
    }
}