using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_code_challenge.DataProviders;
using dotnet_code_challenge.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.IO;
using System.Reflection;
using dotnet_code_challenge.Settings;
using Microsoft.Extensions.Configuration;

namespace dotnet_code_challenge
{
    class Program
    {
        public static void Main(string[] args)
        {
            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // run app
            serviceProvider.GetService<App>().Run().Wait();

            Console.ReadKey();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // add logging
            serviceCollection.AddSingleton(new LoggerFactory()
                .AddConsole());
            serviceCollection.AddLogging();

            // build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            serviceCollection.AddOptions();
            serviceCollection.Configure<DataSourcesConfig>(configuration.GetSection("RaceDataSources"));

            // add services
            serviceCollection.AddSingleton<IHorsePriceProvider, WolferhamptonRaceProvider>();
            serviceCollection.AddSingleton<IHorsePriceProvider, CaulfieldRaceProvider>();

            serviceCollection.AddTransient<App>();
        }



    }

}
