using Microsoft.AspNetCore.Mvc;

namespace Fibon.api.Controllers
{
    [Route("[controller]")]
    public class FibonacciController : Controller
    {
        [HttpGet("{number}")]
        public IActionResult Get(int number) => this.Content("0");

        [HttpPost("{number}")]
        public IActionResult Post(int number)
        {
            return this.Accepted($"fibonnaci/{number}", null);
        }
    }
}