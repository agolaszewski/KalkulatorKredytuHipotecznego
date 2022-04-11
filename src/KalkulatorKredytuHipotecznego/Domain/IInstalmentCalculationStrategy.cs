namespace KalkulatorKredytuHipotecznego.Domain;

public interface IInstallmentCalculationStrategy
{
    Installment Execute(CreditAmount creditAmount, CreditPeriods creditPeriods, Interest interest);
}