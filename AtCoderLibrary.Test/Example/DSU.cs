using System;
using System.IO;
using System.Linq;
using AtCoder.Test.Utils;
using Xunit;

namespace AtCoder.Test.Example
{
    public class DSUTest
    {
        [ProblemTestCase(@"https://judge.yosupo.jp/problem/unionfind")]
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
}
