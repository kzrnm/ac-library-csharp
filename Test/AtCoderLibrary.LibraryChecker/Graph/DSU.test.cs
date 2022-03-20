using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.Graph
{
    public class DSUTest
    {
        static void Main() { using var cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/unionfind
        static void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
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
