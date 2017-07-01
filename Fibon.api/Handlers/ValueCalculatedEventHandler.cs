using System.Threading.Tasks;
using Fibon.api.Repository;
using Fibon.messages.Events;
using RawRabbit;

namespace Fibon.api.Handlers
{
    public class ValueCalculatedEventHandler : IEventHandler<ValueCalculatedEvent>
    {
        private readonly IBusClient busClient;
        private readonly IRepository repository;

        public ValueCalculatedEventHandler(IBusClient busClient, IRepository repository)
        {
            this.busClient = busClient;
            this.repository = repository;
        }

        public Task HandleAsync(ValueCalculatedEvent e)
        {
            this.repository.Add(e.Number, e.Value);
            return Task.CompletedTask;
        }
    }
}