using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UniversalController : ControllerBase
    {
        
    }
}
