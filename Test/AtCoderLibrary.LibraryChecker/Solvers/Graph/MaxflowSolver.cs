using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.Graph
{
    public class MaxflowSolver : ISolver
    {
        public string Name => "bipartitematching";

        public void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int l = cr;
            int r = cr;
            int m = cr;
            var mf = new MfGraphInt(l + r + 2);
            for (int i = 0; i < m; i++)
            {
                int a = cr;
                int b = cr;
                mf.AddEdge(a, l + b, 1);
            }
            for (int i = 0; i < l; i++)
            {
                mf.AddEdge(l + r, i, 1);
            }
            for (int i = 0; i < r; i++)
            {
                mf.AddEdge(l + i, l + r + 1, 1);
            }
            cw.WriteLine(mf.Flow(l + r, l + r + 1));
            foreach (var e in mf.Edges())
            {
                int ll = e.From;
                int rr = e.To;
                if (e.Flow == 1 && ll < l && rr < l + r)
                    cw.WriteLineJoin(e.From, e.To - l);
            }
        }
    }
}
