using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.User
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("api/user/[controller]")]
    public abstract class UserBaseClass : ControllerBase { }
}
