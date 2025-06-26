namespace FinanceApp.Shared
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
