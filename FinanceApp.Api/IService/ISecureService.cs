using FinanceApp.Shared;

namespace FinanceApp.Api.IService
{
    public interface ISecureService
    {
        Task<bool> SignUp(UserRequest user);
        Task<bool> CreateUserAdmin(UserRequest request);
        Task<ApiToken> SignIn(LoginRequest user);
        Task<string> SendOtp(string email);
    }
}
