using ClearBank.DeveloperTest.Services.PaymentValidationStrategies;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services.PaymentValidationStrategies
{
    class BacsValidationStrategyUnitTests
    {
        private MakePaymentRequest _request;
        private Account _account;
        private BacsValidationStrategy _validationStrategy;

        [SetUp]
        public void Setup()
        {
            _request = new MakePaymentRequest();
            _account = new Account();

            _validationStrategy = new BacsValidationStrategy();
        }

        [Test]
        public void PaymentSchemeSetCorrectly()
        {
            Assert.That(_validationStrategy.PaymentScheme, Is.EqualTo(PaymentScheme.Bacs));
        }

        [Test]
        public void WhenAccountIsNullThenValidationReturnFalse()
        {
            _account = null;

            var result = _validationStrategy.ValidatePayment(_account, _request);

            Assert.IsFalse(result);
        }

        [TestCase(AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments)]
        [TestCase(AllowedPaymentSchemes.FasterPayments)]
        [TestCase(AllowedPaymentSchemes.Chaps)]
        public void WhenPaymentSchemeNotAllowedValidationReturnFalse(AllowedPaymentSchemes scheme)
        {
            _account.AllowedPaymentSchemes = scheme;

            var result = _validationStrategy.ValidatePayment(_account, _request);

            Assert.IsFalse(result);
        }

        [TestCase(AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments| AllowedPaymentSchemes.Bacs)]
        [TestCase(AllowedPaymentSchemes.FasterPayments| AllowedPaymentSchemes.Bacs)]
        [TestCase(AllowedPaymentSchemes.Chaps |  AllowedPaymentSchemes.Bacs)]
        [TestCase(AllowedPaymentSchemes.Bacs)]
        public void WhenPaymentSchemeAllowedValidationReturnTrue(AllowedPaymentSchemes scheme)
        {
            _account.AllowedPaymentSchemes = scheme;

            var result = _validationStrategy.ValidatePayment(_account, _request);

            Assert.IsTrue(result);
        }
    }
}
