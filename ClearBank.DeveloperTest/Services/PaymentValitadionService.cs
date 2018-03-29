using System.Collections.Generic;
using System.Linq;
using ClearBank.DeveloperTest.Services.PaymentValidationStrategies;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentValitadionService : IPaymentValitadionService
    {
        private readonly IEnumerable<IPaymentValidationStrategy> _paymentValidationStrategies;

        public PaymentValitadionService(IEnumerable<IPaymentValidationStrategy> paymentValidationStrategies)
        {
            _paymentValidationStrategies = paymentValidationStrategies;
        }

        public bool ValidatePayment(Account account, MakePaymentRequest request)
        {
            var validStrategies = _paymentValidationStrategies.Where(x => x.PaymentScheme == request.PaymentScheme).ToList();
            return validStrategies.Count > 0 && validStrategies.All(x => x.ValidatePayment(account, request));
        }
    }
}
