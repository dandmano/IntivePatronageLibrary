using Microsoft.AspNetCore.Mvc;

namespace IntivePatronageLibraryAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error() => Problem(statusCode: StatusCodes.Status500InternalServerError);
    }
}