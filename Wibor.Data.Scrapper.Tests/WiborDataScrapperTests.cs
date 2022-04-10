using System;
using System.Net.Http;
using System.Threading.Tasks;
using KalkulatorKredytuHipotecznego;
using Xunit;

namespace Wibor.Data.Scrapper.Tests
{
    public class WiborDataScrapperTests
    {
        public WiborDataScrapperTests()
        {
        }

        [Fact]
        public async Task Check()
        {
            var scrapper = new Wibor.Scrapper.Scrapper(new HttpClient());
            await scrapper.ExecuteAsync(new DateTime(2021, 12, 1), new DateTime(2021, 12, 31), "plopln6m");
        }

        [Fact]
        public async Task Check2()
        {
            var wibor = await WiborProvider.Build();
            var xd = wibor.GetValue(DateTime.Today);
        }
    }
}