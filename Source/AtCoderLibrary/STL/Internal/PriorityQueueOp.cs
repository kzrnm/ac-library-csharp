using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AtCoder.Internal
{
    using static MethodImplOptions;
    [DebuggerTypeProxy(typeof(PriorityQueueOp<,>.DebugView))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class PriorityQueueOp<T, TOp> : IPriorityQueueOp<T>, IEnumerable
        where TOp : IComparer<T>
    {
        protected T[] data;
        protected readonly TOp _comparer;
        internal const int DefaultCapacity = 16;
        public PriorityQueueOp() : this(default(TOp)) { }
        public PriorityQueueOp(int capacity) : this(capacity, default(TOp)) { }
        public PriorityQueueOp(TOp comparer) : this(DefaultCapacity, comparer) { }
        public PriorityQueueOp(int capacity, TOp comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            data = new T[Math.Max(capacity, DefaultCapacity)];
            _comparer = comparer;
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Count { get; private set; } = 0;

        public T Peek => data[0];
        [MethodImpl(AggressiveInlining)]
        internal void Resize()
        {
            Array.Resize(ref data, data.Length << 1);
        }
        [MethodImpl(AggressiveInlining)]
        public void Add(T value)
        {
            if (Count >= data.Length) Resize();
            data[Count++] = value;
            UpdateUp(Count - 1);
        }
        [MethodImpl(AggressiveInlining)]
        public bool TryDequeue(out T result)
        {
            if (Count == 0)
            {
                result = default(T);
                return false;
            }
            result = Dequeue();
            return true;
        }
        [MethodImpl(AggressiveInlining)]
        public T Dequeue()
        {
            var res = data[0];
            data[0] = data[--Count];
            UpdateDown(0);
            return res;
        }
        [MethodImpl(AggressiveInlining)]
        protected internal void UpdateUp(int i)
        {
            var tar = data[i];
            while (i > 0)
            {
                var p = (i - 1) >> 1;
                if (_comparer.Compare(tar, data[p]) >= 0)
                    break;
                data[i] = data[p];
                i = p;
            }
            data[i] = tar;
        }
        [MethodImpl(AggressiveInlining)]
        protected internal void UpdateDown(int i)
        {
            var tar = data[i];
            var n = Count;
            var child = 2 * i + 1;
            while (child < n)
            {
                if (child != n - 1 && _comparer.Compare(data[child], data[child + 1]) > 0) child++;
                if (_comparer.Compare(tar, data[child]) <= 0)
                    break;
                data[i] = data[child];
                i = child;
                child = 2 * i + 1;
            }
            data[i] = tar;
        }
        public void Clear() => Count = 0;

        private T[] GetItems()
        {
            var arr = new ArraySegment<T>(data, 0, Count).ToArray();
            Array.Sort(arr, _comparer);
            return arr;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetItems().GetEnumerator();
        private class DebugView
        {
            private readonly PriorityQueueOp<T, TOp> pq;
            public DebugView(PriorityQueueOp<T, TOp> pq)
            {
                this.pq = pq;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public T[] Items => pq.GetItems();
        }
    }
}
