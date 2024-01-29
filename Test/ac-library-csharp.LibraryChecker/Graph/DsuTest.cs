using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.Graph
{
    internal class DsuTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/unionfind";
        public override void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int n = cr;
            int q = cr;

            var dsu = new Dsu(n);

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
