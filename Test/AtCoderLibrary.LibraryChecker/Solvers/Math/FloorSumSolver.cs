using Kzrnm.Competitive;
using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.MathN
{
    public class FloorSumSolver : ISolver
    {
        public string Name => "sum_of_floor_of_linear";

        public void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int T = cr;
            for (int i = 0; i < T; i++)
            {
                cw.WriteLine(MathLib.FloorSum(cr, cr, cr, cr));
            }
        }
    }
}
