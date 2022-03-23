namespace KalkulatorKredytuHipotecznego.Domain;

public record CreditPeriods : ValueObject<int>
{
    public CreditPeriods(Years years)
    {
        Value = years.ToMonths().Value;
    }

    public CreditPeriods(Months months)
    {
        Value = months.Value;
    }

    public static implicit operator CreditPeriods(Years value)
    {
        return new CreditPeriods(value);
    }

    public static implicit operator CreditPeriods(Months value)
    {
        return new CreditPeriods(value);
    }
}