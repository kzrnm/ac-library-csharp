using System;
using System.Collections.Generic;
using System.Text;

namespace AtCoder
{
    public static partial class String
    {
        /// <summary>
        /// 文字列 <paramref name="s"/> の Suffix Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|&lt;10^8</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] SuffixArray(string s) { throw new NotImplementedException(); }

        /// <summary>
        /// 数列 <paramref name="s"/> の Suffix Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|&lt;10^8</para>
        /// <para>計算量: 時間O(|<paramref name="s"/>|log|<paramref name="s"/>|), 空間O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] SuffixArray(int[] s) { throw new NotImplementedException(); }
        /// <summary>
        /// 数列 <paramref name="s"/> の Suffix Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|&lt;10^8</para>
        /// <para>計算量: 時間O(|<paramref name="s"/>|log|<paramref name="s"/>|), 空間O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] SuffixArray(uint[] s) { throw new NotImplementedException(); }
        /// <summary>
        /// 数列 <paramref name="s"/> の Suffix Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|&lt;10^8</para>
        /// <para>計算量: 時間O(|<paramref name="s"/>|log|<paramref name="s"/>|), 空間O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] SuffixArray(long[] s) { throw new NotImplementedException(); }
        /// <summary>
        /// 数列 <paramref name="s"/> の Suffix Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|&lt;10^8</para>
        /// <para>計算量: 時間O(|<paramref name="s"/>|log|<paramref name="s"/>|), 空間O(|<paramref name="s"/>|)</para>
        /// </remarks>
        public static int[] SuffixArray(ulong[] s) { throw new NotImplementedException(); }

        /// <summary>
        /// 数列 <paramref name="s"/> の Suffix Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|&lt;10^8, <paramref name="s"/> のすべての要素 x について 0≤x≤<paramref name="upper"/></para>
        /// <para>計算量: O(|<paramref name="s"/>|+<paramref name="upper"/>)</para>
        /// </remarks>
        public static int[] SuffixArray(int[] s, int upper) { throw new NotImplementedException(); }
    }
}
