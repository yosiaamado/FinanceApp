using FinanceApp.Api.Model.Core;

namespace FinanceApp.Api.Model.DTO
{
    public class TransactionRequest
    {
        public string Id { get; set; }
        public int? Quantity { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransDate { get; set; }
        public bool IsIncome { get; set; }
        public Item Item { get; set; }
    }
}
