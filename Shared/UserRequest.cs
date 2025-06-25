﻿using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared
{
    public class UserRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
