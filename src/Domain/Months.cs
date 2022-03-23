namespace KalkulatorKredytuHipotecznego.Domain;

public record Months : ValueObject<int>
{
    public static Months From(Years years)
    {
        return new Months() { Value = years.Value * 12 };
    }
}