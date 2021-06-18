using System.Collections.Generic;
using System.Diagnostics;
using AtCoder.Internal;

namespace AtCoder.Stl
{
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class PriorityQueue<T> : PriorityQueueOp<T, IComparer<T>>
    {
        public PriorityQueue() : base(Comparer<T>.Default) { }
        public PriorityQueue(int capacity) : base(capacity, Comparer<T>.Default) { }
        public PriorityQueue(IComparer<T> comparer) : base(comparer) { }
        public PriorityQueue(int capacity, IComparer<T> comparer) : base(capacity, comparer) { }
    }
}
