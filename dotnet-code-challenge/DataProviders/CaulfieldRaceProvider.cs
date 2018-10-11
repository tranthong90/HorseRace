using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using dotnet_code_challenge.Models;
using dotnet_code_challenge.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;


namespace dotnet_code_challenge.DataProviders
{
    public class CaulfieldRaceProvider : IHorsePriceProvider
    {
        private const string ProviderName = "CaulfieldRaceFile";
        private readonly DataSourcesConfig _configs;
        private RaceDataSource _settings;
        public CaulfieldRaceProvider(IOptions<DataSourcesConfig> options)
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

            using (XmlReader reader = XmlReader.Create(_settings.FilePath,new XmlReaderSettings { Async = true}))
            {
                XDocument doc = await XDocument.LoadAsync(reader, LoadOptions.None, new System.Threading.CancellationToken());

                var horses = from h in doc.Root.Elements("races").Elements("race").Elements("horses").Elements("horse")
                             join p in doc.Root.Elements("races").Elements("race").Elements("prices").Elements("price").Elements("horses").Elements("horse")
                             on (string)h.Element("number") equals (string)p.Attribute("number")
                             select new HorsePrice
                             {
                                 HorseName = (string)h.Attribute("name"),
                                 Price = (double)p.Attribute("Price")
                             };

                return horses;
            }

        }
    }
}
