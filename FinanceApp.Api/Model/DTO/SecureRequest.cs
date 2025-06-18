using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Api.Model.DTO
{
    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
