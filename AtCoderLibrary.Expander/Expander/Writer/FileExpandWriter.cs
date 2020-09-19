using System.IO;
using System.Threading.Tasks;
using AtCoder.Expand;

namespace AtCoder.Writer
{
    public class FileExpandWriter : ExpandWriter
    {
        private string? LastWriteTimeHeader { get; }
        private StreamReader Reader { get; }

        public FileExpandWriter(string code, ExpandMethod expandMethod, string outputFilePath)
            : this(code, expandMethod, outputFilePath, null) { }
        public FileExpandWriter(string code, ExpandMethod expandMethod, string outputFilePath, string? lastWriteTimeHeader)
            : this(CodeExpander.Create(code, expandMethod), outputFilePath, lastWriteTimeHeader) { }
        public FileExpandWriter(string code, ExpandMethod expandMethod, Stream outputStream)
            : this(code, expandMethod, outputStream, null) { }
        public FileExpandWriter(string code, ExpandMethod expandMethod, Stream outputStream, string? lastWriteTimeHeader)
            : this(CodeExpander.Create(code, expandMethod), outputStream, lastWriteTimeHeader) { }
        public FileExpandWriter(ICodeExpander expander, string outputFilePath, string? lastWriteTimeHeader)
            : this(expander, new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite), lastWriteTimeHeader) { }
        public FileExpandWriter(ICodeExpander expander, Stream outputStream, string? lastWriteTimeHeader)
        : base(expander, outputStream)
        {
            Reader = new StreamReader(OutputStream);
            LastWriteTimeHeader = lastWriteTimeHeader;
        }

        public override void Expand()
        {
            var head = LastWriteTimeHeader;
            var fs = OutputStream;
            if (head != null)
            {
                if (Reader.ReadLine() == head)
                    return;
            }
            fs.SetLength(0);

            if (head != null)
            {
                Writer.WriteLine(head);
            }
            base.Expand();
        }

        public override async Task ExpandAsync()
        {
            var head = LastWriteTimeHeader;
            var fs = OutputStream;
            if (head != null)
            {
                if (await Reader.ReadLineAsync() == head)
                    return;
            }
            fs.SetLength(0);

            if (head != null)
            {
                await Writer.WriteLineAsync(head);
            }
            await base.ExpandAsync();
        }
    }
}
