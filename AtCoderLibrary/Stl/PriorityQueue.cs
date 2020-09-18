using System;
using System.Collections.Generic;

namespace AtCoder.Stl
{
    [System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class PriorityQueue<T>
    {
        protected readonly List<T> data;
        protected readonly IComparer<T> comparer;

        public PriorityQueue() : this(Comparer<T>.Default) { }
        public PriorityQueue(int capacity) : this(capacity, Comparer<T>.Default) { }
        public PriorityQueue(IComparer<T> comparer)
        {
            this.data = new List<T>();
            this.comparer = comparer;
        }
        public PriorityQueue(int capacity, IComparer<T> comparer)
        {
            this.data = new List<T>(capacity);
            this.comparer = comparer;
        }
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Count => data.Count;

        public T Peek => data[0];

        public void Add(T value)
        {
            data.Add(value);
            UpdateUp(data.Count - 1);
        }
        public T Dequeue()
        {
            var res = data[0];
            data[0] = data[^1];
            data.RemoveAt(data.Count - 1);
            UpdateDown(0);
            return res;
        }
        void UpdateUp(int i)
        {
            if (i > 0)
            {
                var p = (i - 1) >> 1;
                if (comparer.Compare(data[i], data[p]) < 0)
                {
                    (data[p], data[i]) = (data[i], data[p]);
                    UpdateUp(p);
                }
            }
        }
        void UpdateDown(int i)
        {
            var n = data.Count;
            var child = 2 * i + 1;
            if (child < n)
            {
                if (child != n - 1 && comparer.Compare(data[child], data[child + 1]) > 0) child++;
                if (comparer.Compare(data[i], data[child]) > 0)
                {
                    (data[child], data[i]) = (data[i], data[child]);
                    UpdateDown(child);
                }
            }
        }
        public void Clear() => data.Clear();
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
        T[] Items
        {
            get
            {
                var arr = data.ToArray();
                Array.Sort(arr, comparer);
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
