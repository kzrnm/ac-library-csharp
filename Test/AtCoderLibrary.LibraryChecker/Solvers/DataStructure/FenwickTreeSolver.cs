using Kzrnm.Competitive;
using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.DataStructure
{
    public class FenwickTreeSolver : ISolver
    {
        public string Name => "point_add_range_sum";

        public void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            int[] a = cr.Repeat(N);
            var fw = new LongFenwickTree(N);
            for (int i = 0; i < a.Length; i++)
            {
                fw.Add(i, a[i]);
            }
            for (int i = 0; i < Q; i++)
            {
                int t = cr;
                int l = cr;
                int r = cr;
                if (t == 0)
                    fw.Add(l, r);
                else
                    cw.WriteLine(fw[l..r]);
            }
        }
    }
}
