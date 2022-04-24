using System;
using Domain;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using KalkulatorKredytuHipotecznego.Store.Features.CalculationFeature.Actions;
using KalkulatorKredytuHipotecznego.Store.States;
using Microsoft.AspNetCore.Components;
using Provider.Indexes;

namespace KalkulatorKredytuHipotecznego.Pages.Main
{
    public partial class BasicDetails : FluxorComponent
    {
        [Inject]
        public IState<CalculationState> State { get; set; }

        [Inject]
        private IDispatcher Dispatcher { get; set; }

        [Inject]
        private IndexProviderFactory IndexProviderFactory { get; set; } 

        protected override void OnParametersSet()
        {
            BaseState<CalculationState>.Dispatcher = Dispatcher;
        }

        public void GetIndexValue()
        {
            var interestIndex = State.Value.WarsawInterbankOfferedRatePeriod == WarsawInterbankOfferedRatePeriod.Wibor3M
                ? Provider.Indexes.Index.Wibor3M
                : Provider.Indexes.Index.Wibor6M;

            IIndexProvider indexProvider = IndexProviderFactory.IndexProvider(interestIndex);
            var indexValue = indexProvider.GetValue(State.Value.CreditOpening);
            Dispatcher.Dispatch(new IndexValueFetched(indexValue));
        }

        public int MaxCreditPeriods()
        {
            return State.Value.CreditPeriodType switch
            {
                PeriodType.Months => 50 * 12,
                PeriodType.Years => 50,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}