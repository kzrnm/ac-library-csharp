using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.Graph
{
    internal class SccTest : BaseSover
    {
        public override string Url => "https://judge.yosupo.jp/problem/scc";
        public override void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
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

            var scc = g.Scc();

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
