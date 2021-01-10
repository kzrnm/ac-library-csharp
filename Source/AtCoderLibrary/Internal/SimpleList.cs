using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AtCoder.Internal
{
    public class SimpleList<T> : IList<T>, IReadOnlyList<T>
    {
        private T[] data;
        private const int DefaultCapacity = 2;
        public SimpleList()
        {
            data = new T[DefaultCapacity];
        }
        public SimpleList(int capacity)
        {
            data = new T[Math.Max(capacity, DefaultCapacity)];
        }

        public Span<T> AsSpan() => new Span<T>(data, 0, Count);

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)index >= (uint)Count)
                    ThrowIndexOutOfRangeException();
                return ref data[index];
            }
        }
        public int Count { get; private set; }
        public void Add(T item)
        {
            if ((uint)Count >= (uint)data.Length)
                Array.Resize(ref data, data.Length << 1);
            data[Count++] = item;
        }
        public void RemoveLast()
        {
            if (--Count < 0)
                ThrowIndexOutOfRangeException();
        }
        public SimpleList<T> Reverse()
        {
            Array.Reverse(data, 0, Count);
            return this;
        }
        public SimpleList<T> Reverse(int index, int count)
        {
            Array.Reverse(data, index, count);
            return this;
        }
        public SimpleList<T> Sort()
        {
            Array.Sort(data, 0, Count);
            return this;
        }
        public SimpleList<T> Sort(IComparer<T> comparer)
        {
            Array.Sort(data, 0, Count, comparer);
            return this;
        }
        public SimpleList<T> Sort(int index, int count, IComparer<T> comparer)
        {
            Array.Sort(data, index, count, comparer);
            return this;
        }
        public void Clear() => Count = 0;
        public bool Contains(T item) => IndexOf(item) >= 0;
        public int IndexOf(T item) => Array.IndexOf(data, item, 0, Count);
        public void CopyTo(T[] array, int arrayIndex) => Array.Copy(data, 0, array, arrayIndex, Count);

        #region Interface
        bool ICollection<T>.IsReadOnly => false;
        T IList<T>.this[int index] { get => data[index]; set => data[index] = value; }
        T IReadOnlyList<T>.this[int index] { get => data[index]; }

        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
        bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
        void IList<T>.RemoveAt(int index) => throw new NotSupportedException();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)this).GetEnumerator();
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return data[i];
        }
        #endregion Interface

        public Span<T>.Enumerator GetEnumerator() => AsSpan().GetEnumerator();
        private static void ThrowIndexOutOfRangeException() => throw new IndexOutOfRangeException();
    }
}
