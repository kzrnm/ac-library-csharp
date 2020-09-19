using System;
using System.IO;
using System.Threading.Tasks;
using AtCoder.Expand;

namespace AtCoder.Writer
{
    public class ExpandWriter : IDisposable
    {
        internal ICodeExpander Expander { get; }
        protected Stream OutputStream { get; }
        protected StreamWriter Writer { get; }
        public ExpandWriter(string code, ExpandMethod expandMethod, Stream outputStream)
            : this(CodeExpander.Create(code, expandMethod), outputStream) { }
        internal ExpandWriter(ICodeExpander expander, Stream outputStream)
        {
            Expander = expander;
            OutputStream = outputStream;
            Writer = new StreamWriter(OutputStream);
        }

        public virtual void Expand()
        {
            foreach (var line in Expander.ExpandedLines())
            {
                Writer.WriteLine(line);
            }
            Writer.Flush();
        }
        public virtual async Task ExpandAsync()
        {
            foreach (var line in Expander.ExpandedLines())
            {
                await Writer.WriteLineAsync(line);
            }
            await Writer.FlushAsync();
        }

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Writer.Dispose();
                    OutputStream.Dispose();
                }
                _disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
