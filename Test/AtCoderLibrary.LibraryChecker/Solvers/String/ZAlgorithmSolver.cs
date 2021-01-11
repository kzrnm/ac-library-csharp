using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.String
{
    public class ZAlgorithmSolver : ISolver
    {
        public string Name => "zalgorithm";

        public void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            string s = cr;
            cw.WriteLineJoin(StringLib.ZAlgorithm(s));
        }
    }
}
