using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentValidationStrategies
{
    public class ChapsValidationStrategy : IPaymentValidationStrategy
    {
        public PaymentScheme PaymentScheme => PaymentScheme.Chaps;

        public bool ValidatePayment(Account account, MakePaymentRequest request)
        {
            return account != null && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) && account.Status == AccountStatus.Live;
        }
    }
}
