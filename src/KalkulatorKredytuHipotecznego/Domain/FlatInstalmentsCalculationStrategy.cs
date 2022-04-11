using Microsoft.VisualBasic;

namespace KalkulatorKredytuHipotecznego.Domain;

public class FlatInstallmentsCalculationStrategy : IInstallmentCalculationStrategy
{
    public Installment Execute(CreditAmount creditAmount, CreditPeriods creditPeriods, Interest interest)
    {
        var result = -Financial.Pmt((double)(interest.Value / 12M), creditPeriods.Value, (double)creditAmount.Value);
        return new Installment((decimal)result);
    }
}