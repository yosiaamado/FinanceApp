using AutoMapper;
using FinanceApp.Api.Database;
using FinanceApp.Api.Helper;
using FinanceApp.Api.IService;
using FinanceApp.Api.Model;
using FinanceApp.Shared;
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
        private readonly IMapper _mapper;
        public SecureService(IConfiguration config, AppDbContext context, IMapper mapper) 
        {
            _config = config;
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> SignUp(UserRequest request)
        {
            var user = _mapper.Map<User>(request);
            var validate = await _context.Users.FirstOrDefaultAsync(u => (u.Email == user.Email || u.Username == user.Username) && u.Role == "User");

            if (validate is not null)
                throw new ConflictException("Username or Email Already Existed");

            if (!user.Email.IsValidEmail())
                throw new ValidationException("Invalid Email Format");

            if (!user.Password.IsValidPassword())
                throw new ValidationException("Invalid Password Format");

            user.Password = EncryptionHelper.Hash(user.Password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateUserAdmin(UserRequest request)
        {
            var user = _mapper.Map<User>(request);
            var validate = await _context.Users.FirstOrDefaultAsync(u => (u.Email == user.Email || u.Username == user.Username) && u.Role == "Admin");

            if (validate is not null)
                throw new ConflictException("Username or Email Already Existed");

            if (!user.Email.IsValidEmail())
                throw new ValidationException("Invalid Email Format");

            user.Password = EncryptionHelper.Hash(_config["AppSettings:DefaultAdminPassword"]!);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ApiToken> SignIn(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

            if (user is null)
                throw new NotFoundException("User Not Existed!");

            var isMatch = EncryptionHelper.Verify(request.Password, user.Password);
            if (!isMatch)
                throw new ValidationException("Invalid Password");

            var result = GenerateToken(user);
            return result;
        }

        public async Task<string> SendOtp(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
                throw new NotFoundException("User Not Existed!");

            return "result";
        }

        private ApiToken GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            if (user.Role == "SuperAdmin")
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));

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
