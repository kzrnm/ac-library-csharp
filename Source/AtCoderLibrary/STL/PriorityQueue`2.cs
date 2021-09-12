using System.Collections.Generic;
using System.Diagnostics;
using AtCoder.Internal;

namespace AtCoder
{
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class PriorityQueueDictionary<TKey, TValue> : PriorityQueueOp<TKey, TValue, IComparer<TKey>>
    {
        public PriorityQueueDictionary() : base(Comparer<TKey>.Default) { }
        public PriorityQueueDictionary(int capacity) : base(capacity, Comparer<TKey>.Default) { }
        public PriorityQueueDictionary(IComparer<TKey> comparer) : base(comparer) { }
        public PriorityQueueDictionary(int capacity, IComparer<TKey> comparer) : base(capacity, comparer) { }
    }
}
