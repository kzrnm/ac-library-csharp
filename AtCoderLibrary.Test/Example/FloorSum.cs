using System;
using System.IO;
using System.Linq;
using AtCoder.Test.Utils;
using Xunit;

namespace AtCoder.Test.Example
{
    public class FloorSumTest
    {
        [ProblemTestCase(@"https://judge.yosupo.jp/problem/sum_of_floor_of_linear")]
        public void Practice(string input, string answer)
        {
            var reader = new StringReader(input);
            var writer = new StringWriter();

            Solver(reader, writer);

            Assert.True(new TokenEqualityValidator().IsValid(input, answer, writer.ToString()));

            static void Solver(TextReader reader, TextWriter writer)
            {
                int t = int.Parse(reader.ReadLine());
                for (int i = 0; i < t; i++)
                {
                    long[] query = reader.ReadLine().Split().Select(long.Parse).ToArray();
                    (long n, long m, long a, long b) = (query[0], query[1], query[2], query[3]);
                    writer.WriteLine(Math.FloorSum(n, m, a, b));
                }
            }
        }
    }
}
