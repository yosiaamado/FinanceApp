﻿using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Api.Model
{
    public class TempItem
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsReviewed { get; set; } = false;
        public bool MovedToItemTable = false;
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
