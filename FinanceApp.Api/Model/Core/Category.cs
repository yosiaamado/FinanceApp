﻿using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Api.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<TempItem> TempItems { get; set; } = new List<TempItem>();
    }
}
