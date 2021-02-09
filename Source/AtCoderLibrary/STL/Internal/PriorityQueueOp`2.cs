using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDequeue(out TKey key, out TValue value)
        {
            if (Count == 0)
            {
                key = default(TKey);
                value = default(TValue);
                return false;
            }
            (key, value) = Dequeue();
            return true;
        }
    }
}
