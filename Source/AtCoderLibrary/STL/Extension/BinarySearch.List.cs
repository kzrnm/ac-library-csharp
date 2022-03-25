using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AtCoder.Extension
{
    public static class BinarySearchListExtension
    {
        private struct DefaultComparer<T> : IComparer<T> where T : IComparable<T>
        {
            [MethodImpl(256)]
            public int Compare(T x, T y) => x.CompareTo(y);
        }
        private interface IOk
        {
            bool Ok(int c);
        }
        private struct L : IOk
        {
            [MethodImpl(256)]
            public bool Ok(int c) => c >= 0;
        }
        private struct U : IOk
        {
            [MethodImpl(256)]
            public bool Ok(int c) => c > 0;
        }

        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> 以上の値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(n)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static int LowerBound<T>(this IList<T> a, T v) where T : IComparable<T>
            => LowerBound(a, v, default(DefaultComparer<T>));
        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> 以上の値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(n)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static int LowerBound<T, TOp>(this IList<T> a, T v, TOp cmp) where TOp : IComparer<T>
            => BinarySearch<T, TOp, L>(a, v, cmp);
        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> より大きい値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(n)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static int UpperBound<T>(this IList<T> a, T v) where T : IComparable<T>
            => UpperBound(a, v, default(DefaultComparer<T>));
        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> より大きい値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(n)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static int UpperBound<T, TOp>(this IList<T> a, T v, TOp cmp) where TOp : IComparer<T>
            => BinarySearch<T, TOp, U>(a, v, cmp);
        [MethodImpl(256)]
        private static int BinarySearch<T, TOp, TOk>(this IList<T> a, T v, TOp cmp)
            where TOp : IComparer<T>
            where TOk : IOk
        {
            int ok = a.Count;
            int ng = -1;
            while (ok - ng > 1)
            {
                var m = (ok + ng) >> 1;
                var c = cmp.Compare(a[m], v);
                if (default(TOk).Ok(c)) ok = m;
                else ng = m;
            }
            return ok;
        }
    }
}
