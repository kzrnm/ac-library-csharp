using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive
{
    public interface ISolver
    {
        public string Name { get; }
        public void Solve(ConsoleReader cr, ConsoleWriter cw);
    }
}
