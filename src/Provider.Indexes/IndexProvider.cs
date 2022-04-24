using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

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
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly DateTime _minDate;

    internal IndexProvider(
        IDateTimeProvider dateTimeProvider,
        Dictionary<DateTime, decimal> data,
        Holidays.Holidays holidays,
        DateTime minDate)
    {
        _data = data;
        _holidays = holidays;
        _dateTimeProvider = dateTimeProvider;
        _minDate = minDate;
    }

    public static async Task<IndexProvider> Build(IDateTimeProvider dateTimeProvider, HttpClient httpClient, Index index, Holidays.Holidays holidays)
    {
        var file = await httpClient.GetAsync($"https://kalkulatorkredytublob.blob.core.windows.net/wibor/{index.Name}.csv");

        using var reader = new StreamReader(await file.Content.ReadAsStreamAsync());
        using var csv = new CsvReader(reader, Config);

        var records = csv.GetRecords<IndexData>().ToList();
        var minDate = records.Min(x => x.Date);
        var data = records.ToDictionary(x => x.Date, x => x.Value);
        return new IndexProvider(dateTimeProvider, data, holidays, minDate);
    }

    private record IndexData
    {
        public DateTime Date { get; init; }

        public decimal Value { get; init; }
    }

    public decimal GetValue(DateTime date)
    {
        date = date.Date;

        if (date <= _minDate)
        {
            return _data[date];
        }

        if (date > _dateTimeProvider.Today)
        {
            return GetValue(_dateTimeProvider.Today);
        }

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