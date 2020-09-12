using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AtCoder.Test
{
    public class FloorSumTest
    {
        [Test]
        public string Practice(string testCase)
        {
            var reader = new StringReader(testCase);
            var writer = new StringWriter();
            int t = int.Parse(reader.ReadLine());
            for (int i = 0; i < t; i++)
            {
                long[] query = reader.ReadLine().Split().Select(long.Parse).ToArray();
                (long n, long m, long a, long b) = (query[0], query[1], query[2], query[3]);
                writer.WriteLine(Math.FloorSum(n, m, a, b));
            }
            return writer.ToString();
        }
    }
}
