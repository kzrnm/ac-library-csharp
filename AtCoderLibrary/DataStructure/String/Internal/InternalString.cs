using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder.Internal
{
    public static class String
    {
        /// <summary>
        /// 数列 <paramref name="sm"/> の Suffix Array をナイーブな文字列比較により求め、長さ |<paramref name="sm"/>| の配列として返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="sm"/>|&lt;10^8</para>
        /// <para>計算量: 時間O(|<paramref name="sm"/>|^2 log|<paramref name="sm"/>|), 空間O(|<paramref name="sm"/>|)</para>
        /// </remarks>
        private static int[] SANaive(ReadOnlyMemory<int> sm)
        {
            var n = sm.Length;
            var sa = Enumerable.Range(0, n).ToArray();
            Array.Sort(sa, Compare);
            return sa;

            int Compare(int l, int r)
            {
                // l == r にはなり得ない
                var s = sm.Span;
                while (l < s.Length && r < s.Length)
                {
                    if (s[l] != s[r])
                    {
                        return s[l] - s[r];
                    }
                    l++;
                    r++;
                }

                return r - l;
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
        private static int[] SADoubling(ReadOnlyMemory<int> sm)
        {
            var s = sm.Span;
            var n = s.Length;
            var sa = Enumerable.Range(0, n).ToArray();
            var rnk = new int[n];
            var tmp = new int[n];
            s.CopyTo(rnk);

            for (int k = 1; k < n; k <<= 1)
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
                        return rnk[x] - rnk[y];
                    }

                    int rx = x + k < n ? rnk[x + k] : -1;
                    int ry = y + k < n ? rnk[y + k] : -1;

                    return rx - ry;
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
        public static int[] SAIS(ReadOnlyMemory<int> sm, int upper) => SAIS(sm, upper, 10, 40);

        /// <summary>
        /// 数列 <paramref name="sm"/> の Suffix Array を SA-IS 等により求め、長さ |<paramref name="sm"/>| の配列を返す。
        /// </summary>
        /// <remarks>
        /// <para>Suffix Array sa は (0,1,…,n−1) の順列であって、i=0,1,⋯,n−2 について s[sa[i]..n)&lt;s[sa[i+1]..n) を満たすもの。</para>
        /// <para>制約: 0≤|<paramref name="sm"/>|&lt;10^8</para>
        /// <para>計算量: O(|<paramref name="sm"/>|)</para>
        /// </remarks>
        public static int[] SAIS(ReadOnlyMemory<int> sm, int upper, int thresholdNaive, int thresholdDouling)
        {
            var s = sm.Span;
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
                return SANaive(sm);
            }
            else if (n < thresholdDouling)
            {
                return SADoubling(sm);
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

            var lmsMap = GetFilledArray(-1, n + 1);
            int m = 0;
            for (int i = 1; i < ls.Length; i++)
            {
                if (!ls[i - 1] && ls[i])
                {
                    lmsMap[i] = m++;
                }
            }

            var lms = new List<int>(m);
            for (int i = 1; i < ls.Length; i++)
            {
                if (!ls[i - 1] && ls[i])
                {
                    lms.Add(i);
                }
            }

            Induce(lms);

            // LMSを再帰的にソート
            if (m > 0)
            {
                var sortedLms = new List<int>(m);
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

                var recSA = SAIS(recS, recUpper, thresholdNaive, thresholdDouling);

                for (int i = 0; i < sortedLms.Count; i++)
                {
                    sortedLms[i] = lms[recSA[i]];
                }

                Induce(sortedLms);
            }

            return sa;

            void Induce(List<int> lms)
            {
                var s = sm.Span;
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

        /// <summary>
        /// 各要素が <paramref name="value"/> で初期化された長さ <paramref name="length"/> の配列を取得する。
        /// </summary>
        private static T[] GetFilledArray<T>(T value, int length)
        {
            // Enumerable.Repeatより1-2割ほど高速（64bit環境、intの場合）
            // |           Method |     Mean |   Error |  StdDev |
            // |----------------- |---------:|--------:|--------:|
            // | EnumerableRepeat | 212.7 ms | 2.99 ms | 2.80 ms |
            // |         SpanFill | 178.7 ms | 1.29 ms | 1.14 ms |

            // ちなみにEnumerable.Rangeとnew[] + for文とでは有意差は見られない
            // |          Method |     Mean |   Error |  StdDev |
            // |---------------- |---------:|--------:|--------:|
            // | EnumerableRange | 225.6 ms | 4.35 ms | 3.85 ms |
            // |         SpanFor | 223.0 ms | 2.88 ms | 2.69 ms |

            var result = new T[length];
            result.AsSpan().Fill(value);
            return result;
        }
    }
}
