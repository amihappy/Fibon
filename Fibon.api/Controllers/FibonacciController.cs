using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace Fibon.api.Controllers
{
    [Route("[controller]")]
    public class FibonacciController : Controller
    {
        private readonly IBusClient busClient;

        public FibonacciController(IBusClient busClient)
        {
            this.busClient = busClient;
        }

        [HttpGet("{number}")]
        public IActionResult Get(int number) => this.Content("0");

        [HttpPost("{number}")]

        public IActionResult Post(int number)
        {
            return this.Accepted($"fibonnaci/{number}", null);
        }
    }
}