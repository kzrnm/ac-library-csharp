﻿using System;
using System.Collections.Generic;
using System.Linq;
using AtCoder.Internal;
using Shouldly;
using Xunit;

namespace AtCoder
{
    public class StringLibTest
    {
        private static int Compare<T>(IEnumerable<T> x, IEnumerable<T> y)
        {
            var xe = x.GetEnumerator();
            var ye = y.GetEnumerator();
            var xm = xe.MoveNext();
            var ym = ye.MoveNext();
            while (xm && ym)
            {
                if (!EqualityComparer<T>.Default.Equals(xe.Current, ye.Current))
                    return Comparer<T>.Default.Compare(xe.Current, ye.Current);
                xm = xe.MoveNext();
                ym = ye.MoveNext();
            }
            if (xm == ym) return 0;
            else if (xm) return 1;
            else return -1;
        }
        private static int[] SaNaive(int[] s)
        {
            int n = s.Length;
            var sa = Enumerable.Range(0, n).ToArray();
            Array.Sort(sa, (l, r) => Compare(s.Skip(l), s.Skip(r)));
            return sa;
        }
        private static int[] LcpNaive(int[] s, int[] sa)
        {
            int n = s.Length;
            n.ShouldNotBe(0);
            var lcp = new int[n - 1];
            for (int i = 0; i < n - 1; i++)
            {
                int l = sa[i], r = sa[i + 1];
                while (l + lcp[i] < n && r + lcp[i] < n && s[l + lcp[i]] == s[r + lcp[i]]) lcp[i]++;
            }
            return lcp;
        }

        private static int[] Znaive(int[] s)
        {
            int n = s.Length;
            var z = new int[n];
            for (int i = 0; i < n; i++)
            {
                while (i + z[i] < n && s[z[i]] == s[i + z[i]]) z[i]++;
            }
            return z;
        }

        [Fact]
        public void Empty()
        {
            StringLib.SuffixArray("").ShouldBe([]);
            StringLib.SuffixArray(Array.Empty<int>()).ShouldBe([]);

            StringLib.ZAlgorithm("").ShouldBe([]);
            StringLib.ZAlgorithm(Array.Empty<int>()).ShouldBe([]);
        }
        [Fact]
        public void SaLcpNaive()
        {
            for (int n = 1; n <= 5; n++)
            {
                int m = 1;
                for (int i = 0; i < n; i++) m *= 4;
                for (int f = 0; f < m; f++)
                {
                    var s = new int[n];
                    int g = f;
                    int max_c = 0;
                    for (int i = 0; i < n; i++)
                    {
                        s[i] = g % 4;
                        max_c = Math.Max(max_c, s[i]);
                        g /= 4;
                    }
                    var sa = SaNaive(s);
                    StringLib.SuffixArray(s).ShouldBe(sa);
                    StringLib.SuffixArray(s, max_c).ShouldBe(sa);
                    StringLib.LcpArray(s, sa).ShouldBe(LcpNaive(s, sa));
                }
            }
            for (int n = 1; n <= 10; n++)
            {
                int m = 1;
                for (int i = 0; i < n; i++) m *= 2;
                for (int f = 0; f < m; f++)
                {
                    var s = new int[n];
                    int g = f;
                    int max_c = 0;
                    for (int i = 0; i < n; i++)
                    {
                        s[i] = g % 2;
                        max_c = Math.Max(max_c, s[i]);
                        g /= 2;
                    }
                    var sa = SaNaive(s);
                    StringLib.SuffixArray(s).ShouldBe(sa);
                    StringLib.SuffixArray(s, max_c).ShouldBe(sa);
                    StringLib.LcpArray(s, sa).ShouldBe(LcpNaive(s, sa));
                }
            }
        }
        [Fact]
        public void InternalSaNaiveNaive()
        {
            for (int n = 1; n <= 5; n++)
            {
                int m = 1;
                for (int i = 0; i < n; i++) m *= 4;
                for (int f = 0; f < m; f++)
                {
                    var s = new int[n];
                    int g = f;
                    int max_c = 0;
                    for (int i = 0; i < n; i++)
                    {
                        s[i] = g % 4;
                        max_c = Math.Max(max_c, s[i]);
                        g /= 4;
                    }
                    var sa = InternalString.SaNaive(s);
                    sa.ShouldBe(SaNaive(s));
                }
            }
            for (int n = 1; n <= 10; n++)
            {
                int m = 1;
                for (int i = 0; i < n; i++) m *= 2;
                for (int f = 0; f < m; f++)
                {
                    var s = new int[n];
                    int g = f;
                    for (int i = 0; i < n; i++)
                    {
                        s[i] = g % 2;
                        g /= 2;
                    }

                    var sa = InternalString.SaNaive(s);
                    sa.ShouldBe(SaNaive(s));
                }
            }
        }
        [Fact]
        public void InternalSaDoublingNaive()
        {
            for (int n = 1; n <= 5; n++)
            {
                int m = 1;
                for (int i = 0; i < n; i++) m *= 4;
                for (int f = 0; f < m; f++)
                {
                    var s = new int[n];
                    int g = f;
                    for (int i = 0; i < n; i++)
                    {
                        s[i] = g % 4;
                        g /= 4;
                    }

                    var sa = InternalString.SaDoubling(s);
                    sa.ShouldBe(SaNaive(s));
                }
            }
            for (int n = 1; n <= 10; n++)
            {
                int m = 1;
                for (int i = 0; i < n; i++) m *= 2;
                for (int f = 0; f < m; f++)
                {
                    var s = new int[n];
                    int g = f;
                    for (int i = 0; i < n; i++)
                    {
                        s[i] = g % 2;
                        g /= 2;
                    }

                    var sa = InternalString.SaDoubling(s);
                    sa.ShouldBe(SaNaive(s));
                }
            }
        }

