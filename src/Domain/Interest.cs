namespace KalkulatorKredytuHipotecznego.Domain;

public record Interest : ValueObject<decimal>
{
    private Interest()
    {
    }

    public Interest(Margin margin, WarsawInterbankOfferedRate warsawInterbankOfferedRate)
    {
        Value = margin.Value + warsawInterbankOfferedRate.Value;
    }

    public static implicit operator Interest(decimal value)
    {
        return new Interest() { Value = value / 100M };
    }
}