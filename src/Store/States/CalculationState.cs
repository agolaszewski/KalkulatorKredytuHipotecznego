using System;
using KalkulatorKredytuHipotecznego.Store.Features.CalculationFeature.Actions;

namespace KalkulatorKredytuHipotecznego.Store.States
{
    public record CalculationState : BaseState<CalculationState>
    {
        public CalculationState()
        {
            _creditAmount = 0;
            _creditPeriods = 0;
            _creditPeriodType = PeriodType.Years;
            _margin = 0;
            _warsawInterbankOfferedRate = 0;
            _firstInstalmentDate = DateTime.Today;
            _instalmentType = InstalmentType.Flat;
        }

        private decimal _creditAmount;
        public decimal CreditAmount
        {
            get => _creditAmount;
            set => OnChange(ref _creditAmount, value);
        }

        private int _creditPeriods;
        public int CreditPeriods
        {
            get => _creditPeriods;
            set => OnChange(ref _creditPeriods, value);
        }

        private PeriodType _creditPeriodType;
        public PeriodType CreditPeriodType
        {
            get => _creditPeriodType;
            set => OnChange(ref _creditPeriodType, value, new CreditPeriodTypeChanged());
        }

        private decimal _margin;
        public decimal Margin
        {
            get => _margin;
            set => OnChange(ref _margin, value);
        }

        private decimal _warsawInterbankOfferedRate;
        public decimal WarsawInterbankOfferedRate
        {
            get => _warsawInterbankOfferedRate;
            set => OnChange(ref _warsawInterbankOfferedRate, value);
        }

        private DateTime _firstInstalmentDate;
        public DateTime FirstInstalmentDate
        {
            get => _firstInstalmentDate;
            set => OnChange(ref _firstInstalmentDate, value);
        }

        private DateTime _creditOpening;
        public DateTime CreditOpening
        {
            get => _creditOpening;
            set => OnChange(ref _creditOpening, value);
        }

        private InstalmentType _instalmentType;
        public InstalmentType InstalmentType
        {
            get => _instalmentType;
            set => OnChange(ref _instalmentType, value);
        }
    }

    public enum PeriodType
    {
        Months = 1,
        Years = 2
    }

    public enum InstalmentType
    {
        Flat = 1,
        Decreasing = 2
    }
}