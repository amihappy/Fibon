using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fibon.messages.Commands;
using Fibon.messages.Events;
using RawRabbit;

namespace Fibon.service.Handlers
{
    public class CalculatedValueCommandHandler : ICommandHandler<CalculateValue>
    {
        private readonly IBusClient client;

        private static int Fib(int n)
        {
            switch (n)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                default:
                    return Fib(n - 2) + Fib(n - 1);
            }
        }

        public CalculatedValueCommandHandler(IBusClient client)
        {
            this.client = client;
        }

        public async Task HandleAsync(CalculateValue command)
        {
            int result = Fib(command.Number);
            await this.client.PublishAsync(new ValueCalculatedEvent(command.Number, result));
        }
    }
}
