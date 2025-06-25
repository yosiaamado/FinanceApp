using AutoMapper;
using FinanceApp.Api.Database;
using FinanceApp.Api.IService;
using FinanceApp.Api.Model;
using FinanceApp.Shared;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace FinanceApp.Api.Service
{
    public class CoreService : ICoreService
    {
        public readonly AppDbContext _context;
        public readonly IMapper _mapper;

        public CoreService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<Item>> GetItems(int page, int pageSize, string? search = null)
        {
            IQueryable<Item> query = _context.Items;

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(i => i.Name.Contains(search));

            var items = await query
                .OrderByDescending(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();

            return new PagedResult<Item>
            {
                Items = items,
                TotalItems = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
        public async Task<Item> GetItem(Int64 id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);

            return item ?? new Item();
        }
        public async Task<PagedResult<TempItem>> GetTempItems(int page, int pageSize, string? search = null)
        {
            IQueryable<TempItem> query = _context.TempItems;

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(i => i.Name.Contains(search));

            var items = await query
                .OrderByDescending(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();

            return new PagedResult<TempItem>
            {
                Items = items,
                TotalItems = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<TempItem> GetTempItem(Int64 id)
        {
            var item = await _context.TempItems.FirstOrDefaultAsync(i => i.Id == id);

            return item ?? new TempItem();
        }

        public async Task AddItem(ItemRequest item)
        {
            var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.Name == item.Name);

            if (existingItem is not null)
                throw new Exception("Duplicate Value");

            var newItem = _mapper.Map<Item>(item);
            await _context.AddAsync(newItem);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateItem(ItemRequest item)
        {
            var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == item.Id);

            if (existingItem is null)
                throw new Exception("Item doesn't exists");

            existingItem.Name = item.Name;
            existingItem.CategoryId = item.CategoryId;
            await _context.SaveChangesAsync();
        }
        public async Task FinalizeTempItem(int id, int categoryId)
        {
            var tempItem = await _context.TempItems.FirstOrDefaultAsync(i => i.Id == id);

            if (tempItem is null)
                throw new Exception("Temp item doesn't exists");

            var item = await _context.Items.FirstOrDefaultAsync(i => i.Name == tempItem.Name);
            tempItem.IsReviewed = true;
            
            if(item is not null)
            {
                tempItem.MovedToItemTable = false;
                item = new Item()
                {
                    Name = tempItem.Name,
                    CategoryId = categoryId
                };

                await _context.Items.AddAsync(item);
            }
            
            _context.TempItems.Update(tempItem);
            await _context.SaveChangesAsync();
        }
    }
}
