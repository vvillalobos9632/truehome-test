using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Settings
{
    public interface IConnectionStringsSettings
    {
        public string this[string dbContextName] { get;set; }

        List<string> DBContextNames { get; }

    }
}
