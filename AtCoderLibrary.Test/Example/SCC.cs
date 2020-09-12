using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AtCoder.Test
{
    public class SCCTest
    {
        [Test]
        public string Practice(string testCase)
        {
            var reader = new StringReader(testCase);
            var writer = new StringWriter();

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

            return writer.ToString();
        }
    }
}
