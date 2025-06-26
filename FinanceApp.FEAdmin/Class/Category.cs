using System.ComponentModel.DataAnnotations;

namespace FinanceApp.FEAdmin.Class
{
    public class Category
    {
        public int Id { get; set; }
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
