using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Api.Model.DTO
{
    public class TransactionRequest
    {
        public string Id { get; set; }
        public int Quantity { get; set; } = 1;
        [Required]
        public decimal Amount { get; set; }
        public DateTime TransDate { get; set; }
        [Required]
        public bool IsIncome { get; set; }
        public Item Item { get; set; }
    }
}
