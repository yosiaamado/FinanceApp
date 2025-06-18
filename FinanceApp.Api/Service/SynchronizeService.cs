using FinanceApp.Api.Database;
using FinanceApp.Api.Model.DTO;
using FinanceApp.Api.IService;
using FinanceApp.Api.Model.Transaction;

namespace FinanceApp.Api.Service
{
    public class SynchronizeService : ISynchronizeService
    {
        public readonly AppDbContext _context;
        public readonly ITransactionService _transService;

        public SynchronizeService(AppDbContext context, ITransactionService transactionService)
        {
            _context = context;
            _transService = transactionService;
        }

        public async Task<SyncResult> ProcessSingleSync(SyncRequest trx)
        {
            var result = new SyncResult { LocalId = trx.LocalId };

            try
            {
                var entity = new Transaction
                {
                    ItemName = trx.ItemName,
                    Amount = trx.Amount,
                    TransDate = trx.InputDate,
                    InputDate = trx.InputDate,
                    IsIncome = trx.IsIncome,
                    LocalId = trx.LocalId,
                    SyncDate = DateTime.Now
                };

                await _transService.AddSyncTransaction(entity);
                result.SyncStatus = SyncStatus.Synchronized;
            }
            catch (Exception ex)
            {
                result.SyncStatus = SyncStatus.Unsynchronized;
            }

            return result;
        }
    }
}
