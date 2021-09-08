using System;
using System.Collections.Generic;

namespace KalkulatorKredytuHipotecznego.Domain
{
    public record CreditAmount : ValueObject<decimal>
    {
    }

    public record Years : ValueObject<int>
    {
        public static Years From(Months months)
        {
            return new Years() { Value = months.Value / 12 };
        }

        public Months ToMonths()
        {
            return Months.From(this);
        }
    }

    public record Months : ValueObject<int>
    {
        public static Months From(Years years)
        {
            return new Months() { Value = years.Value * 12 };
        }
    }

    public record Days : ValueObject<int>
    {
        public Days(double value)
        {
            Value = (int)value;
        }

        public static Days Diffrance(CreditOpening creditOpening, InstalmentDate instalmentDate)
        {
            return new Days((instalmentDate.Value - creditOpening.Value).TotalDays);
        }
    }

    public record CreditPeriods : ValueObject<int>
    {
        public CreditPeriods(Years years)
        {
            Value = years.ToMonths().Value;
        }

        public CreditPeriods(Months months)
        {
            Value = months.Value;
        }
    }

    public record Margin : ValueObject<decimal>
    {
    }

    public record WarsawInterbankOfferedRate : ValueObject<decimal>
    {
    }

    public record Interest : ValueObject<decimal>
    {
        public Interest(Margin margin, WarsawInterbankOfferedRate warsawInterbankOfferedRate)
        {
            Value = margin.Value + warsawInterbankOfferedRate.Value;
        }
    }

    public record CreditOpening : ValueObject<DateTime>
    {
    }

    public record InstalmentDate : ValueObject<DateTime>
    {
    }

    public record InstalmentType : ValueObject<Store.States.InstalmentType>
    {
        private IInstalmentCalculationStrategy Strategy => Value == Store.States.InstalmentType.Decreasing ? new DecreasingInstalmentsCalculationStrategy() : null;
    }

    public class DecreasingInstalmentsCalculationStrategy : IInstalmentCalculationStrategy
    {
        public Instalment Execute(CreditAmount creditAmount, CreditPeriods creditPeriods, Interest interest, Days instalmentPeriod)
        {
            decimal q = 1 + (interest.Value / 12);
            decimal r = (decimal)Math.Pow((double)q, creditPeriods.Value);
            decimal upper = creditAmount.Value * r * (interest.Value - 1);
            decimal lower = r - 1;
            decimal result = upper / lower;
            return new Instalment(result, interest, creditAmount, instalmentPeriod);
        }
    }

    public interface IInstalmentCalculationStrategy
    {
        Instalment Execute(CreditAmount creditAmount, CreditPeriods creditPeriods, Interest interest, Days instalmentPeriod);
    }

    public record ValueObject<T>
    {
        public T Value { get; init; }
    }

    public record Instalment
    {
        public Instalment(decimal value, Interest interest, CreditAmount creditAmount, Days instalmentPeriod)
        {
            Total = value;
            Interest = creditAmount.Value * interest.Value * (instalmentPeriod.Value / 365M);
            Principal = Total - Interest;
        }

        public decimal Principal { get; set; }

        public decimal Interest { get; set; }

        public decimal Total { get; set; }
    }

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

    public class Holidays
    {
        private Dictionary<int, IReadOnlyList<Holiday>> _movable = new Dictionary<int, IReadOnlyList<Holiday>>();
        private IReadOnlyList<Holiday> _static;

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

        private bool IsHoliday(DateTime date)
        {
        }

        public bool IsFreeDay(DateTime date)
        {
            return IsSunday(date) || IsHoliday(date);
        }
    }
}