        [Fact]
        public void InternalSaIsNaive()
        {
            for (int n = 1; n <= 5; n++)
            {
                int m = 1;
                for (int i = 0; i < n; i++) m *= 4;
                for (int f = 0; f < m; f++)
                {
                    var s = new int[n];
                    int g = f;
                    int max_c = 0;
                    for (int i = 0; i < n; i++)
                    {
                        s[i] = g % 4;
                        max_c = Math.Max(max_c, s[i]);
                        g /= 4;
                    }

                    var sa = InternalString.SaIs(s, max_c, -1, -1);
                    sa.ShouldBe(SaNaive(s));
                }
            }
            for (int n = 1; n <= 10; n++)
            {
                int m = 1;
                for (int i = 0; i < n; i++) m *= 2;
                for (int f = 0; f < m; f++)
                {
                    var s = new int[n];
                    int g = f;
                    int max_c = 0;
                    for (int i = 0; i < n; i++)
                    {
                        s[i] = g % 2;
                        max_c = Math.Max(max_c, s[i]);
                        g /= 2;
                    }

                    var sa = InternalString.SaIs(s, max_c, -1, -1);
                    sa.ShouldBe(SaNaive(s));
                }
            }
        }
        [Fact]
        public void SAAllATest()
        {
            for (int n = 1; n <= 100; n++)
            {
                var s = Enumerable.Repeat(10, n).ToArray();
                var sa = SaNaive(s);
                StringLib.SuffixArray(s).ShouldBe(sa);
                StringLib.SuffixArray(s, 10).ShouldBe(sa);
                StringLib.SuffixArray(s, 12).ShouldBe(sa);
            }
        }
        [Fact]
        public void SAAllABTest()
        {
            for (int n = 1; n <= 100; n++)
            {
                var s = new int[n];
                for (int i = 0; i < n; i++) s[i] = (i % 2);
                var sa = SaNaive(s);
                StringLib.SuffixArray(s).ShouldBe(sa);
                StringLib.SuffixArray(s, 3).ShouldBe(sa);
            }
            for (int n = 1; n <= 100; n++)
            {
                var s = new int[n];
                for (int i = 0; i < n; i++) s[i] = 1 - (i % 2);
                var sa = SaNaive(s);
                StringLib.SuffixArray(s).ShouldBe(sa);
                StringLib.SuffixArray(s, 3).ShouldBe(sa);
            }
        }
        [Fact]
        public void SuffixArray()
        {
            var s = "missisippi";

            var sa = StringLib.SuffixArray(s);

            var answer = new[] {
                "i",           // 9
                "ippi",        // 6
                "isippi",      // 4
                "issisippi",   // 1
                "missisippi",  // 0
                "pi",          // 8
                "ppi",         // 7
                "sippi",       // 5
                "sisippi",     // 3
                "ssisippi",    // 2
            };

            sa.Length.ShouldBe(answer.Length);
            for (int i = 0; i < sa.Length; i++)
            {
                s[sa[i]..].ShouldBe(answer[i]);
            }
        }
        [Fact]
        public void SuffixArrayInt()
        {
            const int size = Global.IsCi ? 500000 : 50000;

            int[] s, sa;
            s = Enumerable.Range(0, size).SelectMany(i => new[] { i, ~i }).ToArray();
            sa = StringLib.SuffixArray(s);
            sa.ShouldBe(
                Enumerable.Range(0, size).Select(i => 2 * size - 1 - 2 * i).Concat(
                    Enumerable.Range(0, size).Select(i => 2 * i))
                );

            s = Enumerable.Range(0, 2 * size).ToArray();
            sa = StringLib.SuffixArray(s);
            sa.ShouldBe(s);

            s = Enumerable.Range(0, 2 * size).Reverse().ToArray();
            sa = StringLib.SuffixArray(s);
            sa.ShouldBe(s);
        }

