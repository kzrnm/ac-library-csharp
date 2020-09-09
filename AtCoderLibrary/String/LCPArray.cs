using System;
using System.Collections.Generic;
using System.Text;

namespace AtCoder
{
    public static partial class String
    {
        /// <summary>
        /// 列 <paramref name="s"/> の LCP Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>LCP Array とは、i 番目の要素が s[sa[i]..|<paramref name="s"/>|), s[sa[i+1]..|<paramref name="s"/>|) の LCP(Longest Common Prefix) の長さのもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|≤10^8, <paramref name="sa"/> は <paramref name="s"/> の Suffix Array</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] LCPArray<T>(ReadOnlySpan<T> s, int[] sa) { throw new NotImplementedException(); }

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
        public static int[] LCPArray(int[] s, int[] sa) => LCPArray((ReadOnlySpan<int>)s, sa);
        /// <summary>
        /// 数列 <paramref name="s"/> の LCP Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>LCP Array とは、i 番目の要素が s[sa[i]..|<paramref name="s"/>|), s[sa[i+1]..|<paramref name="s"/>|) の LCP(Longest Common Prefix) の長さのもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|≤10^8, <paramref name="sa"/> は <paramref name="s"/> の Suffix Array</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] LCPArray(uint[] s, int[] sa) => LCPArray((ReadOnlySpan<uint>)s, sa);
        /// <summary>
        /// 数列 <paramref name="s"/> の LCP Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>LCP Array とは、i 番目の要素が s[sa[i]..|<paramref name="s"/>|), s[sa[i+1]..|<paramref name="s"/>|) の LCP(Longest Common Prefix) の長さのもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|≤10^8, <paramref name="sa"/> は <paramref name="s"/> の Suffix Array</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] LCPArray(long[] s, int[] sa) => LCPArray((ReadOnlySpan<long>)s, sa);
        /// <summary>
        /// 数列 <paramref name="s"/> の LCP Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>LCP Array とは、i 番目の要素が s[sa[i]..|<paramref name="s"/>|), s[sa[i+1]..|<paramref name="s"/>|) の LCP(Longest Common Prefix) の長さのもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|≤10^8, <paramref name="sa"/> は <paramref name="s"/> の Suffix Array</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] LCPArray(ulong[] s, int[] sa) => LCPArray((ReadOnlySpan<ulong>)s, sa);
    }
}
