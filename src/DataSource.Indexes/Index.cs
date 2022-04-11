using System;
using System.Globalization;

namespace DataSource.Indexes
{
    public class Index
    {
        public Index(string date, string value)
        {
            Date = DateTime.ParseExact(date, "yyyy-MM-dd", null);
            Value = decimal.Parse(value, new NumberFormatInfo() { NumberDecimalSeparator = "." });
        }

        public Index(DateTime date, decimal value)
        {
            Date = date;
            Value = value;
        }

        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }
}