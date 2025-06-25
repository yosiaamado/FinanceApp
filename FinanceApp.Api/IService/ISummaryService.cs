using FinanceApp.Api.Model;

namespace FinanceApp.Api.IService
{
    public interface ISummaryService
    {
        Task<MonthlyCashflow> GetCashflowMonthly(int? month = null, int? year = null);
        Task<CategoryChart> GetChartPerCategory(int? month = null, int? year = null);
    }
}
