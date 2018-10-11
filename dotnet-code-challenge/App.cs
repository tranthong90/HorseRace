using dotnet_code_challenge.DataProviders;
using dotnet_code_challenge.Models;
using dotnet_code_challenge.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_code_challenge
{
    public class App
    {
        private readonly IEnumerable<IHorsePriceProvider> _providers;
        private readonly ILogger<App> _logger;
        private readonly DataSourcesConfig _config;

        public App(IEnumerable<IHorsePriceProvider> providers, ILogger<App> logger, IOptions<DataSourcesConfig> options)
        {
            _providers = providers;
            _logger = logger;
            _config = options.Value;
        }
        public async Task Run()
        {

            List<HorsePrice> horsePrices = new List<HorsePrice>();

            foreach (var provider in _providers)
            {
                if (provider.CanProcess())
                    horsePrices.AddRange(await provider.GetHorsePrices());
            }

            //order the list by name and print out
            horsePrices = horsePrices.OrderBy(x => x.Price).ToList();
            foreach (var price in horsePrices)
            {
                Console.WriteLine($"Horse Name: {price.HorseName} . Price: {price.Price}");
            }
        }
    }
}
