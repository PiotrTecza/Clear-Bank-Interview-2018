using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentValidationStrategies
{
    public interface IPaymentValidationStrategy
    {
        PaymentScheme PaymentScheme { get; }

        bool ValidatePayment(Account account, MakePaymentRequest request);
    }
}