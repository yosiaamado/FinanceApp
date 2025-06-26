using FinanceApp.Api.Model;
using FinanceApp.Shared;

namespace FinanceApp.Api.IService
{
    public interface ICoreService
    {
        Task<PagedResult<Item>> GetItems(int page, int pageSize, string? search = null);
        Task<Item> GetItem(Int64 id);

        #region Admin
        Task<PagedResult<TempItem>> GetTempItems(int page, int pageSize, string? search = null);
        Task<TempItem> GetTempItem(Int64 id);
        Task AddItem(ItemRequest item);
        Task UpdateItem(ItemRequest item);
        Task FinalizeTempItem(int id, int categoryId);
        #endregion
    }
}
