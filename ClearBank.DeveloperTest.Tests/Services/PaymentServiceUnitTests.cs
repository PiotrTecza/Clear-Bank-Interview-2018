using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services
{
    [TestFixture]
    class PaymentServiceUnitTests
    {
        private MakePaymentRequest _request;
        private Account _account;
        private bool _validationResult;
        private decimal _accountBalanceBefore;
        private decimal _accountBalanceAfter;

        private Mock<IAccountDataStore> _accountDataStore;
        private Mock<IAccountDataStoreFactory> _accountDataStoreFactory;
        private Mock<IPaymentValitadionService> _paymentValitadionService;

        private PaymentService _paymentService;

        [SetUp]
        public void Setup()
        {
            _accountBalanceBefore = 100;

            _request = new MakePaymentRequest
            {
                DebtorAccountNumber = "ABC123",
                Amount = 10
            };

            _account = new Account
            {
                Balance = _accountBalanceBefore
            };

            _validationResult = true;

            _accountDataStore = new Mock<IAccountDataStore>();
            _accountDataStore.Setup(x => x.GetAccount(_request.DebtorAccountNumber)).Returns(_account);
            _accountDataStore.Setup(x => x.UpdateAccount(_account)).Callback<Account>(x => _accountBalanceAfter = x.Balance);

            _accountDataStoreFactory = new Mock<IAccountDataStoreFactory>();
            _accountDataStoreFactory.Setup(x => x.GetAccountDataStore()).Returns(_accountDataStore.Object);

            _paymentValitadionService = new Mock<IPaymentValitadionService>();
            _paymentValitadionService.Setup(x => x.ValidatePayment(_account, _request)).Returns(() => _validationResult);

            _paymentService = new PaymentService(_accountDataStoreFactory.Object, _paymentValitadionService.Object);
        }

        [Test]
        public void WhenValidationFailedThenPaymentNotDone()
        {
            _validationResult = false;

            _paymentService.MakePayment(_request);

            _accountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void WhenValidationFailedThenResultNotSuccessfull()
        {
            _validationResult = false;

            var result = _paymentService.MakePayment(_request);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void WhenValidationPassedThenPaymentDone()
        {
            _paymentService.MakePayment(_request);

            _accountDataStore.Verify(x => x.UpdateAccount(_account), Times.Once);
        }

        [Test]
        public void WhenValidationPassedThenPaymentSuccessfull()
        {
            var result = _paymentService.MakePayment(_request);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void WhenValidationPassedThenPaymentDoneWithCorrectBalance()
        {
            _paymentService.MakePayment(_request);

            Assert.That(_accountBalanceAfter, Is.EqualTo(_accountBalanceBefore - _request.Amount));
        }
    }
}
