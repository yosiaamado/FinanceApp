using FinanceApp.Api.Model.DTO;
using FinanceApp.Api.Model.Transaction;

namespace FinanceApp.Api.IService
{
    public interface ITransactionService
    {
        Task<bool> AddTransaction(TransactionRequest request);
        Task<bool> AddSyncTransaction(Transaction request);
    }
}
