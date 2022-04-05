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

        public async Task<IReadOnlyList<Wibor>> ExecuteAsync(DateTime from, DateTime to, string type)
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

            return await GetWiborDataAsync(url);
        }

        public async Task<IReadOnlyList<Wibor>> ExecuteAsync(int maxPage,string type)
        {
            int page = maxPage;
            var result = new List<Wibor>();
            do
            {
                var url =
                    $"https://www.money.pl/pieniadze/depozyty/zlotowearch/1922-04-06,2022-04-06,{type},strona,{page}.html";

                Console.WriteLine($"Processing page no : {page}");
                var data = await GetWiborDataAsync(url);
                result.AddRange(data);
                page--;
            } while (page > 0);

            return result;
        }

        private async Task<List<Wibor>> GetWiborDataAsync(string url)
        {
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