using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared
{
    public class ItemRequest
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
