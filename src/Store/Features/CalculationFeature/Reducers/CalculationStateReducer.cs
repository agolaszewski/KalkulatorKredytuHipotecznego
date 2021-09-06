using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using KalkulatorKredytuHipotecznego.Store.Features.CalculationFeature.Actions;
using KalkulatorKredytuHipotecznego.Store.States;

namespace KalkulatorKredytuHipotecznego.Store.Features.CalculationFeature
{
    public class CalculationStateReducer
    {
        [ReducerMethod]
        public static CalculationState CreditPeriodTypeChangedReducer(CalculationState state, CreditPeriodTypeChanged _)
        {
            var periods = state.CreditPeriodType switch
            {
                PeriodType.Months => state.CreditPeriods * 12,
                PeriodType.Years => state.CreditPeriods / 12
            };

            return state with { CreditPeriods = periods };
        }
    }
}
