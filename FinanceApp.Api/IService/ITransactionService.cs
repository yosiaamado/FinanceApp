using FinanceApp.Api.Model;
using FinanceApp.Shared;

namespace FinanceApp.Api.IService
{
    public interface ITransactionService
    {
        Task<PagedResult<Transaction>> GetTransactions(int page = 1, int pageSize = 10, DateTime? startDate = null, DateTime? endDate = null, string? item = null);
        Task<Transaction> GetTransaction(Int64 transactionId);
        Task<bool> AddTransaction(TransactionRequest request);
        Task<bool> AddSyncTransaction(Transaction request);
    }
}
