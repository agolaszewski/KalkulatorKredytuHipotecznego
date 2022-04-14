using Domain;

namespace Calculator.Schedule
{
    public class ScheduleCalculator
    {
        public List<InstallmentDetails> Calculate(IInstallmentCalculationStrategy strategy, CreditAmount creditAmount,
            CreditOpening creditOpening, CreditPeriods creditPeriods, InstallmentDate installmentDate,
            Interest interest, WarsawInterbankOfferedRatePeriod wiborInterbankOfferedRatePeriodRatePeriod)
        {
            var installments = new List<InstallmentDetails>();
            Installment baseInstallment = strategy.Execute(creditAmount, creditPeriods, interest);

            DateTime from = creditOpening.Value;
            DateTime to = installmentDate.Value;

            int index = 0;
            while (creditPeriods.Value > 1)
            {
                if (index % wiborInterbankOfferedRatePeriodRatePeriod.Value == 0)
                {
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