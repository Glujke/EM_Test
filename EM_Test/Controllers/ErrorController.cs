using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EM_Test.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (exception != null)
            {
                _logger.LogError(exception, "An unhandled exception occurred.");
            }
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An error occurred while processing your request.",
                detail: "Please contact support."
            );
        }
    }
}
