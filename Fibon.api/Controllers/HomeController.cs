using Microsoft.AspNetCore.Mvc;

namespace Fibon.api.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")] // bez tego przesloni posta
        public IActionResult Get()
            => this.Content("hello :)");
    }
}