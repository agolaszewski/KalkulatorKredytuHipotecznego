using Calculator.Schedule;
using KalkulatorKredytuHipotecznego.Store.Features.CalculationFeature.Actions;
using Provider.Indexes;
using System;
using System.Collections.Generic;

namespace KalkulatorKredytuHipotecznego.Store.States
{
    public record CalculationState : BaseState<CalculationState>
    {
        public CalculationState(IndexProviderFactory _indexProviderFactory)
        {
            var indexProvider = _indexProviderFactory.IndexProvider(Provider.Indexes.Index.Wibor3M);
            var today = DateTime.Today;

            _creditAmount = 160000;
            _creditPeriods = 120;
            _creditPeriodType = PeriodType.Months;
            _margin = 2.1M;

            _signingDay = new DateTime(today.Year, today.Month, 1);
            _creditOpening = new DateTime(today.Year, today.Month, 1);
            _firstInstallmentDate = today;

            _warsawInterbankOfferedRate = indexProvider.GetValue(new DateTime(_signingDay.Year, _signingDay.Month, 1).AddDays(-1));
            _installmentType = InstallmentType.Flat;
            _warsawInterbankOfferedRatePeriod = 3;

            TotalMargin = _margin + _warsawInterbankOfferedRate;
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

        private DateTime _firstInstallmentDate;
        public DateTime FirstInstallmentDate
        {
            get => _firstInstallmentDate;
            set => OnChange(ref _firstInstallmentDate, value, new FirstInstallmentDateValueChanged());
        }

        private DateTime _creditOpening;
        public DateTime CreditOpening
        {
            get => _creditOpening;
            set => OnChange(ref _creditOpening, value, new CreditOpeningValueChanged());
        }

        private InstallmentType _installmentType;
        public InstallmentType InstallmentType
        {
            get => _installmentType;
            set => OnChange(ref _installmentType, value);
        }

        private int _warsawInterbankOfferedRatePeriod;
        public int WarsawInterbankOfferedRatePeriod
        {
            get => _warsawInterbankOfferedRatePeriod;
            set => OnChange(ref _warsawInterbankOfferedRatePeriod, value);
        }

        private DateTime _signingDay;
        public DateTime SigningDay
        {
            get => _signingDay;
            set => OnChange(ref _signingDay, value, new SigningDayValueChanged());
        }

        public decimal TotalMargin { get; set; }

        public IReadOnlyList<InstallmentDetails> ScheduleInstallmentDetails { get; set; } = new List<InstallmentDetails>();
    }
}