using Kzrnm.Competitive.IO;

namespace AtCoder
{
    public interface ISolver
    {
        public string Name { get; }
        public void Solve(ConsoleReader cr, ConsoleWriter cw);
    }
}
