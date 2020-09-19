#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
            Assert.Equal("$", s.AllProd);
        }

        [Fact]
        public void Invalid()
        {
            Assert.Throws<DebuAssertException>(() => new Segtree<string, MonoidOperator>(-1));
            var s = new Segtree<string, MonoidOperator>(10);
            Assert.Throws<DebuAssertException>(() => s[-1]);
            Assert.Throws<DebuAssertException>(() => s[10]);

            Assert.Throws<DebuAssertException>(() => s.Prod(-1, -1));
            Assert.Throws<DebuAssertException>(() => s.Prod(3, 2));
            Assert.Throws<DebuAssertException>(() => s.Prod(0, 11));
            Assert.Throws<DebuAssertException>(() => s.Prod(-1, 11));

            Assert.Throws<DebuAssertException>(() => s.MaxRight(11, s => true));
            Assert.Throws<DebuAssertException>(() => s.MaxRight(-1, s => true));
            Assert.Throws<DebuAssertException>(() => s.MaxRight(0, s => false));
        }

        [Fact]
        public void One()
        {
            var s = new Segtree<string, MonoidOperator>(1);
            Assert.Equal("$", s.AllProd);
            Assert.Equal("$", s[0]);
            Assert.Equal("$", s.Prod(0, 1));
            s[0] = "dummy";
            Assert.Equal("dummy", s[0]);
            Assert.Equal("$", s.Prod(0, 0));
            Assert.Equal("dummy", s.Prod(0, 1));
            Assert.Equal("$", s.Prod(1, 1));
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
                        Assert.Equal(seg0.Prod(l, r), seg1.Prod(l, r));
                    }
                }

                for (int l = 0; l <= n; l++)
                {
                    for (int r = l; r <= n; r++)
                    {
                        var y = seg1.Prod(l, r);
                        Assert.Equal(seg0.MaxRight(l, x => x.Length <= y.Length), seg1.MaxRight(l, x => x.Length <= y.Length));
                    }
                }


                for (int r = 0; r <= n; r++)
                {
                    for (int l = 0; l <= r; l++)
                    {
                        var y = seg1.Prod(l, r);
                        Assert.Equal(seg0.MinLeft(l, x => x.Length <= y.Length), seg1.MinLeft(l, x => x.Length <= y.Length));
                    }
                }
            }
        }
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
            Assert.True(f(sum));
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
            Assert.True(f(sum));
            for (int i = r - 1; i >= 0; i--)
            {
                sum = op.Operate(d[i], sum);
                if (!f(sum)) return i + 1;
            }
            return 0;
        }
    }
}
