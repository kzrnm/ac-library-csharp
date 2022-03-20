using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.String
{
    public class LcpArrayTest
    {
        static void Main() { using var cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/number_of_substrings
        static void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            string s = cr;
            var sa = StringLib.SuffixArray(s);
            var answer = 1L * s.Length * (s.Length + 1) / 2;
            foreach (var x in StringLib.LCPArray(s, sa))
            {
                answer -= x;
            }
            cw.WriteLine(answer);
        }
    }
}
