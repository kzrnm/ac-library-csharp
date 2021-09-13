using System;
using System.Collections.Generic;
using System.Diagnostics;
using AtCoder.Internal;

namespace AtCoder
{
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class PriorityQueueDictionary<TKey, TValue> : PriorityQueueOp<TKey, TValue, ComparableComparer<TKey>>
        where TKey : IComparable<TKey>
    {
        public PriorityQueueDictionary() : base(new ComparableComparer<TKey>()) { }
        public PriorityQueueDictionary(int capacity) : base(capacity, new ComparableComparer<TKey>()) { }
        public PriorityQueueDictionary(IComparer<TKey> comparer) : base(new ComparableComparer<TKey>(comparer)) { }
        public PriorityQueueDictionary(int capacity, IComparer<TKey> comparer) : base(capacity, new ComparableComparer<TKey>(comparer)) { }
    }
}
