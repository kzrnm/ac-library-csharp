using System.Collections.Generic;
using Kzrnm.Competitive.IO;

namespace AtCoder
{
    public class SccSolver : ISolver
    {
        public string Name => "scc";

        public void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int n = cr;
            int m = cr;

            var g = new SCCGraph(n);

            for (int i = 0; i < m; i++)
            {
                int u = cr;
                int v = cr;
                g.AddEdge(u, v);
            }

            List<List<int>> scc = g.SCC();

            cw.WriteLine(scc.Count);
            foreach (List<int> v in scc)
            {
                cw.StreamWriter.Write(v.Count);
                cw.StreamWriter.Write(' ');
                cw.WriteLineJoin(v);
            }
        }
    }
}
