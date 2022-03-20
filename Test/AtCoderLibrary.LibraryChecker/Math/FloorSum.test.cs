using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.MathN
{
    public class FloorSumTest
    {
        static void Main() { using var cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/sum_of_floor_of_linear
        static void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int T = cr;
            for (int i = 0; i < T; i++)
            {
                cw.WriteLine(MathLib.FloorSum(cr, cr, cr, cr));
            }
        }
    }
}
