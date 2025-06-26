namespace FinanceApp.Shared
{
    public class SyncRequest
    {
        public string ItemName { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime InputDate { get; set; }
        public bool IsIncome { get; set; }
        public string LocalId { get; set; }
    }
}
