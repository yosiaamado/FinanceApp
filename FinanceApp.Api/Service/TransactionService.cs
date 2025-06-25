using AutoMapper;
using FinanceApp.Api.Database;
using FinanceApp.Api.IService;
using FinanceApp.Api.Model;
using Microsoft.EntityFrameworkCore;
using FinanceApp.Shared;

namespace FinanceApp.Api.Service
{
    public class TransactionService : ITransactionService
    {
        public readonly IConfiguration _config;
        public readonly AppDbContext _context;
        public readonly IMapper _mapper;

        public TransactionService(IConfiguration configuration, AppDbContext context, IMapper mapper)
        {
            _config = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<Transaction>> GetTransactions(int page = 1, int pageSize = 10, DateTime? startDate = null, DateTime? endDate = null, string? item = null)
        {
            startDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            endDate ??= DateTime.Now;

            var query = _context.Transactions
                .Where(t => t.TransDate >= startDate && t.TransDate <= endDate);

            if (!string.IsNullOrWhiteSpace(item))
                query = query.Where(t => t.ItemName.Contains(item));

            var transaction = await query
                .OrderByDescending(t => t.TransDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();

            return new PagedResult<Transaction>
            {
                Items = transaction,
                TotalItems = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<Transaction> GetTransaction(Int64 transactionId)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);

            return transaction ?? new Transaction();
        }

        public async Task<bool> AddTransaction(TransactionRequest request)
        {
            if (request.Item is null || string.IsNullOrWhiteSpace(request.Item.Name))
                return false;

            var trans = _mapper.Map<Transaction>(request);
            trans.InputDate = DateTime.Now;

            var existingItem = await _context.Items
                .FirstOrDefaultAsync(x => x.Name.ToLower() == request.Item.Name.ToLower());

            if(existingItem is not null)
            {
                trans.ItemId = request.Item.Id;
                trans.ItemName = request.Item.Name;
            }
            else
            {
                trans.ItemId = null;
                trans.ItemName = request.Item.Name;

                var alreadyTemp = await _context.TempItems
                    .AnyAsync(x => x.Name.ToLower() == request.Item.Name.ToLower());

                if (!alreadyTemp)
                {
                    var tempItem = new TempItem
                    {
                        Name = request.Item.Name,
                        IsReviewed = false,
                        CreatedAt = DateTime.Now,
                        MovedToItemTable = false
                    };
                    await _context.TempItems.AddAsync(tempItem);
                }
            }

            await _context.Transactions.AddAsync(trans);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddSyncTransaction(Transaction request)
        {
            await _context.Transactions.AddAsync(request);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
