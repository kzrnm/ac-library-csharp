using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.String
{
    internal class ZAlgorithmTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/zalgorithm";
        public override void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            string s = cr;
            cw.WriteLineJoin(StringLib.ZAlgorithm(s));
        }
    }
}
