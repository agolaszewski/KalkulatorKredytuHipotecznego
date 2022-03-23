namespace KalkulatorKredytuHipotecznego.Domain;

public record Days : ValueObject<int>
{
    private Days()
    {
    }

    public Days(double value)
    {
        Value = (int)value;
    }

    public static Days Difference(CreditOpening creditOpening, InstalmentDate instalmentDate)
    {
        return new Days((instalmentDate.Value - creditOpening.Value).TotalDays);
    }

    public static implicit operator Days(int value)
    {
        return new Days() { Value = value };
    }
}