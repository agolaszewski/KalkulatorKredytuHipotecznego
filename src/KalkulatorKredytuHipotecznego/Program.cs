using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Core;
using Fluxor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Provider.Holidays;
using Provider.Indexes;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Calculator.Schedule;

namespace KalkulatorKredytuHipotecznego
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services
            .AddBlazorise(options =>
            {
                options.ChangeTextOnKeyPress = true;
                options.DelayTextOnKeyPress = true;
                options.DelayTextOnKeyPressInterval = 500;
            })
            .AddBootstrapProviders()
            .AddFontAwesomeIcons();

            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            builder.Services.AddCommonServices();
            await builder.Services.AddIndexProviders(new DateTimeProvider(), new Holidays());
            builder.Services.AddSingleton<ScheduleCalculator>();

            var currentAssembly = typeof(Program).Assembly;
            builder.Services.AddFluxor(options =>
            {
                options.ScanAssemblies(currentAssembly);
                options.UseReduxDevTools(reduxDevToolsMiddlewareOptions =>
                {
                    reduxDevToolsMiddlewareOptions.UseNewtonsoftJson();
                });
            });

            builder.RootComponents.Add<App>("#app");

            var host = builder.Build();

            await host.RunAsync();
        }
    }
}