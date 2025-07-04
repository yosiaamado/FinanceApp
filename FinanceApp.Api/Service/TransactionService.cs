using AutoMapper;
using FinanceApp.Api.Database;
using FinanceApp.Api.IService;
using FinanceApp.Api.Model;
using Microsoft.EntityFrameworkCore;
using FinanceApp.Shared;
using FinanceApp.Api.Helper;
using AIClassifierLib.Interface;
using Newtonsoft.Json;

namespace FinanceApp.Api.Service
{
    public class TransactionService : ITransactionService
    {
        public readonly IConfiguration _config;
        public readonly AppDbContext _context;
        public readonly IMapper _mapper;
        public readonly EngineHelper _engineHelper;
        public readonly IItemClassifierEngine _itemEngine;
        public readonly IServiceScopeFactory _scopeFactory;

        public TransactionService(IConfiguration configuration, AppDbContext context, IMapper mapper, EngineHelper engineHelper, IItemClassifierEngine itemEngine, IServiceScopeFactory scopeFactory)
        {
            _config = configuration;
            _context = context;
            _mapper = mapper;
            _engineHelper = engineHelper;
            _itemEngine = itemEngine;
            _scopeFactory = scopeFactory;
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
                    _ = Task.Run(async () =>
                    {
                        using var scope = _scopeFactory.CreateScope();

                        var engine = scope.ServiceProvider.GetRequiredService<EngineHelper>();

                        var requestClone = JsonConvert.DeserializeObject<TransactionRequest>(
                            JsonConvert.SerializeObject(request)
                        );

                        await _engineHelper.FillCategoryInTempItem(request);
                    });
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
