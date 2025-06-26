using System.ComponentModel.DataAnnotations;

namespace FinanceApp.FEAdmin.Class
{
    public class Transaction
    {
        public long Id { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Amount { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime InputDate { get; set; }
        public bool IsIncome { get; set; }
        public DateTime? SyncDate { get; set; }
        public string? LocalId { get; set; }
    }
}
