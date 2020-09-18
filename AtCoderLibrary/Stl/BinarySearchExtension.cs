using System;
using System.Collections.Generic;

namespace AtCoder.Stl
{
    public static class BinarySearchExtension
    {
        public static int BinarySearch(int ok, int ng, Predicate<int> Ok)
        {
            while (System.Math.Abs(ok - ng) > 1)
            {
                var m = (ok + ng) >> 1;
                if (Ok(m)) ok = m;
                else ng = m;
            }
            return ok;
        }

        /// <summary>
        /// 与えられた比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> 以上の要素であるような最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="T:T[]"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int LowerBound<T>(this T[] a, T v, IComparer<T> cmp) => BinarySearch(a.AsSpan(), v, cmp, true);

        /// <summary>
        /// デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> 以上の要素であるような最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="T:T[]"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int LowerBound<T>(this T[] a, T v) => BinarySearch(a.AsSpan(), v, Comparer<T>.Default, true);

        /// <summary>
        /// 与えられた比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> より真に大きい要素が現れる最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="T:T[]"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int UpperBound<T>(this T[] a, T v, IComparer<T> cmp) => BinarySearch(a.AsSpan(), v, cmp, false);

        /// <summary>
        /// デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> より真に大きい要素が現れる最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="T:T[]"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int UpperBound<T>(this T[] a, T v) => BinarySearch(a.AsSpan(), v, Comparer<T>.Default, false);
        /// <summary>
        /// 与えられた比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> 以上の要素であるような最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int LowerBound<T>(this IList<T> a, T v, IComparer<T> cmp) => BinarySearch(a, v, cmp, true);

        /// <summary>
        /// デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> 以上の要素であるような最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int LowerBound<T>(this IList<T> a, T v) => BinarySearch(a, v, Comparer<T>.Default, true);

        /// <summary>
        /// 与えられた比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> より真に大きい要素が現れる最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int UpperBound<T>(this IList<T> a, T v, IComparer<T> cmp) => BinarySearch(a, v, cmp, false);

        /// <summary>
        /// デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> より真に大きい要素が現れる最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int UpperBound<T>(this IList<T> a, T v) => BinarySearch(a, v, Comparer<T>.Default, false);
        /// <summary>
        /// 与えられた比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> 以上の要素であるような最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="Span{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int LowerBound<T>(this Span<T> a, T v, IComparer<T> cmp) => BinarySearch(a, v, cmp, true);

        /// <summary>
        /// デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> 以上の要素であるような最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="Span{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int LowerBound<T>(this Span<T> a, T v) => BinarySearch(a, v, Comparer<T>.Default, true);

        /// <summary>
        /// 与えられた比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> より真に大きい要素が現れる最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="Span{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int UpperBound<T>(this Span<T> a, T v, IComparer<T> cmp) => BinarySearch(a, v, cmp, false);

        /// <summary>
        /// デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> より真に大きい要素が現れる最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="Span{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int UpperBound<T>(this Span<T> a, T v) => BinarySearch(a, v, Comparer<T>.Default, false);
        /// <summary>
        /// 与えられた比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> 以上の要素であるような最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="ReadOnlySpan{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int LowerBound<T>(this ReadOnlySpan<T> a, T v, IComparer<T> cmp) => BinarySearch(a, v, cmp, true);

        /// <summary>
        /// デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> 以上の要素であるような最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="ReadOnlySpan{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int LowerBound<T>(this ReadOnlySpan<T> a, T v) => BinarySearch(a, v, Comparer<T>.Default, true);

        /// <summary>
        /// 与えられた比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> より真に大きい要素が現れる最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="ReadOnlySpan{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int UpperBound<T>(this ReadOnlySpan<T> a, T v, IComparer<T> cmp) => BinarySearch(a, v, cmp, false);

        /// <summary>
        /// デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> より真に大きい要素が現れる最小のインデックスを取得します．</summary>
        /// <typeparam name="T"><see cref="ReadOnlySpan{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param><returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public static int UpperBound<T>(this ReadOnlySpan<T> a, T v) => BinarySearch(a, v, Comparer<T>.Default, false);

        static int BinarySearch<T>(this IList<T> a, T v, IComparer<T> cmp, bool isLowerBound)
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
        static int BinarySearch<T>(this ReadOnlySpan<T> a, T v, IComparer<T> cmp, bool isLowerBound)
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
