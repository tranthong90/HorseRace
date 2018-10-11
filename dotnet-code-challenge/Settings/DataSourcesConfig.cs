using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet_code_challenge.Settings
{
    public class DataSourcesConfig
    {
        public List<RaceDataSource> DataSources { get; set; }
    }

    public class RaceDataSource
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
    }
}
