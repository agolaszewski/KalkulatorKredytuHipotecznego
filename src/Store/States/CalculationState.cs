using KalkulatorKredytuHipotecznego.Store.Features.CalculationFeature.Actions;
using System;

namespace KalkulatorKredytuHipotecznego.Store.States
{
    public record CalculationState : BaseState<CalculationState>
    {
        public CalculationState()
        {
            var today = DateTime.Today;

            _creditAmount = 0;
            _creditPeriods = 0;
            _creditPeriodType = PeriodType.Years;
            _margin = 0;
            _warsawInterbankOfferedRate = 0;
            _firstInstalmentDate = today;
            _creditOpening = new DateTime(today.Year, today.Month, 1);
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
            set => OnChange(ref _margin, value, new MarginValueChanged());
        }

        private decimal _warsawInterbankOfferedRate;
        public decimal WarsawInterbankOfferedRate
        {
            get => _warsawInterbankOfferedRate;
            set => OnChange(ref _warsawInterbankOfferedRate, value, new WarsawInterbankOfferedRateValueChanged());
        }

        private DateTime _firstInstalmentDate;
        public DateTime FirstInstalmentDate
        {
            get => _firstInstalmentDate;
            set => OnChange(ref _firstInstalmentDate, value, new FirstInstalmentDateValueChanged());
        }

        private DateTime _creditOpening;
        public DateTime CreditOpening
        {
            get => _creditOpening;
            set => OnChange(ref _creditOpening, value, new CreditOpeningValueChanged());
        }

        private InstalmentType _instalmentType;
        public InstalmentType InstalmentType
        {
            get => _instalmentType;
            set => OnChange(ref _instalmentType, value);
        }

        public decimal TotalMargin { get; set; }
    }
}