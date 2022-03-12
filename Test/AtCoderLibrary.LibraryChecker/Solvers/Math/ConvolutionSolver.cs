using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.MathN
{
    public class ConvolutionSolver : Solver
    {
        public override string Name => "convolution_mod";
        public override double TimeoutSecond => 5;
        public override void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int n = cr;
            int m = cr;
            int[] a = cr.Repeat(n);
            int[] b = cr.Repeat(m);
            cw.WriteLineJoin(MathLib.Convolution<Mod998244353>(a, b));
        }
    }
}
