using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contendo.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "api.admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "api.admin")]
    public class AdminBaseController: ControllerBase {}
}