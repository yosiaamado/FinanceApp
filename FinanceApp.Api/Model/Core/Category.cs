using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Api.Model.Core
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
