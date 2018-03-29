using System.Configuration;

namespace ClearBank.DeveloperTest.Configuration
{
    public class Configuration : IConfiguration
    {
        public string DataStoreType => ConfigurationManager.AppSettings["DataStoreType"];
    }
}
