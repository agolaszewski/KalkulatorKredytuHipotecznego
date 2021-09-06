using System;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using KalkulatorKredytuHipotecznego.Store.States;
using Microsoft.AspNetCore.Components;

namespace KalkulatorKredytuHipotecznego.Pages.Main
{
    public partial class BasicDetails : FluxorComponent
    {
        [Inject]
        public IState<CalculationState> State { get; set; }

        [Inject]
        private IDispatcher Dispatcher { get; set; }

        protected override void OnParametersSet()
        {
            BaseState<CalculationState>.Dispatcher = Dispatcher;
        }

        public int MaxCreditPeriods()
        {
            return State.Value.CreditPeriodType switch
            {
                PeriodType.Months => 50 * 12,
                PeriodType.Years => 50,
            };
        }
    }
}