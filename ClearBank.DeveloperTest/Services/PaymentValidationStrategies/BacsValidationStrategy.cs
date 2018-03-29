using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentValidationStrategies
{
    public class BacsValidationStrategy : IPaymentValidationStrategy
    {
        public PaymentScheme PaymentScheme => PaymentScheme.Bacs;

        public bool ValidatePayment(Account account, MakePaymentRequest request)
        {
            return account != null && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
        }
    }
}
