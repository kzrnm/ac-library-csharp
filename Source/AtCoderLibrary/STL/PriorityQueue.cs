using System;
using System.Collections.Generic;
using System.Diagnostics;
using AtCoder.Internal;

namespace AtCoder
{
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class PriorityQueue<T> : PriorityQueueOp<T, ComparableComparer<T>>
        where T : IComparable<T>
    {
        public PriorityQueue() : base(new ComparableComparer<T>()) { }
        public PriorityQueue(int capacity) : base(capacity, new ComparableComparer<T>()) { }
        public PriorityQueue(IComparer<T> comparer) : base(new ComparableComparer<T>(comparer)) { }
        public PriorityQueue(int capacity, IComparer<T> comparer) : base(capacity, new ComparableComparer<T>(comparer)) { }
    }
}
