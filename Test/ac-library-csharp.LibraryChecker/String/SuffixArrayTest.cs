using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.String
{
    internal class SuffixArrayTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/suffixarray";
        public override void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            string s = cr;
            cw.WriteLineJoin(StringLib.SuffixArray(s));
        }
    }
}
