using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Wibor.Scrapper
{
    public class Scrapper
    {
        public Scrapper()
        {
            NumberFormatInfo _numberFormatWithComma = new NumberFormatInfo();
            _numberFormatWithComma.NumberDecimalSeparator = ",";
        }

        private readonly NumberFormatInfo _numberFormatWithComma;

        public async Task<IReadOnlyList<Wibor>> ExecuteFor3MAsync(DateTime from, DateTime to)
        {
            return await ExecuteAsync(from, to, "WIBOR3M");
        }

        public async Task<IReadOnlyList<Wibor>> ExecuteFor6MAsync(DateTime from, DateTime to)
        {
            return await ExecuteAsync(from, to, "WIBOR6M");
        }

        private async Task<IReadOnlyList<Wibor>> ExecuteAsync(DateTime from, DateTime to, string type)
        {
            if ((to - from).TotalDays > 50)
            {
                throw new ArgumentException("Period between from and to cannot be larger then 50 days");
            }

            if (from > to)
            {
                throw new ArgumentException("from cannot be earlier then to");
            }

            var url =
                $"https://www.money.pl/pieniadze/depozyty/zlotowearch/{@from:yyyy-MM-dd},{to:yyyy-MM-dd},{type},strona,1.html";

            var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            var doc = await context.OpenAsync(new Url(url));
            var html = doc.DocumentElement.OuterHtml;
            var tbody = doc.QuerySelector(".tabela").QuerySelector("tbody");

            var result = new List<Wibor>();
            foreach (var tr in tbody.Children)
            {
                var date = tr.QuerySelector(".ac").InnerHtml;
                var value = tr.QuerySelector(".ar").InnerHtml;
                var wibor = new Wibor(date, value);

                result.Add(wibor);
            }

            return result;
        }
    }
}