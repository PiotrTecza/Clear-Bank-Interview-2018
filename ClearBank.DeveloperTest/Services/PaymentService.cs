using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStoreFactory _accountDataStoreFactory;
        private readonly IPaymentValitadionService _paymentValitadionService;

        public PaymentService(IAccountDataStoreFactory accountDataStoreFactory, IPaymentValitadionService paymentValitadionService)
        {
            _accountDataStoreFactory = accountDataStoreFactory;
            _paymentValitadionService = paymentValitadionService;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var accountDataStore = _accountDataStoreFactory.GetAccountDataStore();
            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            var result = new MakePaymentResult();
            result.Success = _paymentValitadionService.ValidatePayment(account, request);

            if (result.Success)
            {
                account.Balance -= request.Amount;
                accountDataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}

/*
Fixed bugs
1. Payment result being always false
2. Marking AllowedPaymentSchemes with Flags attribute

Potential bugs (don't know the business logic so didn't attempt to fix them)
1. Debtor account charged but money not transfered to creditor account
2. If above is valid then there should be a transaction wrapping these both operations
3. Account should be locked for the time of the opperation, otherwise storing new balance is not safe

Notes
1. I'm not veryfying constructor parameteras cause I'd use IoC container which should verify that
2. With IoC container in place I would register correct AccountDataSTore and AccountDataStoreFactory wouldn't be needed.
3. I'm assuming this is part of a bigger system and modyfying interfaces is not possible. Otherwise I'd replace MakePaymentRequest with bool
*/
