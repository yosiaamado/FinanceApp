using FinanceApp.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Api.Model
{
    public class MonthlyCashflow
    {
        public decimal Income { get; set; }
        public decimal Outcome { get; set; }
        public static async Task<MonthlyCashflow> LoadAsync(AppDbContext db, int? month = null, int? year = null)
        {
            int targetMonth = month ?? DateTime.Now.Month;
            int targetYear = year ?? DateTime.Now.Year;

            var income = await db.Transactions
                .Where(t => t.IsIncome && t.TransDate.Month == targetMonth && t.TransDate.Year == targetYear)
                .SumAsync(t => t.Amount);

            var outcome = await db.Transactions
                .Where(t => !t.IsIncome && t.TransDate.Month == targetMonth && t.TransDate.Year == targetYear)
                .SumAsync(t => t.Amount);

            return new MonthlyCashflow { Income = income, Outcome = outcome };
        }
    }

    public class CategoryChart
    {
        public List<SingleData> ChartData { get; set; }

        public class SingleData 
        {
            public string Category { get; set; }
            public decimal Value { get; set; }
        }

        public static async Task<CategoryChart> LoadMonthlyExpenseCategoryChart(AppDbContext db, int? month = null, int? year = null)
        {
            decimal summer = 0m;
            var categoryChart = new CategoryChart()
            {
                ChartData = new List<SingleData>()
            };

            int targetMonth = month ?? DateTime.Now.Month;
            int targetYear = year ?? DateTime.Now.Year;

            var totalAmount = await db.Transactions
                .Where(t => !t.IsIncome && t.TransDate.Month == targetMonth && t.TransDate.Year == targetYear)
                .SumAsync(t => t.Amount);

            if (totalAmount == 0)
                return categoryChart;

            var allCategory = await db.Categories.ToListAsync();

            foreach(var category in allCategory)
            {
                var categoryAmount = await db.Transactions
                    .Where(t => !t.IsIncome && t.Item.CategoryId == category.Id && t.TransDate.Month == targetMonth && t.TransDate.Year == targetYear)
                    .SumAsync(t => t.Amount);

                if (categoryAmount == 0)
                    continue;

                var percentage = (categoryAmount / totalAmount) * 100;
                categoryChart.ChartData.Add(new SingleData
                {
                    Category = category.Name,
                    Value = Math.Round(percentage, 2)
                });
                summer += percentage;
            }

            if (summer < 100)
                categoryChart.ChartData.Add(new SingleData
                {
                    Category = "Unknown",
                    Value = Math.Round(100 - summer, 2)
                });

            return categoryChart;
        }
    }
}
