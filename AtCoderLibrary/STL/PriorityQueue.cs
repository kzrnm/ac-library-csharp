using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class PriorityQueue<T>
    {
        protected T[] data;
        protected readonly IComparer<T> _comparer;
        internal const int DefaultCapacity = 16;
        public PriorityQueue() : this(Comparer<T>.Default) { }
        public PriorityQueue(int capacity) : this(capacity, Comparer<T>.Default) { }
        public PriorityQueue(IComparer<T> comparer) : this(DefaultCapacity, comparer) { }
        public PriorityQueue(int capacity, IComparer<T> comparer)
        {
            data = new T[Math.Max(capacity, DefaultCapacity)];
            _comparer = comparer;
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Count { get; private set; } = 0;

        public T Peek => data[0];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Resize()
        {
            Array.Resize(ref data, data.Length << 1);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T value)
        {
            if (Count >= data.Length) Resize();
            data[Count++] = value;
            UpdateUp(Count - 1);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Dequeue()
        {
            var res = data[0];
            data[0] = data[--Count];
            UpdateDown(0);
            return res;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void UpdateUp(int i)
        {
            if (i > 0)
            {
                var p = (i - 1) >> 1;
                if (_comparer.Compare(data[i], data[p]) < 0)
                {
                    (data[p], data[i]) = (data[i], data[p]);
                    UpdateUp(p);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void UpdateDown(int i)
        {
            var n = Count;
            var child = 2 * i + 1;
            if (child < n)
            {
                if (child != n - 1 && _comparer.Compare(data[child], data[child + 1]) > 0) child++;
                if (_comparer.Compare(data[i], data[child]) > 0)
                {
                    (data[child], data[i]) = (data[i], data[child]);
                    UpdateDown(child);
                }
            }
        }
        public void Clear() => Count = 0;
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051", Justification = "for debugger")]
        private T[] Items
        {
            get
            {
                var arr = new ArraySegment<T>(data, 0, Count).ToArray();
                Array.Sort(arr, _comparer);
                return arr;
            }
        }
    }


    public class PriorityQueue<TKey, TValue> : PriorityQueue<KeyValuePair<TKey, TValue>>
    {
        class KeyComparer : IComparer<KeyValuePair<TKey, TValue>>
        {
            public readonly IComparer<TKey> comparer;
            public KeyComparer(IComparer<TKey> comparer)
            {
                this.comparer = comparer;
            }
            public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
                => comparer.Compare(x.Key, y.Key);
        }

        public PriorityQueue() : this(Comparer<TKey>.Default) { }
        public PriorityQueue(int capacity) : this(capacity, Comparer<TKey>.Default) { }
        public PriorityQueue(IComparer<TKey> comparer) : base(new KeyComparer(comparer)) { }
        public PriorityQueue(int capacity, IComparer<TKey> comparer) : base(capacity, new KeyComparer(comparer)) { }

        public void Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value));
    }
}
