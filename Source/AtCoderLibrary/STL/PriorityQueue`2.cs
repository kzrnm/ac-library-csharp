using System.Collections.Generic;
using System.Diagnostics;
using AtCoder.Internal;

namespace AtCoder
{
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class PriorityQueue<TKey, TValue> : PriorityQueueOp<TKey, TValue, IComparer<TKey>>
    {
        public PriorityQueue() : base(Comparer<TKey>.Default) { }
        public PriorityQueue(int capacity) : base(capacity, Comparer<TKey>.Default) { }
        public PriorityQueue(IComparer<TKey> comparer) : base(comparer) { }
        public PriorityQueue(int capacity, IComparer<TKey> comparer) : base(capacity, comparer) { }
    }
}
