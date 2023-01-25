using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.String
{
    internal class LcpArrayTest : BaseSover
    {
        public override string Url => "https://judge.yosupo.jp/problem/number_of_substrings";
        public override void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            string s = cr;
            var sa = StringLib.SuffixArray(s);
            var answer = 1L * s.Length * (s.Length + 1) / 2;
            foreach (var x in StringLib.LcpArray(s, sa))
            {
                answer -= x;
            }
            cw.WriteLine(answer);
        }
    }
}
