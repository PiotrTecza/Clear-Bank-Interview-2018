using System.Collections.Generic;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Services.PaymentValidationStrategies;
using ClearBank.DeveloperTest.Types;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services
{
    class PaymentValitadionServiceUnitTests
    {
        private Account _account;
        private MakePaymentRequest _request;

        private Mock<IPaymentValidationStrategy> _bacsStrategy1Valid;
        private Mock<IPaymentValidationStrategy> _bacsStrategy2Valid;
        private Mock<IPaymentValidationStrategy> _bacsStrategy3Invalid;
        private Mock<IPaymentValidationStrategy> _chapsStrategy;

        private PaymentValitadionService _paymentValitadionService;

        [SetUp]
        public void Setup()
        {
            _account = new Account();
            _request = new MakePaymentRequest {PaymentScheme = PaymentScheme.Bacs};

            _bacsStrategy1Valid = new Mock<IPaymentValidationStrategy>();
            _bacsStrategy1Valid.SetupGet(x => x.PaymentScheme).Returns(PaymentScheme.Bacs);
            _bacsStrategy1Valid.Setup(x => x.ValidatePayment(_account, _request)).Returns(true);

            _bacsStrategy2Valid = new Mock<IPaymentValidationStrategy>();
            _bacsStrategy2Valid.SetupGet(x => x.PaymentScheme).Returns(PaymentScheme.Bacs);
            _bacsStrategy2Valid.Setup(x => x.ValidatePayment(_account, _request)).Returns(true);

            _bacsStrategy3Invalid = new Mock<IPaymentValidationStrategy>();
            _bacsStrategy3Invalid.SetupGet(x => x.PaymentScheme).Returns(PaymentScheme.Bacs); ;
            _bacsStrategy3Invalid.Setup(x => x.ValidatePayment(_account, _request)).Returns(false);

            _chapsStrategy = new Mock<IPaymentValidationStrategy>();
            _chapsStrategy.SetupGet(x => x.PaymentScheme).Returns(PaymentScheme.Chaps); ;
        }

        [Test]
        public void WhenAllValidStrategiesReturnTrueThenValidationPassed()
        {
            var strategies = new List<IPaymentValidationStrategy>
            {
                _bacsStrategy1Valid.Object,
                _bacsStrategy2Valid.Object,
                _chapsStrategy.Object
            };

            _paymentValitadionService = new PaymentValitadionService(strategies);

            var result = _paymentValitadionService.ValidatePayment(_account, _request);

            Assert.IsTrue(result);
        }

        [Test]
        public void WhenNotAllValidStrategiesReturnTrueThenValidationPassed()
        {
            var strategies = new List<IPaymentValidationStrategy>
            {
                _bacsStrategy1Valid.Object,
                _bacsStrategy3Invalid.Object,
                _chapsStrategy.Object
            };

            _paymentValitadionService = new PaymentValitadionService(strategies);

            var result = _paymentValitadionService.ValidatePayment(_account, _request);

            Assert.IsFalse(result);
        }
    }
}
