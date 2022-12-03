using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.MathN
{
    internal class FloorSumTest : BaseSover
    {
        public override string Url => "https://judge.yosupo.jp/problem/sum_of_floor_of_linear";
        public override void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int T = cr;
            for (int i = 0; i < T; i++)
            {
                cw.WriteLine(MathLib.FloorSum(cr, cr, cr, cr));
            }
        }
    }
}
