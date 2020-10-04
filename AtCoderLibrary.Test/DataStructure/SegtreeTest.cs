using System;
using System.Collections.Generic;
using FluentAssertions;
using AtCoder.Test.Utils;
using Xunit;

namespace AtCoder.Test.DataStructure
{
    public class SegtreeTest : TestWithDebugAssert
    {
        [Fact]
        public void Zero()
        {
            var s = new Segtree<string, MonoidOperator>(0);
            s.AllProd.Should().Be("$");
        }

        [SkippableFact]
        public void Invalid()
        {
            DebugUtil.SkipIfNotDebug();
            Assert.Throws<DebugAssertException>(() => new Segtree<string, MonoidOperator>(-1));
            var s = new Segtree<string, MonoidOperator>(10);
            s.Invoking(s => s[-1]).Should().Throw<DebugAssertException>();
            s.Invoking(s => s[10]).Should().Throw<DebugAssertException>();

            s.Invoking(s => s.Prod(-1, -1)).Should().Throw<DebugAssertException>();
            s.Invoking(s => s.Prod(3, 2)).Should().Throw<DebugAssertException>();
            s.Invoking(s => s.Prod(0, 11)).Should().Throw<DebugAssertException>();
            s.Invoking(s => s.Prod(-1, 11)).Should().Throw<DebugAssertException>();

            s.Invoking(s => s.MaxRight(11, s => true)).Should().Throw<DebugAssertException>();
            s.Invoking(s => s.MaxRight(-1, s => true)).Should().Throw<DebugAssertException>();
            s.Invoking(s => s.MaxRight(0, s => false)).Should().Throw<DebugAssertException>();
        }

        [Fact]
        public void One()
        {
            var s = new Segtree<string, MonoidOperator>(1);
            s.AllProd.Should().Be("$");
            s[0].Should().Be("$");
            s.Prod(0, 1).Should().Be("$");
            s[0] = "dummy";
            s[0].Should().Be("dummy");
            s.Prod(0, 0).Should().Be("$");
            s.Prod(0, 1).Should().Be("dummy");
            s.Prod(1, 1).Should().Be("$");
        }

        [Fact]
        public void CompareNaive()
        {
            for (int n = 0; n < 30; n++)
            {
                var seg0 = new SegtreeNaive(n);
                var seg1 = new Segtree<string, MonoidOperator>(n);

                for (int i = 0; i < n; i++)
                {
                    var s = "";
                    s += (char)('a' + i);
                    seg0[i] = s;
                    seg1[i] = s;
                }

                for (int l = 0; l <= n; l++)
                {
                    for (int r = l; r <= n; r++)
                    {
                        seg1.Prod(l, r).Should().Be(seg0.Prod(l, r));
                    }
                }

                for (int l = 0; l <= n; l++)
                {
                    for (int r = l; r <= n; r++)
                    {
                        var y = seg1.Prod(l, r);
                        seg1.MaxRight(l, x => x.Length <= y.Length).Should().Be(seg0.MaxRight(l, x => x.Length <= y.Length));
                    }
                }


                for (int r = 0; r <= n; r++)
                {
                    for (int l = 0; l <= r; l++)
                    {
                        var y = seg1.Prod(l, r);
                        seg1.MinLeft(l, x => x.Length <= y.Length).Should().Be(seg0.MinLeft(l, x => x.Length <= y.Length));
                    }
                }
            }
        }


        [Fact]
        // https://atcoder.jp/contests/practice2/tasks/practice2_j
        public void Practice2_J()
        {
            int target;
            // int n = 5;
            int q = 5;
            var a = new int[5] { 1, 2, 3, 2, 1 };
            var queries = new[]
            {
                new[]{2,1,5},
                new[]{3,2,3},
                new[]{1,3,1},
                new[]{2,2,4},
                new[]{3,1,3},
            };

            var seg = new Segtree<int, OpJ>(a);

            var list = new List<int>();

            for (int i = 0; i < q; i++)
            {
                int t = queries[i][0];
                if (t == 1)
                {
                    int x = queries[i][1];
                    int v = queries[i][2];
                    x--;
                    seg[x] = v;
                }
                else if (t == 2)
                {
                    int l = queries[i][1];
                    int r = queries[i][2];
                    l--;
                    list.Add(seg.Prod(l, r));
                }
                else if (t == 3)
                {
                    int p = queries[i][1];
                    target = queries[i][2];
                    p--;
                    list.Add(seg.MaxRight(p, v => v < target) + 1);
                }
            }
            list.Should().Equal(new[] { 3, 3, 2, 6 });
        }
    }

    struct OpJ : IMonoidOperator<int>
    {
        public int Identity => -1;
        public int Operate(int a, int b) => Math.Max(a, b);
    }

    struct MonoidOperator : IMonoidOperator<string>
    {
        public string Identity => "$";
        public string Operate(string a, string b)
        {
            if (!(a == "$" || b == "$" || StringComparer.Ordinal.Compare(a, b) <= 0)) throw new Exception();
            if (a == "$") return b;
            if (b == "$") return a;
            return a + b;
        }
    }

    class SegtreeNaive
    {
        private static readonly MonoidOperator op = default;
        int n;
        string[] d;

        public SegtreeNaive(int _n)
        {
            n = _n;
            d = new string[n];
            Array.Fill(d, op.Identity);
        }
        public string this[int p]
        {
            set => d[p] = value;
            get => d[p];
        }

        public string Prod(int l, int r)
        {
            var sum = op.Identity;
            for (int i = l; i < r; i++)
            {
                sum = op.Operate(sum, d[i]);
            }
            return sum;
        }
        public string AllProd => Prod(0, n);
        public int MaxRight(int l, Predicate<string> f)
        {
            var sum = op.Identity;
            f(sum).Should().BeTrue();
            for (int i = l; i < n; i++)
            {
                sum = op.Operate(sum, d[i]);
                if (!f(sum)) return i;
            }
            return n;
        }
        public int MinLeft(int r, Predicate<string> f)
        {
            var sum = op.Identity;
            f(sum).Should().BeTrue();
            for (int i = r - 1; i >= 0; i--)
            {
                sum = op.Operate(d[i], sum);
                if (!f(sum)) return i + 1;
            }
            return 0;
        }
    }
}
