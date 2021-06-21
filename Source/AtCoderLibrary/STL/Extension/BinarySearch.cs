using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AtCoder.Extension
{
    using static MethodImplOptions;
    public static class BinarySearchExtension
    {
        private struct DefaultComparer<T> : IComparer<T> where T : IComparable<T>
        {
            public int Compare(T x, T y) => x.CompareTo(y);
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
    }
}
