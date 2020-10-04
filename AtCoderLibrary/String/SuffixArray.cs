using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AtCoder
{
    public static partial class StringLib
    {
        /// <summary>
        /// 列 <paramref name="m"/> の Suffix Array として、長さ |<paramref name="m"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="m"/>|&lt;10^8</para>
        /// <para>計算量: 時間O(|<paramref name="m"/>|log|<paramref name="m"/>|), 空間O(|<paramref name="m"/>|)</para>
        /// </remarks>
        private static int[] SuffixArray<T>(ReadOnlyMemory<T> m)
        {
            var s = m.Span;
            var n = m.Length;
            var idx = Enumerable.Range(0, n).ToArray();
            Array.Sort(idx, Compare);
            var s2 = new int[n];
            var now = 0;

            // 座標圧縮
            for (int i = 0; i < idx.Length; i++)
            {
                if (i > 0 && !EqualityComparer<T>.Default.Equals(s[idx[i - 1]], s[idx[i]]))
                {
                    now++;
                }
                s2[idx[i]] = now;
            }

            return Internal.String.SAIS(s2, now);

            int Compare(int l, int r)
            {
                var s = m.Span;
                return Comparer<T>.Default.Compare(s[l], s[r]);
            }
        }

        /// <summary>
        /// 文字列 <paramref name="s"/> の Suffix Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|&lt;10^8</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] SuffixArray(string s)
        {
            var n = s.Length;
            int[] s2 = s.Select(c => (int)c).ToArray();
            return Internal.String.SAIS(s2, char.MaxValue);
        }


        /// <summary>
        /// 数列 <paramref name="s"/> の Suffix Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|&lt;10^8</para>
        /// <para>計算量: 時間O(|<paramref name="s"/>|log|<paramref name="s"/>|), 空間O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] SuffixArray<T>(T[] s) => SuffixArray<T>(s.AsMemory());

        /// <summary>
        /// 数列 <paramref name="s"/> の Suffix Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|&lt;10^8, <paramref name="s"/> のすべての要素 x について 0≤x≤<paramref name="upper"/></para>
        /// <para>計算量: O(|<paramref name="s"/>|+<paramref name="upper"/>)</para>
        /// </remarks>
        public static int[] SuffixArray(int[] s, int upper)
        {
            Debug.Assert(0 <= upper);
            foreach (var si in s)
            {
                Debug.Assert(unchecked((uint)si) <= upper);
            }
            return Internal.String.SAIS(s, upper);
        }
    }
}
