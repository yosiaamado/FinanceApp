using FinanceApp.Api.Database;
using FinanceApp.Api.Helper;
using FinanceApp.Api.IService;
using FinanceApp.Api.Model.DTO;
using FinanceApp.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinanceApp.Api.Service
{
    public class SecureService : ISecureService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public SecureService(IConfiguration config, AppDbContext context) 
        {
            _config = config;
            _context = context;
        }

        public async Task<bool> SignUp(User user)
        {
            var validate = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email || u.Username == user.Username);

            if (validate is not null)
                throw new Exception("Username or Email Already Existed");

            if (!user.Email.IsValidEmail())
                throw new Exception("Invalid Email Format");

            if (!user.Password.IsValidPassword())
                throw new Exception("Invalid Password Format");

            user.Password = EncryptionHelper.Hash(user.Password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ApiToken> SignIn(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

            if (user is null)
                throw new Exception("User Not Existed!");

            var isMatch = EncryptionHelper.Verify(request.Password, user.Password);
            if (!isMatch)
                throw new Exception("Invalid Password");

            var result = GenerateToken(user);
            return result;
        }

        public async Task<string> SendOtp(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
                throw new Exception("User Not Existed!");

            return "result";
        }

        private ApiToken GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(double.Parse(_config["Jwt:ExpiredDay"]!)),
                signingCredentials: cred
            );

            return new ApiToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiryDate = token.ValidTo
            };
        }
    }
}
