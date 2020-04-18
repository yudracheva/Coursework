using Microsoft.Extensions.Options;
using Project.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Providers
{
    public class SettingsProvider
    {
        public string DatabaseUrl { get; private set; }

        public string ConnectionString { get; set; }

        public SettingsProvider(IOptions<AppSettings> appSettings)
        {
            DatabaseUrl = appSettings.Value.DatabaseUrl;
            if (String.IsNullOrEmpty(DatabaseUrl))
                DatabaseUrl = Path.Combine(Environment.CurrentDirectory, "dbase.db");

            ConnectionString = $@"URI=file:{DatabaseUrl}";
        }
    }
}
