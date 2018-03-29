using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentValidationStrategies
{
    public class FasterPaymentValidationStrategy : IPaymentValidationStrategy
    {
        public PaymentScheme PaymentScheme => PaymentScheme.FasterPayments;

        public bool ValidatePayment(Account account, MakePaymentRequest request)
        {
            return account != null && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) && account.Balance >= request.Amount;
        }
    }
}
