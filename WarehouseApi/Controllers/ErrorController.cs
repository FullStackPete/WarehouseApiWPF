using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WarehouseApi.Controllers
{
    
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var stackTrace = context?.Error.StackTrace;
            var errorMessage = context?.Error.Message; 

            // use this somewhere not client-sided -> Console.Write(errorMessage)
            return Problem();
        }
    }
}
