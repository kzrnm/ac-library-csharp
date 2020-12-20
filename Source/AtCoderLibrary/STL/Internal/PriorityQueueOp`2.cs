using System.Collections.Generic;
using System.Diagnostics;

namespace AtCoder.Internal
{
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class PriorityQueueOp<TKey, TValue, TKOp>
        : PriorityQueueOp<KeyValuePair<TKey, TValue>, KeyComparer<TKey, TValue, TKOp>>
        where TKOp : IComparer<TKey>
    {
        public PriorityQueueOp() : this(default(TKOp)) { }
        public PriorityQueueOp(int capacity) : this(capacity, default(TKOp)) { }
        public PriorityQueueOp(TKOp comparer) : base(new KeyComparer<TKey, TValue, TKOp>(comparer)) { }
        public PriorityQueueOp(int capacity, TKOp comparer) : base(capacity, new KeyComparer<TKey, TValue, TKOp>(comparer)) { }
        public void Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value));
    }
}
