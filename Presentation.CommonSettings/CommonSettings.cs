using Common.DTOs.Settings;
using Microsoft.Extensions.Configuration;

namespace Presentation.CommonSettings
{
    public static class CommonSettings
    {
        public static IConfiguration ConfigurationContainer { get; private set; }

        static CommonSettings()
        {
            ConfigurationContainer = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile($"SharedSettings.json").Build();
        }

        public static string PolicyRules
        {
            get
            {
                return ConfigurationContainer["PolicyRules:Rule"];
            }
        }

        public static string HashToEncryption
        {
            get
            {
                return ConfigurationContainer["HashToEncryption:Value"];
            }
        }

        public static TokenValidationParametersData TokenValidationParameters
        {
            get
            {
                var tokenValidationParametersData = new TokenValidationParametersData();
                ConfigurationContainer.GetSection("TokenValidationParameters").Bind(tokenValidationParametersData);
                return tokenValidationParametersData;
            }
        }

        public static string[] AllowedOrigins
        {
            get
            {
                var allowedOriginsData = new AllowedOriginsData();
                ConfigurationContainer.GetSection("AllowedOrigins").Bind(allowedOriginsData);
                return allowedOriginsData.Origins.ToArray();
            }
        }

        public static string DeployType
        {
            get
            {
                try
                {
                    return ConfigurationContainer["DeployConnectionStrings:Deploy"];
                }
                catch (Exception)
                {
                    return "PROD";
                }
            }
        }

        public static string[] AllowedOriginsLocal
        {
            get
            {
                var allowedOriginsData = new AllowedOriginsData();
                ConfigurationContainer.GetSection("AllowedOriginsLocal").Bind(allowedOriginsData);
                return allowedOriginsData.Origins.ToArray();
            }
        }
    }
}
