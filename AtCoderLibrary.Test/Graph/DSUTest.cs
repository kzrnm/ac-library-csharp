using System.ComponentModel;
using System.IO;
using System.Linq;
using AtCoder.Test.Utils;
using FluentAssertions;
using Xunit;

namespace AtCoder.Test.DataStructure
{
    public class DSUTest : TestWithDebugAssert
    {


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
            => new SolverRunner(Solver).Solve(input).Should().EqualLines(expected);
        static void Solver(TextReader reader, TextWriter writer)
        {
            int[] nq = reader.ReadLine().Split().Select(int.Parse).ToArray();
            (int n, int q) = (nq[0], nq[1]);
            var dsu = new DSU(n);
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
