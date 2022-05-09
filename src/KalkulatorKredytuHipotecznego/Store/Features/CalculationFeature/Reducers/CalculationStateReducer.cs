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
            var signingDay = state.SigningDay;
            var creditOpening = state.CreditOpening;
            var firstInstallmentDate = state.FirstInstallmentDate;

            if (creditOpening < signingDay)
            {
                signingDay = new DateTime(creditOpening.Year, creditOpening.Month, creditOpening.Day);
            }

            if (creditOpening > firstInstallmentDate)
            {
                firstInstallmentDate = new DateTime(creditOpening.Year, creditOpening.Month, creditOpening.Day).AddMonths(1);
            }

            return state with
            {
                SigningDay = signingDay,
                FirstInstallmentDate = firstInstallmentDate,
                CreditOpening = creditOpening,
            };
        }

        [ReducerMethod]
        public static CalculationState SigningDayValueChangedReducer(CalculationState state, SigningDayValueChanged _)
        {
            var signingDay = state.SigningDay;
            var creditOpening = state.CreditOpening;
            var firstInstallmentDate = state.FirstInstallmentDate;

            creditOpening = new DateTime(signingDay.Year, creditOpening.Month, creditOpening.Day);
            if (signingDay > creditOpening)
            {
                creditOpening = new DateTime(signingDay.Year, signingDay.Month, signingDay.Day);
            }

            firstInstallmentDate = new DateTime(signingDay.Year, firstInstallmentDate.Month, firstInstallmentDate.Day);
            if (signingDay > firstInstallmentDate)
            {
                firstInstallmentDate = new DateTime(creditOpening.Year, creditOpening.Month, creditOpening.Day).AddMonths(1);
            }

            return state with
            {
                SigningDay = signingDay,
                FirstInstallmentDate = firstInstallmentDate,
                CreditOpening = creditOpening,
            };
        }

        [ReducerMethod]
        public static CalculationState FirstInstallmentDateValueChangedReducer(CalculationState state, FirstInstallmentDateValueChanged _)
        {
            var signingDay = state.SigningDay;
            var creditOpening = state.CreditOpening;
            var firstInstallmentDate = state.FirstInstallmentDate;

            if (firstInstallmentDate < signingDay)
            {
                signingDay = new DateTime(firstInstallmentDate.Year, firstInstallmentDate.Month, firstInstallmentDate.Day);
            }

            if (firstInstallmentDate < creditOpening)
            {
                creditOpening = new DateTime(firstInstallmentDate.Year, firstInstallmentDate.Month, firstInstallmentDate.Day).AddMonths(-1);
            }

            return state with
            {
                SigningDay = signingDay,
                FirstInstallmentDate = firstInstallmentDate,
                CreditOpening = creditOpening,
            };
        }

        [ReducerMethod]
        public static CalculationState IndexValueFetchedReducer(CalculationState state, IndexValueFetched action)
        {
            return state with { WarsawInterbankOfferedRate = action.Value };
        }

        [ReducerMethod]
        public static CalculationState ScheduleCalculatedReducer(CalculationState state, ScheduleCalculated action)
        {
            return state with { ScheduleInstallmentDetails = action.ScheduleInstallmentsDetails };
        }
    }
}