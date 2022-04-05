using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Wibor.Scrapper.Runner // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = false
        };

        private static async Task Main(string[] args)
        {
            DateTime from = DateTime.ParseExact(args[0], "yyyy-MM-dd", null);
            DateTime to = DateTime.ParseExact(args[1], "yyyy-MM-dd", null);

            IReadOnlyList<Wibor> output = await new Scrapper().ExecuteAsync(from, to, args[2]);

            await using var writer = Console.Out;
            await using var csv = new CsvWriter(writer, config);
            await csv.WriteRecordsAsync(output);
        }
    }
}