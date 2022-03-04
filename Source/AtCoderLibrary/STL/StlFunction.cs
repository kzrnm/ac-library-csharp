using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    public static class StlFunction
    {
        #region NextPermutation
        public struct NextPermutationEnumerator<T> : IEnumerator<T[]>, IEnumerable<T[]> where T : IComparable<T>
        {
            internal readonly IEnumerable<T> _orig;
            internal NextPermutationEnumerator(IEnumerable<T> orig)
            {
                _orig = orig;
                Current = null;
            }

            public T[] Current { get; private set; }

            [MethodImpl(256)]
            public bool MoveNext()
            {
                if (Current == null)
                {
                    Current = _orig.ToArray();
                    return true;
                }
                return NextPermutation(Current);
            }
            public void Reset() => Current = null;
            object IEnumerator.Current => Current;
            void IDisposable.Dispose() { }
            [MethodImpl(256)] public NextPermutationEnumerator<T> GetEnumerator() => this;
            IEnumerator<T[]> IEnumerable<T[]>.GetEnumerator() => this;
            IEnumerator IEnumerable.GetEnumerator() => this;
        }
        /// <summary>
        /// 順列を辞書順によるその次の順列に更新します。
        /// </summary>
        /// <returns>更新に成功したら <see langword="true"/>。順列の最後ならば <see langword="false"/> </returns>
        [MethodImpl(256)] public static bool NextPermutation<T>(T[] array) where T : IComparable<T> => NextPermutation(array.AsSpan());
        /// <summary>
        /// 順列を辞書順によるその次の順列に更新します。
        /// </summary>
        /// <returns>更新に成功したら <see langword="true"/>。順列の最後ならば <see langword="false"/> </returns>
        [MethodImpl(256)]
        public static bool NextPermutation<T>(Span<T> span) where T : IComparable<T>
        {
            int i;
            for (i = span.Length - 2; i >= 0; i--)
                if (span[i].CompareTo(span[i + 1]) < 0)
                    break;
            if (i < 0)
                return false;
            int j;
            for (j = span.Length - 1; j >= 0; j--)
                if (span[i].CompareTo(span[j]) < 0)
                    break;
            (span[i], span[j]) = (span[j], span[i]);
            span.Slice(i + 1, span.Length - i - 1).Reverse();
            return true;
        }

        /// <summary>
        /// 辞書順による順列を生成します。返すインスタンスは共通です。
        /// </summary>
        [MethodImpl(256)]
        public static NextPermutationEnumerator<T> Permutations<T>(IEnumerable<T> orig) where T : IComparable<T>
            => new NextPermutationEnumerator<T>(orig);
        #endregion NextPermutation

        #region BinarySerch
        private struct DefaultComparer<T> : IComparer<T> where T : IComparable<T>
        {
            [MethodImpl(256)]
            public int Compare(T x, T y) => x.CompareTo(y);
        }

        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="Ok"/>(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="Ok"/>(<paramref name="ok"/>) &amp;&amp; !<paramref name="Ok"/>(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static int BinarySearch(int ok, int ng, Predicate<int> Ok)
        {
            while (Math.Abs(ok - ng) > 1)
            {
                var m = (ok + ng) >> 1;
                if (Ok(m)) ok = m;
                else ng = m;
            }
            return ok;
        }

        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="Ok"/>(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="Ok"/>(<paramref name="ok"/>) &amp;&amp; !<paramref name="Ok"/>(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static long BinarySearch(long ok, long ng, Predicate<long> Ok)
        {
            while (Math.Abs(ok - ng) > 1)
            {
                var m = (ok + ng) >> 1;
                if (Ok(m)) ok = m;
                else ng = m;
            }
            return ok;
        }
        #endregion BinarySerch
    }
}
