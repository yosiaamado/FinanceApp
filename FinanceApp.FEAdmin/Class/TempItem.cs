using System.ComponentModel.DataAnnotations;

namespace FinanceApp.FEAdmin.Class
{
    public class TempItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsReviewed { get; set; } = false;
        public bool MovedToItemTable = false;
    }
}
