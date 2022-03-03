using Common.DTOs.Settings;
using Common.Types.Utils;
using Presentation.CommonSettings;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Presentation.CommonSettings
{
    public sealed class ConnectionStringsSettings : IConnectionStringsSettings
    {
        private readonly Dictionary<string, string> connectionStrings = new Dictionary<string, string>();
        public ConnectionStringsSettings()
        {
            foreach (var section in CommonSettings.ConfigurationContainer.GetSection(CommonSettings.DeployType).GetChildren())
            {
                this[section.Key] = section.Value;
            }
        }

        public string this[string dbContextName]
        {
            get => connectionStrings[dbContextName];
            set => connectionStrings[dbContextName] = value;
        }

        public List<string> DBContextNames
        {
            get
            {
                var keyCollection = connectionStrings.Keys;
                return (from k in keyCollection
                        select k).ToList();
            }
        }
    }
}
