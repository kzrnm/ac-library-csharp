﻿using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.MathN
{
    public class FloorSumSolver : Solver
    {
        public override string Name => "sum_of_floor_of_linear";
        public override double TimeoutSecond => 5;
        public override void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int T = cr;
            for (int i = 0; i < T; i++)
            {
                cw.WriteLine(MathLib.FloorSum(cr, cr, cr, cr));
            }
        }
    }
}
