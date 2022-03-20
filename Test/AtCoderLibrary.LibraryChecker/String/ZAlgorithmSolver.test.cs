using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.String
{
    public class ZAlgorithmSolver
    {
        static void Main() { using var cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/zalgorithm
        static void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            string s = cr;
            cw.WriteLineJoin(StringLib.ZAlgorithm(s));
        }
    }
}
