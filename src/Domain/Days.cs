namespace Domain;

public partial record Days : ValueObject<int>
{
    public Days(double value)
    {
        Value = (int)value;
    }

    public static Days Difference(DateTime from, DateTime to)
    {
        return new Days((to - from).TotalDays);
    }
}