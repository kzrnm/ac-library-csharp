using Kzrnm.Competitive;
using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.String
{
    public class LcpArraySolver : ISolver
    {
        public string Name => "number_of_substrings";

        public void Solve(ConsoleReader cr, ConsoleWriter cw)
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
