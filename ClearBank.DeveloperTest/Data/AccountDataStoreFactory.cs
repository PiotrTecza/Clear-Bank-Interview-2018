using ClearBank.DeveloperTest.Configuration;

namespace ClearBank.DeveloperTest.Data
{
    public class AccountDataStoreFactory : IAccountDataStoreFactory
    {
        public const string BackupDataStoreType = "Backup";
        private readonly IConfiguration _configuration;

        public AccountDataStoreFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IAccountDataStore GetAccountDataStore()
        {
            if (_configuration.DataStoreType == BackupDataStoreType)
            {
                return new BackupAccountDataStore();
            }

            return new AccountDataStore();
        }
    }
}
