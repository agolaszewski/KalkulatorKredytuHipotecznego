using Microsoft.Extensions.DependencyInjection;

namespace Provider.Indexes
{
    public static class ServiceCollectionExtensions
    {
        public static async Task AddIndexProviders(this IServiceCollection services, IDateTimeProvider dateTimeProvider, Holidays.Holidays holidays)
        {
            var httpClient = new HttpClient();

            var map = new Dictionary<Index, IIndexProvider>
            {
                { Index.Wibor3M, await IndexProvider.Build(dateTimeProvider, httpClient,Index.Wibor3M,holidays) },
                { Index.Wibor6M, await IndexProvider.Build(dateTimeProvider, httpClient,Index.Wibor6M,holidays) }
            };
            services.AddSingleton(new IndexProviderFactory(map));
        }
    }
}