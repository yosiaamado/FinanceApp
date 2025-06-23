using AutoMapper;
using FinanceApp.Api.Database;
using FinanceApp.Api.Model;
using FinanceApp.Api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Api.Service
{
    public class CoreService
    {
        public readonly AppDbContext _context;
        public readonly IMapper _mapper;

        public CoreService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddItem(ItemRequest item)
        {
            var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.Name == item.Name);

            if (existingItem is not null)
                return false;

            var newItem = _mapper.Map<Item>(item);
            await _context.AddAsync(newItem);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> UpdateItem(ItemRequest item)
        {
            var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == item.Id);

            if (existingItem is null)
                return false;

            existingItem.Name = item.Name;
            existingItem.CategoryId = item.CategoryId;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
