using System;

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

    public record CreditPeriods : ValueObject<Months>
    {
        public CreditPeriods(Years years)
        {
            Value = years.ToMonths();
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
            throw new NotImplementedException();
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

    public record Instalment : ValueObject<decimal>
    {
        
    }
}