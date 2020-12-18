using System;
using System.Collections.Generic;
using System.Linq;
using AtCoder.Internal;
using FluentAssertions;
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
        private static int[] SaNaive(IList<int> s)
        {
            int n = s.Count;
            var sa = Enumerable.Range(0, n).ToArray();
            Array.Sort(sa, (l, r) => Compare(s.Skip(l), s.Skip(r)));
            return sa;
        }
        private static int[] LcpNaive(IList<int> s, IList<int> sa)
        {
            int n = s.Count;
            n.Should().NotBe(0);
            var lcp = new int[n - 1];
            for (int i = 0; i < n - 1; i++)
            {
                int l = sa[i], r = sa[i + 1];
                while (l + lcp[i] < n && r + lcp[i] < n && s[l + lcp[i]] == s[r + lcp[i]]) lcp[i]++;
            }
            return lcp;
        }

        private static int[] Znaive(IList<int> s)
        {
            int n = s.Count;
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
            StringLib.SuffixArray("").Should().Equal();
            StringLib.SuffixArray(Array.Empty<int>()).Should().Equal();

            StringLib.ZAlgorithm("").Should().Equal();
            StringLib.ZAlgorithm(Array.Empty<int>()).Should().Equal();
        }
        [Fact]
        public void SALCPNaive()
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
                    StringLib.SuffixArray(s).Should().Equal(sa);
                    StringLib.SuffixArray(s, max_c).Should().Equal(sa);
                    StringLib.LCPArray(s, sa).Should().Equal(LcpNaive(s, sa));
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
                    StringLib.SuffixArray(s).Should().Equal(sa);
                    StringLib.SuffixArray(s, max_c).Should().Equal(sa);
                    StringLib.LCPArray(s, sa).Should().Equal(LcpNaive(s, sa));
                }
            }
        }
        [Fact]
        public void InternalSANaiveNaive()
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
                    var sa = InternalString.SANaive(s);
                    sa.Should().Equal(SaNaive(s));
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

                    var sa = InternalString.SANaive(s);
                    sa.Should().Equal(SaNaive(s));
                }
            }
        }
        [Fact]
        public void InternalSADoublingNaive()
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

                    var sa = InternalString.SADoubling(s);
                    sa.Should().Equal(SaNaive(s));
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

                    var sa = InternalString.SADoubling(s);
                    sa.Should().Equal(SaNaive(s));
                }
            }
        }

        [Fact]
        public void InternalSAISNaive()
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

                    var sa = InternalString.SAIS(s, max_c, -1, -1);
                    sa.Should().Equal(SaNaive(s));
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

                    var sa = InternalString.SAIS(s, max_c, -1, -1);
                    sa.Should().Equal(SaNaive(s));
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
                StringLib.SuffixArray(s).Should().Equal(sa);
                StringLib.SuffixArray(s, 10).Should().Equal(sa);
                StringLib.SuffixArray(s, 12).Should().Equal(sa);
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
                StringLib.SuffixArray(s).Should().Equal(sa);
                StringLib.SuffixArray(s, 3).Should().Equal(sa);
            }
            for (int n = 1; n <= 100; n++)
            {
                var s = new int[n];
                for (int i = 0; i < n; i++) s[i] = 1 - (i % 2);
                var sa = SaNaive(s);
                StringLib.SuffixArray(s).Should().Equal(sa);
                StringLib.SuffixArray(s, 3).Should().Equal(sa);
            }
        }
        [Fact]
        public void SA()
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

            sa.Should().HaveCount(answer.Length);
            for (int i = 0; i < sa.Length; i++)
            {
                s[sa[i]..].Should().Be(answer[i]);
            }
        }
        [Fact]
        public void SASingle()
        {
            StringLib.SuffixArray(new[] { 0 }).Should().Equal(new[] { 0 });
            StringLib.SuffixArray(new[] { -1 }).Should().Equal(new[] { 0 });
            StringLib.SuffixArray(new[] { 1 }).Should().Equal(new[] { 0 });
            StringLib.SuffixArray(new[] { int.MinValue }).Should().Equal(new[] { 0 });
            StringLib.SuffixArray(new[] { int.MaxValue }).Should().Equal(new[] { 0 });
        }

        [Fact]
        public void LCP()
        {
            var s = "aab";
            var sa = StringLib.SuffixArray(s);
            sa.Should().Equal(new int[] { 0, 1, 2 });
            var lcp = StringLib.LCPArray(s, sa);
            lcp.Should().Equal(new int[] { 1, 0 });
            StringLib.LCPArray(new[] { 0, 0, 1 }, sa).Should().Equal(lcp);
            StringLib.LCPArray(new[] { -100, -100, 100 }, sa).Should().Equal(lcp);
            StringLib.LCPArray(new[] { int.MinValue, int.MinValue, int.MaxValue }, sa).Should().Equal(lcp);
            StringLib.LCPArray(new[] { long.MinValue, long.MinValue, long.MaxValue }, sa).Should().Equal(lcp);
            StringLib.LCPArray(new[] { uint.MinValue, uint.MinValue, uint.MaxValue }, sa).Should().Equal(lcp);
            StringLib.LCPArray(new[] { ulong.MinValue, ulong.MinValue, ulong.MaxValue }, sa).Should().Equal(lcp);
        }

        [Fact]
        public void ZAlgo()
        {
            var s = "abab";
            var z = StringLib.ZAlgorithm(s);
            z.Should().Equal(4, 0, 2, 0);
            StringLib.ZAlgorithm(new[] { 1, 10, 1, 10 }).Should().Equal(4, 0, 2, 0);
            StringLib.ZAlgorithm(new[] { 0, 0, 0, 0, 0, 0, 0 }).Should().Equal(Znaive(new[] { 0, 0, 0, 0, 0, 0, 0 }));
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
                    StringLib.ZAlgorithm(s).Should().Equal(Znaive(s));
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
                    StringLib.ZAlgorithm(s).Should().Equal(Znaive(s));
                }
            }
        }
    }
}
