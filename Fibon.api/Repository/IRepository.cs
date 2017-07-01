using System.Collections.Generic;

namespace Fibon.api.Repository
{
    public interface IRepository
    {
        void Add(int number, int result);

        int? Get(int number);
    }

    public class MemoryRepo : IRepository
    {
        private readonly Dictionary<int,int> storage = new Dictionary<int, int>();
        public void Add(int number, int result)
        {
            this.storage.Add(number, result);
        }

        public int? Get(int number)
        {
            int value;
            if (this.storage.TryGetValue(number, out value))
            {
                return value;
            }

            return null;
        }
    }
}