using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.String
{
    public class LcpArraySolver : Solver
    {
        public override string Name => "number_of_substrings";
        public override double TimeoutSecond => 5;
        public override void Solve(ConsoleReader cr, ConsoleWriter cw)
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
