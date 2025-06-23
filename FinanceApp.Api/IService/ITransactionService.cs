using FinanceApp.Api.Model;
using FinanceApp.Api.Model.DTO;

namespace FinanceApp.Api.IService
{
    public interface ITransactionService
    {
        Task<bool> AddTransaction(TransactionRequest request);
        Task<bool> AddSyncTransaction(Transaction request);
    }
}
