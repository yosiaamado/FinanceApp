using FinanceApp.Api.Model;
using FinanceApp.Shared;

namespace FinanceApp.Api.IService
{
    public interface ICoreService
    {
        Task<PagedResult<Item>> GetItems(int page, int pageSize, string? search = null);
        Task<Item> GetItems(Int64 id);

        #region Admin
        Task<bool> AddItem(ItemRequest item);
        Task<bool> UpdateItem(ItemRequest item);
        Task<bool> FinalizeTempItem(int id, int categoryId);
        #endregion
    }
}
