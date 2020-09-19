using System;
using System.Collections.Generic;
using System.Text;

namespace AtCoder
{
    public static partial class String
    {
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
            if (n == 0) return new int[] { };
            int[] z = new int[n];
            z[0] = 0;
            for (int i = 1, j = 0; i < n; i++)
            {
                ref int k = ref z[i];
                k = (j + z[j] <= i) ? 0 : System.Math.Min(j + z[j] - i, z[i - j]);
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
    }
}
