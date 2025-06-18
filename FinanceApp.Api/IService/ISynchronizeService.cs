using FinanceApp.Api.Model.DTO;

namespace FinanceApp.Api.IService
{
    public interface ISynchronizeService
    {
        Task<SyncResult> ProcessSingleSync(SyncRequest trx);
    }
}
