using System;

namespace KalkulatorKredytuHipotecznego.Domain;

public class DecreasingInstallmentsCalculationStrategy : IInstallmentCalculationStrategy
{
    public Installment Execute(CreditAmount creditAmount, CreditPeriods creditPeriods, Interest interest)
    {
        decimal q = 1 + (interest.Value / 12);
        decimal r = (decimal)Math.Pow((double)q, creditPeriods.Value);
        decimal upper = creditAmount.Value * r * (interest.Value - 1);
        decimal lower = r - 1;
        decimal result = upper / lower;
        return new Installment(result);
    }
}