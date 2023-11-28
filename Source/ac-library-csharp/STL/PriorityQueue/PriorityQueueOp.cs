using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AtCoder.Internal
{
    using static EditorBrowsableState;
    [DebuggerTypeProxy(typeof(PriorityQueueOp<,>.DebugView))]
    [DebuggerDisplay(nameof(Count) + " = {" + nameof(Count) + "}")]
    public class PriorityQueueOp<T, TOp> : IPriorityQueueOp<T>
        where TOp : IComparer<T>
    {
        [EditorBrowsable(Never)]
        public T[] data;
        [EditorBrowsable(Never)]
        public readonly TOp _comparer;
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
        [DebuggerBrowsable(0)]
        public int Count { get; private set; } = 0;

        public T Peek => data[0];
        [MethodImpl(256)]
        internal void Resize()
        {
            Array.Resize(ref data, data.Length << 1);
        }
        [MethodImpl(256)]
        public void Enqueue(T value)
        {
            if (Count >= data.Length) Resize();
            data[Count++] = value;
            UpdateUp(Count - 1);
        }
        [MethodImpl(256)]
        public bool TryPeek(out T result)
        {
            if (Count == 0)
            {
                result = default;
                return false;
            }
            result = data[0];
            return true;
        }
        [MethodImpl(256)]
        public bool TryDequeue(out T result)
        {
            if (Count == 0)
            {
                result = default;
                return false;
            }
            result = Dequeue();
            return true;
        }
        [MethodImpl(256)]
        public T Dequeue()
        {
            var res = data[0];
            data[0] = data[--Count];
            UpdateDown(0);
            return res;
        }
        /// <summary>
        /// <paramref name="value"/> を Enqueue(T) してから Dequeue します。
        /// </summary>
        [MethodImpl(256)]
        public T EnqueueDequeue(T value)
        {
            var res = data[0];
            if (Count == 0 || _comparer.Compare(value, res) <= 0)
            {
                return value;
            }
            data[0] = value;
            UpdateDown(0);
            return res;
        }
        /// <summary>
        /// Dequeue してから <paramref name="value"/> を Enqueue(T) します。
        /// </summary>
        [MethodImpl(256)]
        public T DequeueEnqueue(T value)
        {
            var res = data[0];
            data[0] = value;
            UpdateDown(0);
            return res;
        }
        /// <summary>
        /// Dequeue した値に <paramref name="func"/> を適用して Enqueue(T) します。
        /// </summary>
        [MethodImpl(256)]
        public void DequeueEnqueue(Func<T, T> func)
        {
            data[0] = func(data[0]);
            UpdateDown(0);
        }
        [MethodImpl(256)]
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
        [MethodImpl(256)]
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
        [MethodImpl(256)] public void Clear() => Count = 0;


        [EditorBrowsable(Never)]
        public ReadOnlySpan<T> Unorderd() => data.AsSpan(0, Count);

#if EMBEDDING
        [SourceExpander.NotEmbeddingSource]
#endif
        private class DebugView
        {
            private readonly PriorityQueueOp<T, TOp> pq;
            public DebugView(PriorityQueueOp<T, TOp> pq)
            {
                this.pq = pq;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public T[] Items
            {
                get
                {
                    var arr = pq.Unorderd().ToArray();
                    Array.Sort(arr, pq._comparer);
                    return arr;
                }
            }
        }
    }
}
