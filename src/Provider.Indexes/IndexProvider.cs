using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Provider.Indexes;

public class IndexProvider : IIndexProvider
{
    private static readonly CsvConfiguration Config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        Delimiter = ";",
        HasHeaderRecord = false
    };

    private readonly Dictionary<DateTime, decimal> _data = new();
    private readonly Holidays.Holidays _holidays;

    internal IndexProvider(Dictionary<DateTime, decimal> data, Holidays.Holidays holidays)
    {
        _data = data;
        _holidays = holidays;
    }

    public static async Task<IndexProvider> Build(Index name, Holidays.Holidays holidays)
    {
        var file = await new HttpClient().GetAsync($"https://kalkulatorkredytublob.blob.core.windows.net/wibor/{name}.csv");

        using var reader = new StreamReader(await file.Content.ReadAsStreamAsync());
        using var csv = new CsvReader(reader, Config);

        var records = csv.GetRecords<(DateTime Date, decimal Value)>().ToList();
        var data = records.ToDictionary(x => x.Date, x => x.Value);
        return new IndexProvider(data, holidays);
    }

    public decimal GetValue(DateTime date)
    {
        date = date.Date;
        if (_holidays.IsFreeDay(date))
        {
            return GetValue(date.AddDays(-1));
        }

        if (_data.ContainsKey(date))
        {
            return _data[date];
        }

        return GetValue(date.AddDays(-1));
    }
}