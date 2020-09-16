using System;
using System.IO;
using System.Linq;
using AtCoder.Test.Utils;
using Xunit;

namespace AtCoder.Test.Example
{
    public class FenwickTreeTest
    {
        [ProblemTestCase(@"https://judge.yosupo.jp/problem/point_add_range_sum")]
        public void Practice(string input, string answer)
        {
            var reader = new StringReader(input);
            var writer = new StringWriter();

            Solver(reader, writer);

            Assert.True(new TokenEqualityValidator().IsValid(input, answer, writer.ToString()));

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
}
