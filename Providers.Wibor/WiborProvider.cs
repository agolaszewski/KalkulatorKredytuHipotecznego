using CsvHelper;
using CsvHelper.Configuration;
using KalkulatorKredytuHipotecznego.Domain;
using System.Globalization;

namespace KalkulatorKredytuHipotecznego
{
    public class WiborProvider
    {
        private static readonly CsvConfiguration Config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = false
        };

        private Dictionary<DateTime, decimal> _data = new();

        public WiborProvider(Dictionary<DateTime, decimal> data)
        {
            _data = data;
        }

        public static async Task<WiborProvider> Build()
        {
            var file = await new HttpClient().GetAsync("https://kalkulatorkredytublob.blob.core.windows.net/wibor/PLOPLN3M.csv");

            using (var reader = new StreamReader(await file.Content.ReadAsStreamAsync()))
            using (var csv = new CsvReader(reader, Config))
            {
                var records = csv.GetRecords<Data>().ToList();
                var data = records.ToDictionary(x => x.Date, x => x.Value);
                return new WiborProvider(data);
            }
        }

        public decimal GetValue(DateTime date)
        {
            date = date.Date;
            var holidays = new Holidays();
            if (holidays.IsFreeDay(date))
            {
                return GetValue(date.AddDays(-1));
            }

            if (_data.ContainsKey(date))
            {
                return _data[date];
            }

            return GetValue(date.AddDays(-1));
        }

        public class Data
        {
            public DateTime Date { get; set; }
            public decimal Value { get; set; }
        }
    }
}