using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AtCoder.Internal
{
    [DebuggerTypeProxy(typeof(PriorityQueueOp<,,>.DebugView))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class PriorityQueueOp<TKey, TValue, TKOp> :
        IPriorityQueueOp<KeyValuePair<TKey, TValue>>, IEnumerable
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
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Count { get; private set; } = 0;

        public KeyValuePair<TKey, TValue> Peek => KeyValuePair.Create(keys[0], values[0]);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Resize()
        {
            Array.Resize(ref keys, keys.Length << 1);
            Array.Resize(ref values, values.Length << 1);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(KeyValuePair<TKey, TValue> pair) => Add(pair.Key, pair.Value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(TKey key, TValue value)
        {
            if (Count >= keys.Length) Resize();
            keys[Count] = key;
            values[Count++] = value;
            UpdateUp(Count - 1);
        }
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public KeyValuePair<TKey, TValue> Dequeue()
        {
            var res = KeyValuePair.Create(keys[0], values[0]);
            keys[0] = keys[--Count];
            values[0] = values[Count];
            UpdateDown(0);
            return res;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        public void Clear() => Count = 0;

        private KeyValuePair<TKey, TValue>[] GetItems()
        {
            var keys = new ArraySegment<TKey>(this.keys, 0, Count).ToArray();
            var values = new ArraySegment<TValue>(this.values, 0, Count).ToArray();
            Array.Sort(keys, values, _comparer);
            var arr = new KeyValuePair<TKey, TValue>[Count];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = KeyValuePair.Create(keys[i], values[i]);
            return arr;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetItems().GetEnumerator();
        private class DebugView
        {
            private readonly PriorityQueueOp<TKey, TValue, TKOp> pq;
            public DebugView(PriorityQueueOp<TKey, TValue, TKOp> pq)
            {
                this.pq = pq;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public KeyValuePair<TKey, TValue>[] Items => pq.GetItems();
        }
    }
}
