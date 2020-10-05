using System.ComponentModel;
using System.IO;
using System.Linq;
using AtCoder.Test.Utils;
using FluentAssertions;
using Xunit;

namespace AtCoder.Test.DataStructure
{
    public class FloorSumTest : TestWithDebugAssert
    {


        [Theory]
        [Trait("Category", "Practice")]
        [InlineData(
// Based on Library Checker https://github.com/yosupo06/library-checker-problems
// Sum of Floor of Linear
// Apache License 2.0
@"5
4 10 6 3
6 5 4 3
1 1 0 0
31415 92653 58979 32384
1000000000 1000000000 999999999 999999999
",

@"3
13
0
314095480
499999999500000000
"
)]
        public void Practice(string input, string expected)
            => new SolverRunner(Solver).Solve(input).Should().EqualLines(expected);
        static void Solver(TextReader reader, TextWriter writer)
        {
            int t = int.Parse(reader.ReadLine());
            for (int i = 0; i < t; i++)
            {
                long[] query = reader.ReadLine().Split().Select(long.Parse).ToArray();
                (long n, long m, long a, long b) = (query[0], query[1], query[2], query[3]);
                writer.WriteLine(MathLib.FloorSum(n, m, a, b));
            }
        }
    }
}
