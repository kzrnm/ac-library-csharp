using System;
using System.IO;

namespace AtCoder.Test.Utils
{
    class SolverRunner
    {
        private Action<TextReader, TextWriter> Action { get; }
        public SolverRunner(Action<TextReader, TextWriter> action)
        {
            Action = action;
        }
        public string Solve(string input)
        {
            using var sr = new StringReader(input);
            using var sw = new StringWriter();
            Action(sr, sw);
            return sw.ToString();
        }
    }
}
