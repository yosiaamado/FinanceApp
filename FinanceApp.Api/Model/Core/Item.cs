using FinanceApp.Api.Model;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Api.Model
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null;
        public ICollection<Transaction> Transactions { get; set; }
    }
}
