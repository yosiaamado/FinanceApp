using AIClassifierLib.Interface;
using AIClassifierLib.Models;
using FinanceApp.Api.Database;
using FinanceApp.Api.Model;
using FinanceApp.Shared;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Api.Helper
{
    public class EngineHelper
    {
        private IItemClassifierEngine _itemEngine;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly string modelPath;
        public EngineHelper(IItemClassifierEngine itemEngine, AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _itemEngine = itemEngine;
            _config = configuration;

            var projectRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName!;
            modelPath = Path.Combine(projectRoot, _config["AppSettings:ModelPath"]!);
        }
        private async Task<List<ItemData>> InitAsync()
        {
            var items = await _context.Items
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

            var datas = items.Select(x => new ItemData
            {
                Name = x.Name,
                Category = x.Category.Name
            }).ToList();

            await _itemEngine.InitAsync(datas, modelPath);
            return datas;
        }

        public async Task FillCategoryInTempItem(TransactionRequest request)
        {
            var datas = new List<ItemData>();
            if(!_itemEngine.IsInitialized)
                datas = await InitAsync();

            var name = _itemEngine.AutoCorrect(request.Item.Name, datas.Select(n => n.Name));
            var predictedCategory = _itemEngine.Predict(name);

            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == predictedCategory);

            if(category is null)
            {
                await _context.AddAsync(new Category
                {
                    Name = predictedCategory
                });
                await _context.SaveChangesAsync();
            }

            var tempItem = new TempItem
            {
                Name = request.Item.Name,
                IsReviewed = false,
                CreatedAt = DateTime.Now,
                MovedToItemTable = false,
                CategoryId = category!.Id
            };

            await _context.TempItems.AddAsync(tempItem);
            await _context.SaveChangesAsync();
        }
    }
}
