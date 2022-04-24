using Domain;
using Provider.Indexes;

namespace Calculator.Schedule
{
    public class ScheduleCalculator
    {
        private readonly IndexProviderFactory _indexProviderFactory;

        public ScheduleCalculator(IndexProviderFactory indexProviderFactory)
        {
            _indexProviderFactory = indexProviderFactory;
        }

        public IReadOnlyList<InstallmentDetails> Calculate(
            IInstallmentCalculationStrategy strategy,
            CreditAmount creditAmount,
            CreditOpening creditOpening,
            CreditPeriods creditPeriods,
            InstallmentDate installmentDate,
            Margin margin,
            WarsawInterbankOfferedRate wibInterbankOfferedRate,
            WarsawInterbankOfferedRatePeriod wiborInterbankOfferedRatePeriodRatePeriod)
        {
            var interestIndex = wiborInterbankOfferedRatePeriodRatePeriod == WarsawInterbankOfferedRatePeriod.Wibor3M
                ? Provider.Indexes.Index.Wibor3M
                : Provider.Indexes.Index.Wibor6M;

            IIndexProvider indexProvider = _indexProviderFactory.IndexProvider(interestIndex);

            DateTime from = creditOpening.Value;
            DateTime to = installmentDate.Value;

            var interest = new Interest(margin, wibInterbankOfferedRate);

            var installments = new List<InstallmentDetails>();
            Installment baseInstallment = strategy.Execute(creditAmount, creditPeriods, interest);

            int index = 0;
            while (creditPeriods.Value > 1)
            {
                if (index % wiborInterbankOfferedRatePeriodRatePeriod.Value == 0)
                {
                    decimal interestIndexValue = indexProvider.GetValue(from);
                    interest = new Interest(margin, interestIndexValue / 100);
                    baseInstallment = strategy.Execute(creditAmount, creditPeriods, interest);
                }

                var installmentPeriod = Days.Difference(from, to);
                var installment = new InstallmentDetails(++index, to, baseInstallment, interest, creditAmount, installmentPeriod);

                installments.Add(installment);

                creditPeriods = creditPeriods.Value - 1;
                creditAmount = creditAmount.Value - installment.Principal;

                from = to;
                to = from.AddMonths(1);
            }

            var lastInstallmentDetails = InstallmentDetails.LastInstallment(++index, to, interest, creditAmount, Days.Difference(from, to));
            installments.Add(lastInstallmentDetails);

            return installments;
        }
    }
}