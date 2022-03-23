namespace KalkulatorKredytuHipotecznego.Domain;

public record Years : ValueObject<int>
{
    private Years()
    {
            
    }

    public Years(int value)
    {
        Value = value;
    }

    public static Years From(Months months)
    {
        return new Years() { Value = months.Value / 12 };
    }

    public Months ToMonths()
    {
        return Months.From(this);
    }
}