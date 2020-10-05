using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AtCoder.Test.Utils
{
    public class TestWithDebugAssert
    {
        protected StringBuilder TraceLog { get; }
        protected StringWriter Writer { get; }
        protected TextWriterTraceListener Listener { get; }

        public TestWithDebugAssert()
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new DebugAssertTraceListener());
        }
    }
    public class DebugAssertTraceListener : DefaultTraceListener
    {
        private static (string file, int line) GetFailedMethod()
        {
            var stackTrace = new StackTrace(true);
            var frame = stackTrace.GetFrames()
                .SkipWhile(f => !IsDebugAssert(f))
                .Skip(1)
                .FirstOrDefault();
            if (frame != null)
                return (frame.GetFileName(), frame.GetFileLineNumber());
            return default;
            static bool IsDebugAssert(StackFrame frame)
            {
                var method = frame.GetMethod();
                return method?.ReflectedType == typeof(Debug) && (method.Name == "Assert" || method.Name == "Fail");
            }
        }
        public override void Fail(string message, string detailMessage)
        {
            var (file, line) = GetFailedMethod();
            if (file != null)
                throw new DebugAssertException(message, detailMessage, file, line);
        }
        public override void Fail(string message)
        {
            Fail(message, null);
        }
    }
    public class DebugAssertException : Exception
    {
        public string DetailMessage { get; }
        public string FileName { get; }
        public int FileLineNumber { get; }
        internal DebugAssertException(string message, string detailMessage, string fileName, int fileLineNumber) : base(message)
        {
            DetailMessage = detailMessage;
            FileName = fileName;
            FileLineNumber = fileLineNumber;
        }
    }
}
