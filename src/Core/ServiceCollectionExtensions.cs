using Microsoft.Extensions.DependencyInjection;
using Provider.Indexes;

namespace Core
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        }
    }
}