        [Fact]
        public void SASingle()
        {
            StringLib.SuffixArray([0]).ShouldBe([0]);
            StringLib.SuffixArray([-1]).ShouldBe([0]);
            StringLib.SuffixArray([1]).ShouldBe([0]);
            StringLib.SuffixArray([int.MinValue]).ShouldBe([0]);
            StringLib.SuffixArray([int.MaxValue]).ShouldBe([0]);
        }

        [Fact]
        public void LCP()
        {
            var s = "aab";
            var sa = StringLib.SuffixArray(s);
            sa.ShouldBe([0, 1, 2]);
            var lcp = StringLib.LcpArray(s, sa);
            lcp.ShouldBe([1, 0]);
            StringLib.LcpArray([0, 0, 1], sa).ShouldBe(lcp);
            StringLib.LcpArray([-100, -100, 100], sa).ShouldBe(lcp);
            StringLib.LcpArray([int.MinValue, int.MinValue, int.MaxValue], sa).ShouldBe(lcp);
            StringLib.LcpArray([long.MinValue, long.MinValue, long.MaxValue], sa).ShouldBe(lcp);
            StringLib.LcpArray([uint.MinValue, uint.MinValue, uint.MaxValue], sa).ShouldBe(lcp);
            StringLib.LcpArray([ulong.MinValue, ulong.MinValue, ulong.MaxValue], sa).ShouldBe(lcp);
        }

        [Fact]
        public void ZAlgo()
        {
            var s = "abab";
            var z = StringLib.ZAlgorithm(s);
            z.ShouldBe([4, 0, 2, 0]);
            StringLib.ZAlgorithm([1, 10, 1, 10]).ShouldBe([4, 0, 2, 0]);
            StringLib.ZAlgorithm([0, 0, 0, 0, 0, 0, 0]).ShouldBe(Znaive([0, 0, 0, 0, 0, 0, 0]));
        }
        [Fact]
        public void ZNaive()
        {
            for (int n = 1; n <= 6; n++)
            {
                int m = 1;
                for (int i = 0; i < n; i++) m *= 4;
                for (int f = 0; f < m; f++)
                {
                    var s = new int[n];
                    int g = f;
                    for (int i = 0; i < n; i++)
                    {
                        s[i] = g % 4;
                        g /= 4;
                    }
                    StringLib.ZAlgorithm(s).ShouldBe(Znaive(s));
                }
            }
            for (int n = 1; n <= 10; n++)
            {
                int m = 1;
                for (int i = 0; i < n; i++) m *= 2;
                for (int f = 0; f < m; f++)
                {
                    var s = new int[n];
                    int g = f;
                    for (int i = 0; i < n; i++)
                    {
                        s[i] = g % 2;
                        g /= 2;
                    }
                    StringLib.ZAlgorithm(s).ShouldBe(Znaive(s));
                }
            }
        }
    }
}
