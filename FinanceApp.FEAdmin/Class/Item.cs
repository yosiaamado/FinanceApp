using System.ComponentModel.DataAnnotations;

namespace FinanceApp.FEAdmin.Class
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
