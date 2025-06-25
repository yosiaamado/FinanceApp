using FinanceApp.Api.IService;
using FinanceApp.Api.Model;
using FinanceApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private ISecureService _secureService;
        public AuthController(ISecureService secureService)
        {
            _secureService = secureService;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([Required] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(ApiResponse<string>.FailedMessage("Either username or email is required."));
            }

            try
            {
                var token = await _secureService.SignIn(request);
                if (string.IsNullOrEmpty(token.Token))
                    return Unauthorized(ApiResponse<string>.FailedMessage("Invalid credentials."));

                return Ok(ApiResponse<ApiToken>.Success(token));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailedMessage("Server error: " + ex.Message));
            }
        }
        
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([Required] UserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(ApiResponse<string>.FailedMessage("Username and Email is required."));
            }

            try
            {
                var result = await _secureService.SignUp(request);
                return Ok(ApiResponse<bool>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailedMessage("Server error: " + ex.Message));
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin([Required] UserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(ApiResponse<string>.FailedMessage("Username and Email is required."));
            }

            try
            {
                var result = await _secureService.CreateUserAdmin(request);
                return Ok(ApiResponse<bool>.CreatedSuccess());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailedMessage("Server error: " + ex.Message));
            }
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromQuery] string email)
        {
            try
            {
                var result = await _secureService.SendOtp(email);
                return Ok(ApiResponse<string>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailedMessage("Server error: " + ex.Message));
            }
        }
    }
}
