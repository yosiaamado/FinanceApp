using FinanceApp.Api.Database;
using FinanceApp.Api.Model;

namespace FinanceApp.Api.Service
{
    public class SummaryService
    {
        public readonly AppDbContext _context;

        public SummaryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MonthlyCashflow> GetCashflowMonthly(int? month = null, int? year = null)
        {
            var result = await MonthlyCashflow.LoadAsync(_context, month, year);

            return result;
        }

        public async Task<CategoryChart> GetChartPerCategory(int? month = null, int? year = null)
        {
            var result = await CategoryChart.LoadMonthlyExpenseCategoryChart(_context, month, year);

            return result;
        }
    }
}
