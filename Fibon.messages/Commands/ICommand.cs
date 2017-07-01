using System.Threading.Tasks;

namespace Fibon.messages.Commands
{
    public interface ICommand
    {
        
    }

    public interface ICommandHandler<in T> where T : ICommand
    {
        Task HandleAsync(T command);
    }

    public class CalculateValue : ICommand
    {
        public int Number { get; set; }

        public CalculateValue(int number)
        {
            Number = number;
        }

        public CalculateValue()
        {
            
        }
    }
}