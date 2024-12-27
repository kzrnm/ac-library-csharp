using System;
using System.IO;

namespace AtCoder
{
    record SolverRunner(Action<TextReader, TextWriter> Action)
    {
        public string Solve(string input)
        {
            using var sr = new StringReader(input);
            using var sw = new StringWriter();
            Action(sr, sw);
            return sw.ToString();
        }
    }
}
