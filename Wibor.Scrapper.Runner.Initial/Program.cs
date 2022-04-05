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
            File.Create(@".\Wibor3M.csv");

            IReadOnlyList<Wibor> output = await new Scrapper().ExecuteAsync(188,"WIBOR3M");

            await using var stream = File.Open(@".\Wibor3M.csv", FileMode.Append);
            await using var writer = new StreamWriter(stream);
            await using var csv = new CsvWriter(writer, config);
            await csv.WriteRecordsAsync(output);
        }
    }
}