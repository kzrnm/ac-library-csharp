using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.MathN
{
    public class ConvolutionSolver
    {
        static void Main() { using var cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/convolution_mod
        static void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int n = cr;
            int m = cr;
            int[] a = cr.Repeat(n);
            int[] b = cr.Repeat(m);
            cw.WriteLineJoin(MathLib.Convolution<Mod998244353>(a, b));
        }
    }
}
