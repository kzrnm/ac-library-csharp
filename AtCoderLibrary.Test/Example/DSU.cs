using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AtCoder.Test
{
    public class DSUTest
    {
        [Test]
        public string Practice(string testCase)
        {
            var reader = new StringReader(testCase);
            var writer = new StringWriter();
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
            return writer.ToString();
        }
    }
}
