using System.IO;
using System.Text;
using Kzrnm.Competitive.IO;
using Kzrnm.Competitive.LibraryChecker;

namespace AtCoder.Solvers
{
    public abstract class Solver : ICompetitiveSolver
    {
        public abstract string Name { get; }
        public abstract double TimeoutSecond { get; }
        public abstract void Solve(ConsoleReader cr, ConsoleWriter cw);

        public void Solve(Stream inputStream, Stream outputStream)
        {
            var utf8 = new UTF8Encoding(false);
            var cr = new ConsoleReader(inputStream, utf8);
            using var cw = new ConsoleWriter(outputStream, utf8);
            Solve(cr, cw);
        }
    }
}
