using System.Collections.Generic;
using System.Linq;

namespace Architecture.Util
{
    public class Paged<T>
    {
        public Paged(int count, IEnumerable<T> items)
        {
            Extension.EnsureArgumentIsInRange(count < 0, "count");
            var enumerable = items as IList<T> ?? items.ToList();
            Extension.EnsureIsNotNull(enumerable, "items");
            Count = count;
            Items = enumerable;
        }

        public int Count { get; private set; }

        public IEnumerable<T> Items { get; private set; }
    }
}
