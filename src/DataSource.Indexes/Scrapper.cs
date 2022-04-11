using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataSource.Indexes
{
    public class Scrapper
    {
        private readonly HttpClient _httpClient;

        public Scrapper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<Index>> ExecuteAsync(DateTime from, DateTime to, string type)
        {
            if (from > to)
            {
                throw new ArgumentException("from cannot be earlier then to");
            }

            var url = new Uri($"https://stooq.pl/q/d/l/?s={type}&d1={@from:yyyyMMdd}&d2={to:yyyyMMdd}&c=1");
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.StatusCode.ToString());
            }

            var result = new List<Index>();

            using (StringReader reader = new StringReader(await response.Content.ReadAsStringAsync()))
            {
                string line = await reader.ReadLineAsync(); //skip header
                do
                {
                    line = await reader.ReadLineAsync();
                    if (line != null)
                    {
                        var columns = line.Split(';');
                        result.Add(new Index(columns[0], columns[1]));
                    }
                } while (line != null);
            }


            return result;
        }
    }
}