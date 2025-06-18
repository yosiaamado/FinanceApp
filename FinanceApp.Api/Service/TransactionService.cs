using AutoMapper;
using FinanceApp.Api.Database;
using FinanceApp.Api.IService;
using FinanceApp.Api.Model.DTO;
using FinanceApp.Api.Model.Transaction;

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
            var trans = _mapper.Map<Transaction>(request);
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
