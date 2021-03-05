using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    using static MethodImplOptions;

    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class Deque<T> : IEnumerable<T>, IReadOnlyCollection<T>, ICollection<T>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        public T[] data;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public int mask;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public int head;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public int tail;

        public Deque() : this(8) { }
        public Deque(int capacity)
        {
            if (capacity <= 8) capacity = 8;
            else capacity = 1 << (InternalBit.CeilPow2(capacity));
            data = new T[capacity];
            mask = capacity - 1;
        }

        public int Count => (tail - head) & mask;

        public ref T this[int i] => ref data[(head + i) & mask];
        public T First => data[head];
        public T Last => data[(tail - 1) & mask];
        private static void ThrowDequeIsEmpty() => throw new InvalidOperationException("deque is empty");

        [MethodImpl(AggressiveInlining)]
        public T PopFirst()
        {
            if (head == tail) ThrowDequeIsEmpty();
            var item = data[head];
            head = (head + 1) & mask;
            return item;
        }
        [MethodImpl(AggressiveInlining)]
        public T PopLast()
        {
            if (head == tail) ThrowDequeIsEmpty();
            return data[tail = (tail - 1) & mask];
        }
        [MethodImpl(AggressiveInlining)]
        public void AddFirst(T item)
        {
            data[head = (head - 1) & mask] = item;
            if (head == tail) Resize();
        }
        [MethodImpl(AggressiveInlining)]
        public void AddLast(T item)
        {
            data[tail] = item;
            tail = (tail + 1) & mask;
            if (head == tail) Resize();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Resize()
        {
            var oldSize = data.Length;
            var newArray = new T[oldSize << 1];

            var hsize = oldSize - head;
            Array.Copy(data, head, newArray, 0, hsize);
            Array.Copy(data, 0, newArray, hsize, tail);
            data = newArray;
            mask = data.Length - 1;
            head = 0;
            tail = oldSize;
        }
        public void Clear() => head = tail = 0;

        public void Add(T item) => AddLast(item);
        bool ICollection<T>.Contains(T item)
        {
            if (head == tail) return false;

            if (head < tail)
            {
                var ix = Array.IndexOf(data, item, head);
                return ix >= 0 && ix < tail;
            }
            else
            {
                var ix = Array.IndexOf(data, item, head);
                if (ix >= 0) return true;
                ix = Array.IndexOf(data, item);
                return (uint)ix < tail;
            }

        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            if (head <= tail)
            {
                Array.Copy(data, head, array, arrayIndex, tail - head);
            }
            else
            {
                var hsize = data.Length - head;
                Array.Copy(data, head, array, arrayIndex, hsize);
                Array.Copy(data, 0, array, arrayIndex + hsize, tail);
            }
        }
        public Enumerator Reversed() => new Enumerator(this, true);
        public Enumerator GetEnumerator() => new Enumerator(this, false);

        bool ICollection<T>.IsReadOnly => false;
        bool ICollection<T>.Remove(T item) { throw new NotSupportedException(); }
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public struct Enumerator : IEnumerator<T>, IEnumerable<T>
        {
            readonly Deque<T> deque;
            readonly bool isReverse;
            int index;
            public readonly int last;
            public T Current => deque.data[index];
            public Enumerator(Deque<T> deque, bool isReverse)
            {
                this.deque = deque;
                this.isReverse = isReverse;
                if (isReverse)
                {
                    index = deque.tail;
                    last = deque.head;
                }
                else
                {
                    index = deque.head - 1;
                    last = (deque.tail - 1) & deque.mask;
                }
            }
            object IEnumerator.Current => Current;
            public bool MoveNext()
            {
                if (index == last) return false;
                if (isReverse) --index; else ++index;
                index &= deque.mask;
                return true;
            }
            public void Reset() { throw new NotSupportedException(); }
            public Enumerator GetEnumerator() => this;
            IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;
            IEnumerator IEnumerable.GetEnumerator() => this;
            public void Dispose() { }
        }
    }

}
