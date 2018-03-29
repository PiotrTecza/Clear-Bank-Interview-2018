using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public interface IPaymentValitadionService
    {
        bool ValidatePayment(Account account, MakePaymentRequest request);
    }
}