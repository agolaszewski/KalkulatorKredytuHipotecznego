namespace KalkulatorKredytuHipotecznego.Domain;

public interface IInstalmentCalculationStrategy
{
    Instalment Execute(CreditAmount creditAmount, CreditPeriods creditPeriods, Interest interest, Days instalmentPeriod);
}