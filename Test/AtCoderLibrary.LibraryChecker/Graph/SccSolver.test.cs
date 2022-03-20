using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.Graph
{
    public class SccSolver
    {
        static void Main() { using var cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/scc
        static void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
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
                cw.Write(v.Length);
                cw.Write(' ');
                cw.WriteLineJoin(v);
            }
        }
    }
}
