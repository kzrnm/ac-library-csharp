using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.String
{
    public class ZAlgorithmSolver : Solver
    {
        public override string Name => "zalgorithm";
        public override double TimeoutSecond => 5;
        public override void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            string s = cr;
            cw.WriteLineJoin(StringLib.ZAlgorithm(s));
        }
    }
}
