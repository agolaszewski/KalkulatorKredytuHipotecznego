using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace Wibor.Scrapper.Runner
{
    internal class Program
    {
        private static readonly CsvConfiguration Config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = false
        };

        private static readonly TypeConverterOptions CsvOptions = new TypeConverterOptions { Formats = new[] { "yyyy-MM-dd" } };

        private static async Task Main(string[] args)
        {
            DateTime from = DateTime.ParseExact(args[0], "yyyy-MM-dd", null);
            DateTime to = DateTime.ParseExact(args[1], "yyyy-MM-dd", null);

            var output = await new Scrapper(new HttpClient()).ExecuteAsync(from, to, "PLOPLN3M");

            await using var writer = Console.Out;
            await using var csv = new CsvWriter(writer, Config);
            csv.Context.TypeConverterOptionsCache.AddOptions<DateTime>(CsvOptions);
            await csv.WriteRecordsAsync(output);
        }
    }
}