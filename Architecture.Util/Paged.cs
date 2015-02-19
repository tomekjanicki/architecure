using System;
using System.Collections.Generic;

namespace Architecture.Util
{
    public class Paged<T>
    {
        public Paged(int count, IEnumerable<T> items)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            if (items == null)
                throw new ArgumentNullException("items");
            Count = count;
            Items = items;
        }

        public int Count { get; private set; }

        public IEnumerable<T> Items { get; private set; }
    }
}
