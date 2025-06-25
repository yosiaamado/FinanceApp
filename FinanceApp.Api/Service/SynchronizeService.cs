using FinanceApp.Api.Database;
using FinanceApp.Api.IService;
using FinanceApp.Api.Model;
using FinanceApp.Shared;
using Microsoft.EntityFrameworkCore;

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
                var itemId = await _context.Items.Where(i => i.Name == trx.ItemName).Select(i => i.Id).FirstOrDefaultAsync();

                if(itemId == 0)
                {
                    var newTempItem = new TempItem
                    {
                        Name = trx.ItemName,
                        CreatedAt = DateTime.Now,
                        IsReviewed = false,
                        MovedToItemTable = false
                    };

                    await _context.TempItems.AddAsync(newTempItem);
                    await _context.SaveChangesAsync();
                }

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

                if (itemId != 0)
                    entity.ItemId = itemId;

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
