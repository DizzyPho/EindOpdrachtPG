using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MultipleChoiceGUI.Config
{
    public static class ConfigurationService
    {
        private static IConfigurationRoot _config;
        static ConfigurationService()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                           .AddJsonFile(@"./Config/appsettings.json", 
                                           optional: false, reloadOnChange: true);
            _config = builder.Build();
        }

        public static string GetConnectionString(string name)
        {
            return _config.GetConnectionString(name);
        }

        public static string GetSetting(string name)
        {
            return _config.GetSection("AppSettings")[name];
        }
    }
}
