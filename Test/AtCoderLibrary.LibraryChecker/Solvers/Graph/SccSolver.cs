﻿using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.Graph
{
    public class SccSolver : Solver
    {
        public override string Name => "scc";
        public override double TimeoutSecond => 5;
        public override void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int n = cr;
            int m = cr;

            var g = new SccGraph(n);

            for (int i = 0; i < m; i++)
            {
                int u = cr;
                int v = cr;
                g.AddEdge(u, v);
            }

            var scc = g.SCC();

            cw.WriteLine(scc.Length);
            foreach (var v in scc)
            {
                cw.StreamWriter.Write(v.Length);
                cw.StreamWriter.Write(' ');
                cw.WriteLineJoin(v);
            }
        }
    }
}
