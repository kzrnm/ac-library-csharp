using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AtCoder
{
    public static class StringLib
    {
        #region LCPArray
        /// <summary>
        /// 列 <paramref name="s"/> の LCP Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>LCP Array とは、i 番目の要素が s[sa[i]..|<paramref name="s"/>|), s[sa[i+1]..|<paramref name="s"/>|) の LCP(Longest Common Prefix) の長さのもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|≤10^8, <paramref name="sa"/> は <paramref name="s"/> の Suffix Array</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] LCPArray<T>(ReadOnlySpan<T> s, int[] sa)
        {
            Debug.Assert(1 <= s.Length);
            int[] rnk = new int[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                rnk[sa[i]] = i;
            }
            int[] lcp = new int[s.Length - 1];
            int h = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (h > 0) h--;
                if (rnk[i] == 0) continue;
                int j = sa[rnk[i] - 1];
                for (; j + h < s.Length && i + h < s.Length; h++)
                {
                    if (!EqualityComparer<T>.Default.Equals(s[j + h], s[i + h])) break;
                }
                lcp[rnk[i] - 1] = h;
            }
            return lcp;
        }

        /// <summary>
        /// 文字列 <paramref name="s"/> の LCP Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>LCP Array とは、i 番目の要素が s[sa[i]..|<paramref name="s"/>|), s[sa[i+1]..|<paramref name="s"/>|) の LCP(Longest Common Prefix) の長さのもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|≤10^8, <paramref name="sa"/> は <paramref name="s"/> の Suffix Array</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] LCPArray(string s, int[] sa) => LCPArray(s.AsSpan(), sa);

        /// <summary>
        /// 数列 <paramref name="s"/> の LCP Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>LCP Array とは、i 番目の要素が s[sa[i]..|<paramref name="s"/>|), s[sa[i+1]..|<paramref name="s"/>|) の LCP(Longest Common Prefix) の長さのもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|≤10^8, <paramref name="sa"/> は <paramref name="s"/> の Suffix Array</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] LCPArray<T>(T[] s, int[] sa) => LCPArray((ReadOnlySpan<T>)s, sa);
        #endregion LCPArray

        #region SuffixArray
        /// <summary>
        /// 列 <paramref name="m"/> の Suffix Array として、長さ |<paramref name="m"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="m"/>|&lt;10^8</para>
        /// <para>計算量: 時間O(|<paramref name="m"/>|log|<paramref name="m"/>|), 空間O(|<paramref name="m"/>|)</para>
        /// </remarks>
        internal static int[] SuffixArray<T>(ReadOnlyMemory<T> m)
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

            return Internal.InternalString.SAIS(s2, now);

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
            return Internal.InternalString.SAIS(s2, char.MaxValue);
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
            return Internal.InternalString.SAIS(s, upper);
        }
        #endregion SuffixArray

        #region ZAlgorithm
        /// <summary>
        /// i 番目の要素は s[0..|<paramref name="s"/>|) と s[i..|<paramref name="s"/>|) の LCP(Longest Common Prefix) の長さであるような、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="s"/>|≤10^8</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] ZAlgorithm<T>(ReadOnlySpan<T> s)
        {
            int n = s.Length;
            if (n == 0) return Array.Empty<int>();
            int[] z = new int[n];
            z[0] = 0;
            for (int i = 1, j = 0; i < n; i++)
            {
                ref int k = ref z[i];
                k = (j + z[j] <= i) ? 0 : Math.Min(j + z[j] - i, z[i - j]);
                while (i + k < n && EqualityComparer<T>.Default.Equals(s[k], s[i + k])) k++;
                if (j + z[j] < i + z[i]) j = i;
            }
            z[0] = n;
            return z;
        }

        /// <summary>
        /// i 番目の要素は s[0..|<paramref name="s"/>|) と s[i..|<paramref name="s"/>|) の LCP(Longest Common Prefix) の長さであるような、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="s"/>|≤10^8</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] ZAlgorithm(string s) => ZAlgorithm(s.AsSpan());

        /// <summary>
        /// i 番目の要素は s[0..|<paramref name="s"/>|) と s[i..|<paramref name="s"/>|) の LCP(Longest Common Prefix) の長さであるような、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="s"/>|≤10^8</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] ZAlgorithm<T>(T[] s) => ZAlgorithm((ReadOnlySpan<T>)s);
        #endregion ZAlgorithm
    }
}
