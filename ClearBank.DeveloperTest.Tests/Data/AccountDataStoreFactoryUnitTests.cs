using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Data
{
    [TestFixture]
    class AccountDataStoreFactoryUnitTests
    {
        private string _dataStoreType;

        private Mock<IConfiguration> _configuration;
        private AccountDataStoreFactory _accountDataStoreFactory;

        [SetUp]
        public void Setup()
        {
            _dataStoreType = "";

            _configuration = new Mock<IConfiguration>();
            _configuration.SetupGet(x => x.DataStoreType).Returns(() => _dataStoreType);

            _accountDataStoreFactory = new AccountDataStoreFactory(_configuration.Object);
        }

        [Test]
        public void WhenDataStoreTypeIsNotBackendThenNormalAccountReturned()
        {
            var accountDataStore = _accountDataStoreFactory.GetAccountDataStore();

            Assert.That(accountDataStore, Is.TypeOf<AccountDataStore>());
        }

        [Test]
        public void WhenDataStoreTypeIsBackendThenNormalAccountReturned()
        {
            _dataStoreType = AccountDataStoreFactory.BackupDataStoreType;

            var accountDataStore = _accountDataStoreFactory.GetAccountDataStore();

            Assert.That(accountDataStore, Is.TypeOf<BackupAccountDataStore>());
        }
    }
}
