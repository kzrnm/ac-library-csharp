using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AtCoder
{
    public class FenwickTreeTest : TestWithDebugAssert
    {


        [Theory]
        [Trait("Category", "Practice")]
        [InlineData(
// Based on Library Checker https://github.com/yosupo06/library-checker-problems
// Point Add Range Sum
// Apache License 2.0
@"5 5
1 2 3 4 5
1 0 5
1 2 4
0 3 10
1 0 5
1 0 3
",

@"15
7
25
6
"
)]
        public void Practice(string input, string expected)
            => new SolverRunner(Solver).Solve(input).Should().EqualLines(expected);
        static void Solver(TextReader reader, TextWriter writer)
        {
            int[] nq = reader.ReadLine().Split().Select(int.Parse).ToArray();
            (int n, int q) = (nq[0], nq[1]);

            var fw = new LongFenwickTree(n);
            int[] a = reader.ReadLine().Split().Select(int.Parse).ToArray();
            for (int i = 0; i < a.Length; i++)
            {
                fw.Add(i, a[i]);
            }
            for (int i = 0; i < q; i++)
            {
                int[] query = reader.ReadLine().Split().Select(int.Parse).ToArray();
                int t = query[0];
                if (t == 0)
                {
                    (int p, int x) = (query[1], query[2]);
                    fw.Add(p, x);
                }
                else
                {
                    (int l, int r) = (query[1], query[2]);
                    writer.WriteLine(fw.Sum(l, r));
                }
            }
        }
    }
}
