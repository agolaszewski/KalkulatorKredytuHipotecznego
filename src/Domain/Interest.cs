namespace Domain;

public partial record Interest : ValueObject<decimal>
{
    public Interest(Margin margin, WarsawInterbankOfferedRate warsawInterbankOfferedRate)
    {
        Value = margin.Value + warsawInterbankOfferedRate.Value;
    }
}