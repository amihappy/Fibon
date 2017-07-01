using System.Threading.Tasks;

namespace Fibon.messages.Events
{
    public interface IEvent
    {
        
    }

    public interface IEventHandler<in T> where T : IEvent
    {
        Task HandleAsync(T e);
    }

    public class ValueCalculatedEvent : IEvent
    {
        public int Number { get; set; }

        public int Value { get; set; }

        public ValueCalculatedEvent(int number, int value)
        {
            Number = number;
            Value = value;
        }
    }
}