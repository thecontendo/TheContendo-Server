using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contendo.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "api.public")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "api.public")]
    public class PublicBaseController: ControllerBase {}
}