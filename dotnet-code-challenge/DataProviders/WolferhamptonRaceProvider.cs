using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dotnet_code_challenge.Models;
using dotnet_code_challenge.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace dotnet_code_challenge.DataProviders
{
    public class WolferhamptonRaceProvider : IHorsePriceProvider
    {
        private const string ProviderName = "WolferhamptonRaceFile";
        private readonly DataSourcesConfig _configs;
        private RaceDataSource _settings;
        public WolferhamptonRaceProvider(IOptions<DataSourcesConfig> options)
        {
            this._configs = options.Value;
        }

        public bool CanProcess()
        {
            foreach (var config in _configs.DataSources)
            {
                if (!string.IsNullOrEmpty(config.Name) && ProviderName.Equals(config.Name))
                {
                    _settings = config;
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<HorsePrice>> GetHorsePrices()
        {
            _settings.FilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), _settings.FilePath);

            FileStream fileStream = new FileStream(_settings.FilePath, FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = await reader.ReadToEndAsync();
                var stuff = JObject.Parse(line);
                var horses = from p in stuff["RawData"]["Markets"][0]["Selections"]
                             select new HorsePrice { Price = (double)p["Price"], HorseName = (string)p["Tags"]["name"] };

                return horses;
            }

        }
    }
}
