using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;

namespace Store.API.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]        // ignore conntroller from docs of swagger
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            return NotFound(new ApiResponse(code));
        }
    }
}
