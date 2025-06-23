using AutoMapper;
using FinanceApp.Api.Database;
using FinanceApp.Api.IService;
using FinanceApp.Api.Model.DTO;
using FinanceApp.Api.Model;
using Microsoft.EntityFrameworkCore;

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
