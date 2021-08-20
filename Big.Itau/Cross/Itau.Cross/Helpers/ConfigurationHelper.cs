using System.Configuration;

namespace Itau.Common.Helpers
{
    public class ConfigurationHelper
    {
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}