using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    using static MethodImplOptions;
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

#pragma warning disable CA1819 // Properties should not return arrays
            public T[] Current { get; private set; }
#pragma warning restore CA1819 // Properties should not return arrays

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
            public NextPermutationEnumerator<T> GetEnumerator() => this;
            IEnumerator<T[]> IEnumerable<T[]>.GetEnumerator() => this;
            IEnumerator IEnumerable.GetEnumerator() => this;
        }
        /// <summary>
        /// 順列を辞書順によるその次の順列に更新します。
        /// </summary>
        /// <returns>更新に成功したら <see langword="true"/>。順列の最後ならば <see langword="false"/> </returns>
        public static bool NextPermutation<T>(T[] array) where T : IComparable<T> => NextPermutation(array.AsSpan());
        /// <summary>
        /// 順列を辞書順によるその次の順列に更新します。
        /// </summary>
        /// <returns>更新に成功したら <see langword="true"/>。順列の最後ならば <see langword="false"/> </returns>
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
        public static NextPermutationEnumerator<T> Permutations<T>(IEnumerable<T> orig) where T : IComparable<T>
            => new NextPermutationEnumerator<T>(orig);
        #endregion NextPermutation

        #region BinarySerch
        private struct DefaultComparer<T> : IComparer<T> where T : IComparable<T>
        {
            public int Compare(T x, T y) => x.CompareTo(y);
        }

        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="Ok"/>(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="Ok"/>(<paramref name="ok"/>) &amp;&amp; !<paramref name="Ok"/>(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
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

        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> 以上の値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int LowerBound<T, TOp>(this T[] a, T v, TOp cmp) where TOp : IComparer<T> => BinarySearch(a.AsSpan(), v, cmp, true);

        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> 以上の値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int LowerBound<T>(this T[] a, T v) where T : IComparable<T> => BinarySearch(a.AsSpan(), v, default(DefaultComparer<T>), true);

        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> より大きい値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int UpperBound<T, TOp>(this T[] a, T v, TOp cmp) where TOp : IComparer<T> => BinarySearch(a.AsSpan(), v, cmp, false);

        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> より大きい値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int UpperBound<T>(this T[] a, T v) where T : IComparable<T> => BinarySearch(a.AsSpan(), v, default(DefaultComparer<T>), false);
        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> 以上の値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int LowerBound<T, TOp>(this IList<T> a, T v, TOp cmp) where TOp : IComparer<T> => BinarySearch(a, v, cmp, true);

        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> 以上の値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int LowerBound<T>(this IList<T> a, T v) where T : IComparable<T> => BinarySearch(a, v, default(DefaultComparer<T>), true);

        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> より大きい値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int UpperBound<T, TOp>(this IList<T> a, T v, TOp cmp) where TOp : IComparer<T> => BinarySearch(a, v, cmp, false);

        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> より大きい値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int UpperBound<T>(this IList<T> a, T v) where T : IComparable<T> => BinarySearch(a, v, default(DefaultComparer<T>), false);
        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> 以上の値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int LowerBound<T, TOp>(this Span<T> a, T v, TOp cmp) where TOp : IComparer<T> => BinarySearch(a, v, cmp, true);

        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> 以上の値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int LowerBound<T>(this Span<T> a, T v) where T : IComparable<T> => BinarySearch(a, v, default(DefaultComparer<T>), true);

        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> より大きい値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int UpperBound<T, TOp>(this Span<T> a, T v, TOp cmp) where TOp : IComparer<T> => BinarySearch(a, v, cmp, false);

        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> より大きい値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int UpperBound<T>(this Span<T> a, T v) where T : IComparable<T> => BinarySearch(a, v, default(DefaultComparer<T>), false);
        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> 以上の値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int LowerBound<T, TOp>(this ReadOnlySpan<T> a, T v, TOp cmp) where TOp : IComparer<T> => BinarySearch(a, v, cmp, true);

        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> 以上の値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int LowerBound<T>(this ReadOnlySpan<T> a, T v) where T : IComparable<T> => BinarySearch(a, v, default(DefaultComparer<T>), true);

        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> より大きい値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int UpperBound<T, TOp>(this ReadOnlySpan<T> a, T v, TOp cmp) where TOp : IComparer<T> => BinarySearch(a, v, cmp, false);

        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> より大きい値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(log <paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static int UpperBound<T>(this ReadOnlySpan<T> a, T v) where T : IComparable<T> => BinarySearch(a, v, default(DefaultComparer<T>), false);
        private static int BinarySearch<T, TOp>(this IList<T> a, T v, TOp cmp, bool isLowerBound)
            where TOp : IComparer<T>
        {
            int ok = a.Count;
            int ng = -1;
            while (ok - ng > 1)
            {
                var m = (ok + ng) >> 1;
                var c = cmp.Compare(a[m], v);
                if (c > 0 || (c == 0 && isLowerBound)) ok = m;
                else ng = m;
            }
            return ok;
        }
        private static int BinarySearch<T, TOp>(this ReadOnlySpan<T> a, T v, TOp cmp, bool isLowerBound)
            where TOp : IComparer<T>
        {
            int ok = a.Length;
            int ng = -1;
            while (ok - ng > 1)
            {
                var m = (ok + ng) >> 1;
                var c = cmp.Compare(a[m], v);
                if (c > 0 || (c == 0 && isLowerBound)) ok = m;
                else ng = m;
            }
            return ok;
        }
        #endregion BinarySerch
    }
}
