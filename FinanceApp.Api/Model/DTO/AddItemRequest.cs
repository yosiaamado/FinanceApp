using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Api.Model.DTO
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
