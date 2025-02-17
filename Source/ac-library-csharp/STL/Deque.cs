using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

#if !NETSTANDARD2_1
using System.Numerics;
#endif

namespace AtCoder
{
    using static EditorBrowsableState;

    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class Deque<T> : IEnumerable<T>, IReadOnlyCollection<T>, ICollection<T>
    {
        [EditorBrowsable(Never)]
        public T[] data;

        [EditorBrowsable(Never)]
        public int mask;

        [EditorBrowsable(Never)]
        public int head;

        [EditorBrowsable(Never)]
        public int tail;

        public Deque() : this(1) { }
        public Deque(int capacity)
        {
            data = Array.Empty<T>();
            Grow(capacity);
        }

        public int Count => (tail - head) & mask;

        public ref T this[int i] => ref data[(head + i) & mask];
        public T First => data[head];
        public T Last => data[(tail - 1) & mask];
        private static void ThrowDequeIsEmpty() => throw new InvalidOperationException("deque is empty");

        [MethodImpl(256)]
        public T PopFirst()
        {
            if (head == tail) ThrowDequeIsEmpty();
            var item = data[head];
            head = (head + 1) & mask;
            return item;
        }
        [MethodImpl(256)]
        public T PopLast()
        {
            if (head == tail) ThrowDequeIsEmpty();
            return data[tail = (tail - 1) & mask];
        }
        [MethodImpl(256)]
        public void AddFirst(T item)
        {
            var nxt = (head - 1) & mask;
            if (nxt == tail)
            {
                Grow();
                nxt = (head - 1) & mask;
            }
            data[nxt] = item;
            head = nxt;
        }
        [MethodImpl(256)]
        public void AddLast(T item)
        {
            var nxt = (tail + 1) & mask;
            if (head == nxt)
            {
                Grow();
                nxt = (tail + 1) & mask;
            }
            data[tail] = item;
            tail = nxt;
        }

        [EditorBrowsable(Never)]
        public void Grow() => Grow(Math.Max(mask << 1, 0b11));

        [EditorBrowsable(Never)]
        public void Grow(int capacity)
        {
            capacity =
#if NET7_0_OR_GREATER
                (int)BitOperations.RoundUpToPowerOf2((uint)capacity + 1u);
#else
                1 << (InternalBit.CeilPow2(capacity + 1));
#endif
            Debug.Assert(BitOperations.PopCount((uint)capacity) == 1);
            if (capacity <= data.Length) return;
            var oldSize = Count;
            var newArray = new T[capacity];
            if (head <= tail)
            {
                Array.Copy(data, head, newArray, 0, oldSize);
            }
            else
            {
                var hsize = data.Length - head;
                Debug.Assert(hsize + tail == oldSize);
                Array.Copy(data, head, newArray, 0, hsize);
                Array.Copy(data, 0, newArray, hsize, tail);
            }

            data = newArray;
            mask = capacity - 1;
            head = 0;
            tail = oldSize;
        }
        [MethodImpl(256)] public void Clear() => head = tail = 0;

        [EditorBrowsable(Never)]
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

        public void CopyTo(T[] array, int arrayIndex)
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
        [MethodImpl(256)] public Enumerator Reversed() => new Enumerator(this, true);
        [MethodImpl(256)] public Enumerator GetEnumerator() => new Enumerator(this, false);

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
                if (deque.head == deque.tail)
                {
                    index = last = 0;
                }
                else if (isReverse)
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
            [MethodImpl(256)]
            public bool MoveNext()
            {
                if (index == last) return false;
                if (isReverse) --index; else ++index;
                index &= deque.mask;
                return true;
            }
            public void Reset() { throw new NotSupportedException(); }
            [MethodImpl(256)] public Enumerator GetEnumerator() => this;
            IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;
            IEnumerator IEnumerable.GetEnumerator() => this;
            public void Dispose() { }
        }
    }

}
