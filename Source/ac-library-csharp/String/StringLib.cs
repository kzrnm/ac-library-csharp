﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    public static class StringLib
    {
        #region LcpArray
        /// <summary>
        /// 列 <paramref name="s"/> の LCP Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>LCP Array とは、i 番目の要素が s[sa[i]..|<paramref name="s"/>|), s[sa[i+1]..|<paramref name="s"/>|) の LCP(Longest Common Prefix) の長さのもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|≤10^8, <paramref name="sa"/> は <paramref name="s"/> の Suffix Array</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static int[] LcpArray<T>(ReadOnlySpan<T> s, int[] sa)
        {
            Contract.Assert(1 <= s.Length, reason: $"{nameof(s)} must contain any");
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
        [MethodImpl(256)] public static int[] LcpArray(string s, int[] sa) => LcpArray(s.AsSpan(), sa);

        /// <summary>
        /// 数列 <paramref name="s"/> の LCP Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>LCP Array とは、i 番目の要素が s[sa[i]..|<paramref name="s"/>|), s[sa[i+1]..|<paramref name="s"/>|) の LCP(Longest Common Prefix) の長さのもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|≤10^8, <paramref name="sa"/> は <paramref name="s"/> の Suffix Array</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        [MethodImpl(256)] public static int[] LcpArray<T>(T[] s, int[] sa) => LcpArray((ReadOnlySpan<T>)s, sa);

        /// <summary>
        /// 数列 <paramref name="s"/> の LCP Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>LCP Array とは、i 番目の要素が s[sa[i]..|<paramref name="s"/>|), s[sa[i+1]..|<paramref name="s"/>|) の LCP(Longest Common Prefix) の長さのもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|≤10^8, <paramref name="sa"/> は <paramref name="s"/> の Suffix Array</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        [MethodImpl(256)] public static int[] LcpArray<T>(Span<T> s, int[] sa) => LcpArray((ReadOnlySpan<T>)s, sa);
        #endregion LcpArray

        #region SuffixArray
        /// <summary>
        /// 文字列 <paramref name="s"/> の Suffix Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|&lt;10^8</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        [MethodImpl(256)] public static int[] SuffixArray(string s) => SuffixArray(s.AsSpan());

        /// <summary>
        /// 数列 <paramref name="s"/> の Suffix Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|&lt;10^8</para>
        /// <para>計算量: 時間O(|<paramref name="s"/>|log|<paramref name="s"/>|), 空間O(|<paramref name="s"/>|)</para>
        /// </remarks>
        [MethodImpl(256)] public static int[] SuffixArray<T>(T[] s) => SuffixArray((ReadOnlySpan<T>)s);

        /// <summary>
        /// 数列 <paramref name="s"/> の Suffix Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|&lt;10^8</para>
        /// <para>計算量: 時間O(|<paramref name="s"/>|log|<paramref name="s"/>|), 空間O(|<paramref name="s"/>|)</para>
        /// </remarks>
        [MethodImpl(256)] public static int[] SuffixArray<T>(Span<T> s) => SuffixArray((ReadOnlySpan<T>)s);

        /// <summary>
        /// 列 <paramref name="m"/> の Suffix Array として、長さ |<paramref name="m"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="m"/>|&lt;10^8</para>
        /// <para>計算量: 時間O(|<paramref name="m"/>|log|<paramref name="m"/>|), 空間O(|<paramref name="m"/>|)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static int[] SuffixArray<T>(ReadOnlySpan<T> s)
        {
            var idx = CreateIdx(s);
            var s2 = new int[s.Length];
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

            return InternalString.SaIs(s2, now);
        }
        [MethodImpl(256)]
        static int[] CreateIdx<T>(ReadOnlySpan<T> m)
        {
            var idx = Enumerable.Range(0, m.Length).ToArray();
            Array.Sort(m.ToArray(), idx);
            return idx;
        }

        /// <summary>
        /// 数列 <paramref name="s"/> の Suffix Array として、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="s"/>|&lt;10^8, <paramref name="s"/> のすべての要素 x について 0≤x≤<paramref name="upper"/></para>
        /// <para>計算量: O(|<paramref name="s"/>|+<paramref name="upper"/>)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static int[] SuffixArray(int[] s, int upper)
        {
            Contract.Assert(0 <= upper, reason: $"{nameof(upper)} must be positive.");
#if EMBEDDING
            Contract.Assert(s.All(si => (uint)si <= (uint)upper), reason: $"si ∈ {nameof(s)} must be 0 <= si && si <= {nameof(upper)}");
#endif
            return InternalString.SaIs(s, upper);
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
        [MethodImpl(256)]
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
        [MethodImpl(256)] public static int[] ZAlgorithm(string s) => ZAlgorithm(s.AsSpan());

        /// <summary>
        /// i 番目の要素は s[0..|<paramref name="s"/>|) と s[i..|<paramref name="s"/>|) の LCP(Longest Common Prefix) の長さであるような、長さ |<paramref name="s"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="s"/>|≤10^8</para>
        /// <para>計算量: O(|<paramref name="s"/>|)</para>
        /// </remarks>
        [MethodImpl(256)] public static int[] ZAlgorithm<T>(T[] s) => ZAlgorithm((ReadOnlySpan<T>)s);
        #endregion ZAlgorithm
    }
    namespace Internal
    {
        public static class InternalString
        {
            /// <summary>
            /// 数列 <paramref name="sm"/> の Suffix Array をナイーブな文字列比較により求め、長さ |<paramref name="sm"/>| の配列として返す。
            /// </summary>
            /// <remarks>
            /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
            /// <para>制約: 0≤|<paramref name="sm"/>|&lt;10^8</para>
            /// <para>計算量: 時間O(|<paramref name="sm"/>|^2 log|<paramref name="sm"/>|), 空間O(|<paramref name="sm"/>|)</para>
            /// </remarks>
            public static int[] SaNaive(int[] s)
            {
                var sa = Enumerable.Range(0, s.Length).ToArray();
                Array.Sort(sa, Compare);
                return sa;

                int Compare(int l, int r)
                {
                    // l == r にはなり得ない
                    while (l < s.Length && r < s.Length)
                    {
                        if (s[l] != s[r])
                        {
                            return s[l].CompareTo(s[r]);
                        }
                        l++;
                        r++;
                    }

                    return r.CompareTo(l);
                }
            }

            /// <summary>
            /// 数列 <paramref name="sm"/> の Suffix Array をダブリングにより求め、長さ |<paramref name="sm"/>| の配列として返す。
            /// </summary>
            /// <remarks>
            /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
            /// <para>制約: 0≤|<paramref name="sm"/>|&lt;10^8</para>
            /// <para>計算量: 時間O(|<paramref name="sm"/>|(log|<paramref name="sm"/>|)^2), 空間O(|<paramref name="sm"/>|)</para>
            /// </remarks>
            [MethodImpl(256)]
            public static int[] SaDoubling(ReadOnlySpan<int> s)
            {
                var sa = Enumerable.Range(0, s.Length).ToArray();
                var rnk = new int[s.Length];
                var tmp = new int[s.Length];
                s.CopyTo(rnk);

                for (int k = 1; k < rnk.Length; k <<= 1)
                {
                    Array.Sort(sa, Compare);
                    tmp[sa[0]] = 0;
                    for (int i = 1; i < sa.Length; i++)
                    {
                        tmp[sa[i]] = tmp[sa[i - 1]] + (Compare(sa[i - 1], sa[i]) < 0 ? 1 : 0);
                    }
                    (tmp, rnk) = (rnk, tmp);

                    int Compare(int x, int y)
                    {
                        if (rnk[x] != rnk[y])
                        {
                            return rnk[x].CompareTo(rnk[y]);
                        }

                        int rx = x + k < rnk.Length ? rnk[x + k] : -1;
                        int ry = y + k < rnk.Length ? rnk[y + k] : -1;

                        return rx.CompareTo(ry);
                    }
                }

                return sa;
            }

            /// <summary>
            /// 数列 <paramref name="sm"/> の Suffix Array を SA-IS 等により求め、長さ |<paramref name="sm"/>| の配列を返す。
            /// </summary>
            /// <remarks>
            /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
            /// <para>制約: 0≤|<paramref name="sm"/>|&lt;10^8</para>
            /// <para>計算量: O(|<paramref name="sm"/>|)</para>
            /// </remarks>
            [MethodImpl(256)] public static int[] SaIs(ReadOnlySpan<int> sm, int upper) => SaIs(sm, upper, 10, 40);

            /// <summary>
            /// 数列 <paramref name="sm"/> の Suffix Array を SA-IS 等により求め、長さ |<paramref name="sm"/>| の配列を返す。
            /// </summary>
            /// <remarks>
            /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
            /// <para>制約: 0≤|<paramref name="sm"/>|&lt;10^8</para>
            /// <para>計算量: O(|<paramref name="sm"/>|)</para>
            /// </remarks>
            [MethodImpl(256)]
            public static int[] SaIs(ReadOnlySpan<int> s, int upper, int thresholdNaive, int thresholdDouling)
            {
                var n = s.Length;

                if (n == 0)
                {
                    return Array.Empty<int>();
                }
                else if (n == 1)
                {
                    return new int[] { 0 };
                }
                else if (n == 2)
                {
                    if (s[0] < s[1])
                    {
                        return new int[] { 0, 1 };
                    }
                    else
                    {
                        return new int[] { 1, 0 };
                    }
                }
                else if (n < thresholdNaive)
                {
                    return SaNaive(s.ToArray());
                }
                else if (n < thresholdDouling)
                {
                    return SaDoubling(s);
                }

                var sa = new int[n];
                var ls = new bool[n];

                for (int i = sa.Length - 2; i >= 0; i--)
                {
                    // S-typeならtrue、L-typeならfalse
                    ls[i] = (s[i] == s[i + 1]) ? ls[i + 1] : (s[i] < s[i + 1]);
                }

                // バケットサイズの累積和（＝開始インデックス）
                var sumL = new int[upper + 1];
                var sumS = new int[upper + 1];

                for (int i = 0; i < s.Length; i++)
                {
                    if (!ls[i])
                    {
                        sumS[s[i]]++;
                    }
                    else
                    {
                        sumL[s[i] + 1]++;
                    }
                }

                for (int i = 0; i < sumL.Length; i++)
                {
                    sumS[i] += sumL[i];
                    if (i < upper)
                    {
                        sumL[i + 1] += sumS[i];
                    }
                }

                var lmsMap = new int[n + 1];
                lmsMap.AsSpan().Fill(-1);
                int m = 0;
                for (int i = 1; i < ls.Length; i++)
                {
                    if (!ls[i - 1] && ls[i])
                    {
                        lmsMap[i] = m++;
                    }
                }

                var lms = new SimpleList<int>(m);
                for (int i = 1; i < ls.Length; i++)
                {
                    if (!ls[i - 1] && ls[i])
                    {
                        lms.Add(i);
                    }
                }

                Induce(lms, s, sa, ls, sumS, sumL);

                // LMSを再帰的にソート
                // m の値は再帰ごとに半分以下になるので再帰の回数も log(n) に抑えられる
                if (m > 0)
                {
                    var sortedLms = new SimpleList<int>(m);
                    foreach (var v in sa)
                    {
                        if (lmsMap[v] != -1)
                        {
                            sortedLms.Add(v);
                        }
                    }

                    var recS = new int[m];
                    var recUpper = 0;
                    recS[lmsMap[sortedLms[0]]] = 0;

                    // 同じLMS同士をまとめていく
                    for (int i = 1; i < sortedLms.Count; i++)
                    {
                        var l = sortedLms[i - 1];
                        var r = sortedLms[i];
                        var endL = (lmsMap[l] + 1 < m) ? lms[lmsMap[l] + 1] : n;
                        var endR = (lmsMap[r] + 1 < m) ? lms[lmsMap[r] + 1] : n;
                        var same = true;

                        if (endL - l != endR - r)
                        {
                            same = false;
                        }
                        else
                        {
                            while (l < endL)
                            {
                                if (s[l] != s[r])
                                {
                                    break;
                                }
                                l++;
                                r++;
                            }

                            if (l == n || s[l] != s[r])
                            {
                                same = false;
                            }
                        }

                        if (!same)
                        {
                            recUpper++;
                        }

                        recS[lmsMap[sortedLms[i]]] = recUpper;
                    }

                    var recSA = SaIs(recS, recUpper, thresholdNaive, thresholdDouling);

                    for (int i = 0; i < sortedLms.Count; i++)
                    {
                        sortedLms[i] = lms[recSA[i]];
                    }

                    Induce(sortedLms, s, sa, ls, sumS, sumL);
                }

                return sa;
            }
            [MethodImpl(256)]
            public static void Induce(SimpleList<int> lms, ReadOnlySpan<int> s, int[] sa, bool[] ls, int[] sumS, int[] sumL)
            {
                var n = s.Length;
                sa.AsSpan().Fill(-1);
                var buf = new int[sumS.Length];

                // LMS
                sumS.AsSpan().CopyTo(buf);
                foreach (var d in lms)
                {
                    if (d == n)
                    {
                        continue;
                    }
                    sa[buf[s[d]]++] = d;
                }

                // L-type
                sumL.AsSpan().CopyTo(buf);
                sa[buf[s[n - 1]]++] = n - 1;
                for (int i = 0; i < sa.Length; i++)
                {
                    int v = sa[i];
                    if (v >= 1 && !ls[v - 1])
                    {
                        sa[buf[s[v - 1]]++] = v - 1;
                    }
                }

                // S-type
                sumL.AsSpan().CopyTo(buf);
                for (int i = sa.Length - 1; i >= 0; i--)
                {
                    int v = sa[i];
                    if (v >= 1 && ls[v - 1])
                    {
                        sa[--buf[s[v - 1] + 1]] = v - 1;
                    }
                }
            }
        }
    }
}
