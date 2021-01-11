using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.Graph
{
    public class DSUSolver : ISolver
    {
        public string Name => "unionfind";

        public void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int n = cr;
            int q = cr;

            var dsu = new DSU(n);

            for (int i = 0; i < q; i++)
            {
                int t = cr;
                int u = cr;
                int v = cr;
                if (t == 0)
                    dsu.Merge(u, v);
                else
                    cw.WriteLine(dsu.Same(u, v) ? 1 : 0);
            }
        }
    }
}
