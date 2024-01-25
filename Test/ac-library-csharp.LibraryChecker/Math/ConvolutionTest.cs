using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.MathN
{
    internal class ConvolutionTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/convolution_mod";
        public override void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int n = cr;
            int m = cr;
            int[] a = cr.Repeat(n);
            int[] b = cr.Repeat(m);
            cw.WriteLineJoin(MathLib.Convolution<Mod998244353>(a, b));
        }
    }
}
