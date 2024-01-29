using Kzrnm.Competitive.IO;

namespace AtCoder
{
    internal abstract class BaseSolver : CompetitiveVerifier.ProblemSolver
    {
        public override void Solve()
        {
            using var cw = new Utf8ConsoleWriter();
            Solve(new ConsoleReader(), cw);
        }
        public abstract void Solve(ConsoleReader cr, Utf8ConsoleWriter cw);
    }
}
