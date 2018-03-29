using ClearBank.DeveloperTest.Services.PaymentValidationStrategies;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services.PaymentValidationStrategies
{
    class ChapsValidationStrategyUnitTests
    {
        private MakePaymentRequest _request;
        private Account _account;
        private ChapsValidationStrategy _validationStrategy;

        [SetUp]
        public void Setup()
        {
            _request = new MakePaymentRequest();
            _account = new Account();

            _validationStrategy = new ChapsValidationStrategy();
        }

        [Test]
        public void PaymentSchemeSetCorrectly()
        {
            Assert.That(_validationStrategy.PaymentScheme, Is.EqualTo(PaymentScheme.Chaps));
        }

        [Test]
        public void WhenAccountIsNullThenValidationReturnFalse()
        {
            _account = null;

            var result = _validationStrategy.ValidatePayment(_account, _request);

            Assert.IsFalse(result);
        }

        [TestCase(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments)]
        [TestCase(AllowedPaymentSchemes.FasterPayments)]
        [TestCase(AllowedPaymentSchemes.Bacs)]
        public void WhenPaymentSchemeNotAllowedValidationReturnFalse(AllowedPaymentSchemes scheme)
        {
            _account.AllowedPaymentSchemes = scheme;

            var result = _validationStrategy.ValidatePayment(_account, _request);

            Assert.IsFalse(result);
        }

        [TestCase(AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments| AllowedPaymentSchemes.Bacs)]
        [TestCase(AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments)]
        [TestCase(AllowedPaymentSchemes.Chaps |  AllowedPaymentSchemes.Bacs)]
        [TestCase(AllowedPaymentSchemes.Chaps)]
        public void WhenPaymentSchemeAllowedValidationReturnTrue(AllowedPaymentSchemes scheme)
        {
            _account.AllowedPaymentSchemes = scheme;

            var result = _validationStrategy.ValidatePayment(_account, _request);

            Assert.IsTrue(result);
        }
    }
}
