namespace Provider.Holidays;

public class Holidays
{
    private readonly Dictionary<int, IReadOnlyList<Holiday>> _movable = new();
    private readonly IReadOnlyList<Holiday> _static;

    public Holidays()
    {
        _static = Static();
    }

    public IReadOnlyList<Holiday> Static()
    {
        var result = new List<Holiday>();
        result.Add(new Holiday("Nowy Rok", 1, 1));
        result.Add(new Holiday("Swięto Trzech Króli", 1, 6, 2011));
        result.Add(new Holiday("Pierwszy Maja", 5, 1));
        result.Add(new Holiday("Trzeci Maja", 5, 3));
        result.Add(new Holiday("Wniebowzięcie Najświętszej Marii Panny", 8, 15));
        result.Add(new Holiday("Wszystkich Świętych", 11, 1));
        result.Add(new Holiday("Narodowe Święto Niepodległości", 11, 11));
        result.Add(new Holiday("Pierwszy Dzień Bożego Narodzenia", 12, 25));
        result.Add(new Holiday("Drugi Dzień Bożego Narodzenia", 12, 26));
        return result;
    }

    private DateTime Eastern(int year)
    {
        // This algorithm was found at http://en.wikipedia.org/wiki/Computus
        //https://rextester.com/discussion/YBG86701/Easter-Holiday-Calculator
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = ((19 * a) + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + (2 * e) + (2 * i) - h - k) % 7;
        int m = (a + (11 * h) + (22 * l)) / 451;
        int n = h + l - (7 * m) + 114;
        int month = n / 31;
        int day = (n % 31) + 1;

        return new DateTime(year, month, day);
    }

    public IReadOnlyList<Holiday> Moveable(int year)
    {
        if (_movable.ContainsKey(year))
        {
            return _movable[year];
        }

        var result = new List<Holiday>();
        DateTime easternDate = Eastern(year);

        var holiday1 = easternDate.AddDays(1);
        result.Add(new Holiday("Drugi Dzień Wielkiej Nocy", holiday1.Year, holiday1.Month, holiday1.Day));

        var holiday2 = easternDate.AddDays(60); //Boże Ciało wypada zawsze w 60. dni po Wielkanocy.
        result.Add(new Holiday("Boże Ciało", holiday2.Year, holiday2.Month, holiday2.Day));

        _movable.Add(year, result);

        return result;
    }

    private bool IsSunday(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Sunday;
    }

    private bool IsSaturday(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday;
    }

    private bool IsHoliday(DateTime date)
    {
        return IsStaticHoliday(date) || IsMovableHoliday(date);
    }

    private bool IsStaticHoliday(DateTime date)
    {
        return _static.Any(holiday => holiday.Equals(date));
    }

    private bool IsMovableHoliday(DateTime date)
    {
        var holidays = Moveable(date.Year);
        return holidays.Any(holiday => holiday.Equals(date));
    }

    public bool IsFreeDay(DateTime date)
    {
        return IsSaturday(date) || IsSunday(date) || IsHoliday(date);
    }
}