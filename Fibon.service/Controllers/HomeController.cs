using Microsoft.AspNetCore.Mvc;

namespace Fibon.service.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Get()
            => this.Content("hello :)");
    }
}