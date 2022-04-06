using System;
using System.Globalization;

namespace Wibor.Scrapper
{
    public class Wibor
    {
        public Wibor(string date, string value)
        {
            Date = DateTime.ParseExact(date, "yyyy-MM-dd", null);
            Value = decimal.Parse(value, new NumberFormatInfo() { NumberDecimalSeparator = "." });
        }

        public Wibor(DateTime date, decimal value)
        {
            Date = date;
            Value = value;
        }

        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }
}