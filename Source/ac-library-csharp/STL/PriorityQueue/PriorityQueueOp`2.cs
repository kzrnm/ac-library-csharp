using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AtCoder.Internal
{
    using static EditorBrowsableState;
    [DebuggerTypeProxy(typeof(PriorityQueueOp<,,>.DebugView))]
    [DebuggerDisplay(nameof(Count) + " = {" + nameof(Count) + "}")]
    public class PriorityQueueOp<TKey, TValue, TKOp> : IPriorityQueueOp<KeyValuePair<TKey, TValue>>
        where TKOp : IComparer<TKey>
    {
        protected TKey[] keys;
        protected TValue[] values;
        protected readonly TKOp _comparer;
        internal const int DefaultCapacity = 16;
        public PriorityQueueOp() : this(default(TKOp)) { }
        public PriorityQueueOp(int capacity) : this(capacity, default(TKOp)) { }
        public PriorityQueueOp(TKOp comparer) : this(DefaultCapacity, comparer) { }
        public PriorityQueueOp(int capacity, TKOp comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            int size = Math.Max(capacity, DefaultCapacity);
            keys = new TKey[size];
            values = new TValue[size];
            _comparer = comparer;
        }
        [DebuggerBrowsable(0)]
        public int Count { get; private set; } = 0;

        public KeyValuePair<TKey, TValue> Peek => KeyValuePair.Create(keys[0], values[0]);
        [MethodImpl(256)]
        internal void Resize()
        {
            Array.Resize(ref keys, keys.Length << 1);
            Array.Resize(ref values, values.Length << 1);
        }
        [MethodImpl(256)]
        [EditorBrowsable(Never)]
        public void Enqueue(KeyValuePair<TKey, TValue> pair) => Enqueue(pair.Key, pair.Value);
        [MethodImpl(256)]
        public void Enqueue(TKey key, TValue value)
        {
            if (Count >= keys.Length) Resize();
            keys[Count] = key;
            values[Count++] = value;
            UpdateUp(Count - 1);
        }
        [MethodImpl(256)]
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
        [MethodImpl(256)]
        public bool TryDequeue(out KeyValuePair<TKey, TValue> result)
        {
            if (Count == 0)
            {
                result = default(KeyValuePair<TKey, TValue>);
                return false;
            }
            result = Dequeue();
            return true;
        }
        [MethodImpl(256)]
        public KeyValuePair<TKey, TValue> Dequeue()
        {
            var res = KeyValuePair.Create(keys[0], values[0]);
            keys[0] = keys[--Count];
            values[0] = values[Count];
            UpdateDown(0);
            return res;
        }
        /// <summary>
        /// <paramref name="key"/>, <paramref name="value"/> を Enqueue(T) してから Dequeue します。
        /// </summary>
        [MethodImpl(256)]
        public KeyValuePair<TKey, TValue> EnqueueDequeue(TKey key, TValue value)
        {
            var res = KeyValuePair.Create(keys[0], values[0]);
            if (_comparer.Compare(key, keys[0]) <= 0)
            {
                return KeyValuePair.Create(key, value);
            }
            keys[0] = key;
            values[0] = value;
            UpdateDown(0);
            return res;
        }
        /// <summary>
        /// Dequeue した値に <paramref name="func"/> を適用して Enqueue(T) します。
        /// </summary>
        [MethodImpl(256)]
        public void DequeueEnqueue(Func<TKey, TValue, (TKey key, TValue value)> func)
        {
            (keys[0], values[0]) = func(keys[0], values[0]);
            UpdateDown(0);
        }
        [MethodImpl(256)]
        protected internal void UpdateUp(int i)
        {
            var tar = keys[i];
            var tarVal = values[i];
            while (i > 0)
            {
                var p = (i - 1) >> 1;
                if (_comparer.Compare(tar, keys[p]) >= 0)
                    break;
                keys[i] = keys[p];
                values[i] = values[p];
                i = p;
            }
            keys[i] = tar;
            values[i] = tarVal;
        }
        [MethodImpl(256)]
        protected internal void UpdateDown(int i)
        {
            var tar = keys[i];
            var tarVal = values[i];
            var n = Count;
            var child = 2 * i + 1;
            while (child < n)
            {
                if (child != n - 1 && _comparer.Compare(keys[child], keys[child + 1]) > 0) child++;
                if (_comparer.Compare(tar, keys[child]) <= 0)
                    break;
                keys[i] = keys[child];
                values[i] = values[child];
                i = child;
                child = 2 * i + 1;
            }
            keys[i] = tar;
            values[i] = tarVal;
        }
        [MethodImpl(256)] public void Clear() => Count = 0;

        [EditorBrowsable(Never)]
        public ReadOnlySpan<TKey> UnorderdKeys() => keys.AsSpan(0, Count);
        [EditorBrowsable(Never)]
        public ReadOnlySpan<TValue> UnorderdValues() => values.AsSpan(0, Count);
        private class DebugView
        {
            private readonly PriorityQueueOp<TKey, TValue, TKOp> pq;
            public DebugView(PriorityQueueOp<TKey, TValue, TKOp> pq)
            {
                this.pq = pq;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public KeyValuePair<TKey, TValue>[] Items
            {
                get
                {
                    var count = pq.Count;
                    var keys = pq.keys.AsSpan(0, count).ToArray();
                    var values = pq.values.AsSpan(0, count).ToArray();
                    Array.Sort(keys, values, pq._comparer);
                    var arr = new KeyValuePair<TKey, TValue>[count];
                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = KeyValuePair.Create(keys[i], values[i]);
                    return arr;
                }
            }
        }
    }
}
