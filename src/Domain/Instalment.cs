namespace KalkulatorKredytuHipotecznego.Domain;

public record Instalment
{
    public Instalment(decimal value, Interest interest, CreditAmount creditAmount, Days instalmentPeriod)
    {
        Total = value;
        Interest = creditAmount.Value * interest.Value * (instalmentPeriod.Value / 365M);
        Principal = Total - Interest;
    }

    public decimal Principal { get; set; }

    public decimal Interest { get; set; }

    public decimal Total { get; set; }
}