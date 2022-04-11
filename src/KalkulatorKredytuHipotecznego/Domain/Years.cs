namespace KalkulatorKredytuHipotecznego.Domain;

public partial record Years : ValueObject<int>
{
    public static Years From(Months months)
    {
        return new Years(months.Value / 12);
    }

    public Months ToMonths()
    {
        return Months.From(this);
    }
}