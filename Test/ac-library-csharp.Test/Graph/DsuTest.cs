using System.IO;
using System.Linq;
using Shouldly;
using Xunit;

namespace AtCoder
{
    public class DsuTest
    {
        [Fact]
        public void Zero()
        {
            var uf = new Dsu(0);
            uf.Groups().ShouldBe([]);
        }

        [Fact]
        public void Simple()
        {
            var uf = new Dsu(2);
            uf.Same(0, 1).ShouldBeFalse();
            int x = uf.Merge(0, 1);
            uf.Leader(0).ShouldBe(x);
            uf.Leader(1).ShouldBe(x);
            uf.Same(0, 1).ShouldBeTrue();
            uf.Size(0).ShouldBe(2);
        }

        [Fact]
        public void Line()
        {
            int n = 500000;
            var uf = new Dsu(n);
            for (int i = 0; i < n - 1; i++)
            {
                uf.Merge(i, i + 1);
            }
            uf.Size(0).ShouldBe(n);
            uf.Groups().Length.ShouldBe(1);
        }

        [Fact]
        public void LineReverse()
        {
            int n = 500000;
            var uf = new Dsu(n);
            for (int i = n - 2; i >= 0; i--)
            {
                uf.Merge(i, i + 1);
            }
            uf.Size(0).ShouldBe(n);
            uf.Groups().Length.ShouldBe(1);
        }

        [Theory]
        [Trait("Category", "Practice")]
        [InlineData(
// Based on Library Checker https://github.com/yosupo06/library-checker-problems
// unionfind
// Apache License 2.0
@"4 7
1 0 1
0 0 1
0 2 3
1 0 1
1 1 2
0 0 2
1 1 3
",

@"0
1
0
1
"
)]
        public void Practice(string input, string expected)
            => new SolverRunner(Solver).Solve(input).ShouldEqualLines(expected);
        static void Solver(TextReader reader, TextWriter writer)
        {
            int[] nq = reader.ReadLine().Split().Select(int.Parse).ToArray();
            (int n, int q) = (nq[0], nq[1]);
            var dsu = new Dsu(n);
            for (int i = 0; i < q; i++)
            {
                int[] tuv = reader.ReadLine().Split().Select(int.Parse).ToArray();
                (int t, int u, int v) = (tuv[0], tuv[1], tuv[2]);
                if (t == 0)
                {
                    dsu.Merge(u, v);
                }
                else
                {
                    if (dsu.Same(u, v)) writer.WriteLine("1");
                    else writer.WriteLine("0");
                }
            }
        }
    }
}
