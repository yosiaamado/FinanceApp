using FinanceApp.Api.Model;
using FinanceApp.Api.Model.DTO;

namespace FinanceApp.Api.IService
{
    public interface ISecureService
    {
        Task<bool> SignUp(User user);
        Task<ApiToken> SignIn(LoginRequest user);
        Task<string> SendOtp(string email);
    }
}
