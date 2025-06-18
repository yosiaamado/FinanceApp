using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Api.Model.Transaction
{
    public class Transaction
    {
        [Key]
        public Int64 TransId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime InputDate { get; set; }
        public bool IsIncome { get; set; }
        public DateTime SyncDate { get; set; }
        public string? LocalId { get; set; }
    }
}
