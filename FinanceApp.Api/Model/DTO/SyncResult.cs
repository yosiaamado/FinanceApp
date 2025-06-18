namespace FinanceApp.Api.Model.DTO
{
    public class SyncResult
    {
        public string LocalId { get; set; }
        public SyncStatus SyncStatus { get; set; }
    }

    public enum SyncStatus
    {
        Pending,
        InQueue,
        Unsynchronized,
        Synchronized
    }
}
