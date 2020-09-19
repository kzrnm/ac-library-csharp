using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtCoder.Test.Utils;
using Xunit;

namespace AtCoder.Test.Example
{
    public class SCCTest
    {
        [ProblemTestCase(@"https://judge.yosupo.jp/problem/scc")]
        public void Practice(string input, string answer)
        {
            var reader = new StringReader(input);
            var writer = new StringWriter();

            Solver(reader, writer);

            Assert.True(new AlwaysFailValidator().IsValid(input, answer, writer.ToString()));

            static void Solver(TextReader reader, TextWriter writer)
            {
                int[] nm = reader.ReadLine().Split().Select(int.Parse).ToArray();
                (int n, int m) = (nm[0], nm[1]);

                var g = new SCCGraph(n);

                for (int i = 0; i < m; i++)
                {
                    int[] uv = reader.ReadLine().Split().Select(int.Parse).ToArray();
                    (int u, int v) = (uv[0], uv[1]);
                    g.AddEdge(u, v);
                }

                List<List<int>> scc = g.SCC();

                writer.WriteLine(scc.Count);
                foreach (List<int> v in scc)
                {
                    writer.WriteLine(string.Join(" ", v));
                }
            }
        }
    }
}
