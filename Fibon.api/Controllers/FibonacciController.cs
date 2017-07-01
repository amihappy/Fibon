using System.Threading.Tasks;
using Fibon.api.Repository;
using Fibon.messages.Commands;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace Fibon.api.Controllers
{
    [Route("[controller]")]
    public class FibonacciController : Controller
    {
        private readonly IBusClient busClient;
        private readonly IRepository repository;

        public FibonacciController(IBusClient busClient, IRepository repository)
        {
            this.busClient = busClient;
            this.repository = repository;
        }

        [HttpGet("{number}")]
        public IActionResult Get(int number)
        {
            int? calcResult = this.repository.Get(number);
            if (calcResult.HasValue)
            {
                return this.Content(calcResult.Value.ToString());
            }

            return NotFound();
        }

        [HttpPost("{number}")]

        public async Task<IActionResult> Post(int number)
        {
            int? calcResult = this.repository.Get(number);
            if (!calcResult.HasValue)
            {
                await this.busClient.PublishAsync(new CalculateValue(number));
            }
            return this.Accepted($"fibonnaci/{number}", null);
        }
    }
}