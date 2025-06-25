using FinanceApp.Shared;

namespace FinanceApp.Api.IService
{
    public interface ISynchronizeService
    {
        Task<SyncResult> ProcessSingleSync(SyncRequest trx);
    }
}
