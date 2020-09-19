using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AtCoder
{
    public static partial class StringLib
    {
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
    }
}
