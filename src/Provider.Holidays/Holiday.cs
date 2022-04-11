namespace Provider.Holidays;

public record Holiday : IEquatable<DateTime>
{
    public Holiday(string name, int month, int day)
    {
        Name = name;
        Month = month;
        Day = day;
        IsStatic = true;
    }

    public Holiday(string name, int month, int day, int fromYear) : this(name, month, day)
    {
        FromYear = fromYear;
    }

    public Holiday(string name, int year, int month, int day, int fromYear = 0)
    {
        Name = name;
        Year = year;
        Month = month;
        Day = day;
        IsStatic = false;
        FromYear = fromYear;
    }

    public string Name { get; init; }

    public bool IsStatic { get; init; }

    public int Year { get; init; }

    public int Month { get; init; }

    public int Day { get; init; }

    public int FromYear { get; set; }

    public virtual bool Equals(DateTime other)
    {
        return other.Year >= FromYear && (IsStatic || Year == other.Year) && Month == other.Month && Day == other.Day;
    }
}