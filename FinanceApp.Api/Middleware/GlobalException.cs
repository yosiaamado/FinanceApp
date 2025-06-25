using FinanceApp.Api.Model;

namespace FinanceApp.Api.Middleware
{
    public class GlobalException
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalException> _logger;

        public GlobalException(RequestDelegate next, ILogger<GlobalException> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(ApiResponse<string>.FailedMessage(ex.Message, "404"));
            }
            catch (ConflictException ex)
            {
                context.Response.StatusCode = 409;
                await context.Response.WriteAsJsonAsync(ApiResponse<string>.FailedMessage(ex.Message, "409"));
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(ApiResponse<string>.FailedMessage(ex.Message, "400"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(ApiResponse<string>.FailedMessage("Internal server error", "500"));
            }
        }
    }

}
