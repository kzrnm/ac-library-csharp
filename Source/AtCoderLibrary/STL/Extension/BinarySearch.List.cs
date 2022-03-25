using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AtCoder.Extension
{
    public static class BinarySearchListExtension
    {
        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> 以上の値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(n)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static int LowerBound<T, TOp>(this IList<T> a, T v, TOp cmp) where TOp : IComparer<T>
            => LowerBound(a, new CmpWrapper<T, TOp>(v, cmp));
        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> 以上の値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(n)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static int LowerBound<T, TCv>(this IList<T> a, TCv v) where TCv : IComparable<T>
            => BinarySearch<T, TCv, L>(a, v);
        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> より大きい値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(n)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static int UpperBound<T, TOp>(this IList<T> a, T v, TOp cmp) where TOp : IComparer<T>
            => UpperBound(a, new CmpWrapper<T, TOp>(v, cmp));
        /// <summary>
        /// <paramref name="a"/> の要素のうち、<paramref name="v"/> より大きい値が現れる最小のインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="a"/> がソート済み</para>
        /// <para>計算量: O(n)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static int UpperBound<T, TCv>(this IList<T> a, TCv v) where TCv : IComparable<T>
            => BinarySearch<T, TCv, U>(a, v);

        private readonly struct CmpWrapper<T, TCmp> : IComparable<T>
            where TCmp : IComparer<T>
        {
            readonly T v;
            readonly TCmp cmp;
            public CmpWrapper(T v, TCmp cmp)
            {
                this.v = v;
                this.cmp = cmp;
            }

            [MethodImpl(256)]
            public int CompareTo(T other) => cmp.Compare(v, other);
        }
        private interface IOk { bool Ok(int c); }
        private struct L : IOk {[MethodImpl(256)] public bool Ok(int c) => c <= 0; }
        private struct U : IOk {[MethodImpl(256)] public bool Ok(int c) => c < 0; }

        [MethodImpl(256)]
        private static int BinarySearch<T, TCv, TOk>(this IList<T> a, TCv v)
            where TCv : IComparable<T>
            where TOk : IOk
        {
            int ok = a.Count;
            int ng = -1;
            while (ok - ng > 1)
            {
                var m = (ok + ng) >> 1;
                var c = v.CompareTo(a[m]);
                if (default(TOk).Ok(c)) ok = m;
                else ng = m;
            }
            return ok;
        }
    }
}
