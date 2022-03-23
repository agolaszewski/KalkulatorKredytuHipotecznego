using Fluxor;
using KalkulatorKredytuHipotecznego.Store.Features.CalculationFeature.Actions;
using KalkulatorKredytuHipotecznego.Store.States;
using System;

namespace KalkulatorKredytuHipotecznego.Store.Features.CalculationFeature.Reducers
{
    public class CalculationStateReducer
    {
        [ReducerMethod]
        public static CalculationState CreditPeriodTypeChangedReducer(CalculationState state, CreditPeriodTypeChanged _)
        {
            var periods = state.CreditPeriodType switch
            {
                PeriodType.Months => state.CreditPeriods * 12,
                PeriodType.Years => state.CreditPeriods / 12,
                _ => throw new ArgumentOutOfRangeException()
            };

            return state with { CreditPeriods = periods };
        }

        [ReducerMethod]
        public static CalculationState MarginValueChangedReducer(CalculationState state, MarginValueChanged _)
        {
            var total = state.Margin + state.WarsawInterbankOfferedRate;

            return state with { TotalMargin = total };
        }

        [ReducerMethod]
        public static CalculationState WarsawInterbankOfferedRateValueChangedReducer(CalculationState state, WarsawInterbankOfferedRateValueChanged _)
        {
            var total = state.Margin + state.WarsawInterbankOfferedRate;

            return state with { TotalMargin = total };
        }

        [ReducerMethod]
        public static CalculationState CreditOpeningValueChangedReducer(CalculationState state, CreditOpeningValueChanged _)
        {
            var creditOpening = state.CreditOpening;

            if (state.FirstInstalmentDate < creditOpening)
            {
                return state with
                {
                    FirstInstalmentDate = new DateTime(creditOpening.Year, creditOpening.Month, creditOpening.Day),
                    CreditOpening = creditOpening,
                };
            }

            return state with { CreditOpening = state.CreditOpening };
        }

        [ReducerMethod]
        public static CalculationState FirstInstalmentDateValueChangedReducer(CalculationState state, FirstInstalmentDateValueChanged _)
        {
            var firstInstalmentDate = state.FirstInstalmentDate;

            if (state.CreditOpening > firstInstalmentDate)
            {
                return state with
                {
                    CreditOpening = new DateTime(firstInstalmentDate.Year, firstInstalmentDate.Month, firstInstalmentDate.Day),
                    FirstInstalmentDate = firstInstalmentDate,
                };
            }

            return state with { FirstInstalmentDate = state.FirstInstalmentDate };
        }
    }
}