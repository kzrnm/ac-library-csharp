using System.IO;
using AtCoder.Internal.CodeExpander;

namespace AtCoder.Internal
{
    internal class ExpandWriter
    {
        private string OutputFilePath { get; }
        private string? LastWriteTimeHeader { get; }
        private ICodeExpander Expander { get; }
        public ExpandWriter(string outputFilePath, string? lastWriteTimeHeader, ICodeExpander expander)
        {
            OutputFilePath = outputFilePath;
            LastWriteTimeHeader = lastWriteTimeHeader;
            Expander = expander;
        }


        public void Expand()
        {
            var head = LastWriteTimeHeader;
            if (head != null)
            {
                using var fs = new FileStream(OutputFilePath, FileMode.OpenOrCreate);
                using var sr = new StreamReader(fs);
                if (sr.ReadLine() == head) return;
            }
            var newCodeLines = Expander.ExpandedLines();

            using var outFs = new FileStream(OutputFilePath, FileMode.Create, FileAccess.Write);
            using var outTx = new StreamWriter(outFs);

            if (head != null)
            {
                outTx.WriteLine(head);
            }

            foreach (var line in newCodeLines)
            {
                outTx.WriteLine(line);
            }
        }
    }
}
