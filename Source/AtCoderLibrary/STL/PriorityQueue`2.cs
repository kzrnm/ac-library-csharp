using System.Collections.Generic;

namespace AtCoder
{
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
