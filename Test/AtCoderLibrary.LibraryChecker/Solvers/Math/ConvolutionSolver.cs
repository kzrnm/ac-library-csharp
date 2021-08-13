using Kzrnm.Competitive;
using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.MathN
{
    public class ConvolutionSolver : ISolver
    {
        public string Name => "convolution_mod";

        public void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int n = cr;
            int m = cr;
            int[] a = cr.Repeat(n);
            int[] b = cr.Repeat(m);
            cw.WriteLineJoin(MathLib.Convolution<Mod998244353>(a, b));
        }
    }
}
