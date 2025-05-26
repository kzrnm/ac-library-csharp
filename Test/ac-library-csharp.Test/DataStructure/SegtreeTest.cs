using System;
using System.IO;
using System.Linq;
using AtCoder.DataStructure.Native;
using AtCoder.Internal;
using Shouldly;
using Xunit;

namespace AtCoder
{
    public class SegtreeTest
    {
        [Fact]
        public void Zero()
        {
            var s = new Segtree<string, MonoidOperator>(0);
            s.AllProd.ShouldBe("$");
        }

        [Fact]
        public void Invalid()
        {
            var s = new Segtree<string, MonoidOperator>(10);
            new Action(() => _ = s[-1]).ShouldThrow<ContractAssertException>();
            new Action(() => _ = s[10]).ShouldThrow<ContractAssertException>();
            new Action(() => _ = s[0]).ShouldNotThrow();
            new Action(() => _ = s[9]).ShouldNotThrow();

            new Action(() => s.Prod(-1, -1)).ShouldThrow<ContractAssertException>();
            new Action(() => s.Prod(3, 2)).ShouldThrow<ContractAssertException>();
            new Action(() => s.Prod(0, 11)).ShouldThrow<ContractAssertException>();
            new Action(() => s.Prod(-1, 11)).ShouldThrow<ContractAssertException>();
            new Action(() => s.Prod(0, 0)).ShouldNotThrow();
            new Action(() => s.Prod(10, 10)).ShouldNotThrow();
            new Action(() => s.Prod(0, 10)).ShouldNotThrow();

            new Action(() => s.MaxRight(11, s => true)).ShouldThrow<ContractAssertException>();
            new Action(() => s.MaxRight(-1, s => true)).ShouldThrow<ContractAssertException>();
            new Action(() => s.MaxRight(0, s => false)).ShouldThrow<ContractAssertException>();
            new Action(() => s.MaxRight(0, s => true)).ShouldNotThrow();
            new Action(() => s.MaxRight(10, s => true)).ShouldNotThrow();

            new Action(() => s.MinLeft(11, s => true)).ShouldThrow<ContractAssertException>();
            new Action(() => s.MinLeft(-1, s => true)).ShouldThrow<ContractAssertException>();
            new Action(() => s.MinLeft(0, s => false)).ShouldThrow<ContractAssertException>();
            new Action(() => s.MinLeft(0, s => true)).ShouldNotThrow();
            new Action(() => s.MinLeft(10, s => true)).ShouldNotThrow();
        }

        [Fact]
        public void One()
        {
            var s = new Segtree<string, MonoidOperator>(1);
            s.AllProd.ShouldBe("$");
            s[0].ShouldBe("$");
            s.Prod(0, 1).ShouldBe("$");
            s[0..1].ShouldBe("$");
            s[0] = "dummy";
            s[0].ShouldBe("dummy");
            s.Prod(0, 0).ShouldBe("$");
            s[0..0].ShouldBe("$");
            s.Prod(0, 1).ShouldBe("dummy");
            s[0..1].ShouldBe("dummy");
            s.Prod(1, 1).ShouldBe("$");
            s[1..1].ShouldBe("$");
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
                        seg1.Prod(l, r).ShouldBe(seg0.Prod(l, r));
                        seg1[l..r].ShouldBe(seg0.Prod(l, r));
                    }
                }

                for (int l = 0; l <= n; l++)
                {
                    for (int r = l; r <= n; r++)
                    {
                        var y = seg1.Prod(l, r);
                        seg1.MaxRight(l, x => x.Length <= y.Length).ShouldBe(seg0.MaxRight(l, x => x.Length <= y.Length));
                    }
                }


                for (int r = 0; r <= n; r++)
                {
                    for (int l = 0; l <= r; l++)
                    {
                        var y = seg1.Prod(l, r);
                        seg1.MinLeft(l, x => x.Length <= y.Length).ShouldBe(seg0.MinLeft(l, x => x.Length <= y.Length));
                    }
                }
            }
        }

        [Theory]
        [Trait("Category", "Practice")]
        [InlineData(
// Based on https://atcoder.jp/contests/practice2/tasks/practice2_j
@"5 5
1 2 3 2 1
2 1 5
3 2 3
1 3 1
2 2 4
3 1 3",

@"3
3
2
6
"
)]
        public void Practice(string input, string expected)
            => new SolverRunner(Solver).Solve(input).ShouldEqualLines(expected);
        static void Solver(TextReader reader, TextWriter writer)
        {
            int[] nq = reader.ReadLine().Split().Select(int.Parse).ToArray();
            (int n, int q) = (nq[0], nq[1]);
            int[] a = reader.ReadLine().Split().Select(int.Parse).ToArray();

            var seg = new Segtree<int, OpPractice>(a);
            int target;

            for (int i = 0; i < q; i++)
            {
                int[] tuv = reader.ReadLine().Split().Select(int.Parse).ToArray();

                int t = tuv[0];
                if (t == 1)
                {
                    int x = tuv[1];
                    int v = tuv[2];
                    x--;
                    seg[x] = v;
                }
                else if (t == 2)
                {
                    int l = tuv[1];
                    int r = tuv[2];
                    l--;
                    writer.WriteLine(seg.Prod(l, r));
                }
                else if (t == 3)
                {
                    int p = tuv[1];
                    target = tuv[2];
                    p--;
                    writer.WriteLine(seg.MaxRight(p, v => v < target) + 1);
                }
            }
        }
        readonly struct OpPractice : ISegtreeOperator<int>
        {
            public int Identity => -1;
            public int Operate(int a, int b) => Math.Max(a, b);
        }
    }
}
