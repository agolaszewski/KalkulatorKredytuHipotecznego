using System;
using Microsoft.VisualBasic;

namespace KalkulatorKredytuHipotecznego.Domain;

public class DecreasingInstalmentsCalculationStrategy : IInstalmentCalculationStrategy
{
    public Instalment Execute(CreditAmount creditAmount, CreditPeriods creditPeriods, Interest interest, Days instalmentPeriod)
    {
        decimal q = 1 + (interest.Value / 12);
        decimal r = (decimal)Math.Pow((double)q, creditPeriods.Value);
        decimal upper = creditAmount.Value * r * (interest.Value - 1);
        decimal lower = r - 1;
        decimal result = upper / lower;
        return new Instalment(result, interest, creditAmount, instalmentPeriod);
    }
}

public class FlatInstalmentsCalculationStrategy : IInstalmentCalculationStrategy
{
    public Instalment Execute(CreditAmount creditAmount, CreditPeriods creditPeriods, Interest interest, Days instalmentPeriod)
    {
        var result = -Financial.Pmt((double)(interest.Value / 12M), creditPeriods.Value, (double)creditAmount.Value);
        result = Math.Round(result, 2);
        return new Instalment((decimal)result, interest, creditAmount, instalmentPeriod);
    }
}