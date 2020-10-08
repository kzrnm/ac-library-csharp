using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder
{
    public static class StlFunction
    {
        public struct NextPermutationEnumerator<T> : IEnumerator<T[]>, IEnumerable<T[]> where T : IComparable<T>
        {
            private readonly IEnumerable<T> _orig;
            internal NextPermutationEnumerator(IEnumerable<T> orig)
            {
                _orig = orig;
                Current = null;
            }

            public T[] Current { get; private set; }

            public bool MoveNext()
            {
                if (Current == null)
                {
                    Current = _orig.ToArray();
                    return true;
                }

                var arr = Current;

                int i;
                for (i = arr.Length - 2; i >= 0; i--)
                    if (arr[i].CompareTo(arr[i + 1]) < 0)
                        break;
                if (i < 0)
                    return false;
                int j;
                for (j = arr.Length - 1; j >= 0; j--)
                    if (arr[i].CompareTo(arr[j]) < 0)
                        break;
                (arr[i], arr[j]) = (arr[j], arr[i]);
                Array.Reverse(arr, i + 1, arr.Length - i - 1);
                return true;
            }
            public void Reset() => Current = null;
            object IEnumerator.Current => Current;
            void IDisposable.Dispose() { }
            public NextPermutationEnumerator<T> GetEnumerator() => this;
            IEnumerator<T[]> IEnumerable<T[]>.GetEnumerator() => this;
            IEnumerator IEnumerable.GetEnumerator() => this;
        }

        /// <summary>
        /// 辞書順によるその次の順列を生成します。返すインスタンスは共通です。
        /// </summary>
        public static NextPermutationEnumerator<T> NextPermutation<T>(IEnumerable<T> orig) where T : IComparable<T>
            => new NextPermutationEnumerator<T>(orig);
    }
}
