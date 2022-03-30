namespace KalkulatorKredytuHipotecznego.Domain;

public partial record Months : ValueObject<int>
{
    public static Months From(Years years)
    {
        int value = years.Value * 12;
        return new Months(value);
    }
}