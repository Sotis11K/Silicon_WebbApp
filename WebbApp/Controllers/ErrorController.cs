using Microsoft.AspNetCore.Mvc;

namespace WebbApp.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("/Error/{statusCode}")]
        public IActionResult Error404(int statusCode)
        {
            return View(statusCode);
        }
    }

}
