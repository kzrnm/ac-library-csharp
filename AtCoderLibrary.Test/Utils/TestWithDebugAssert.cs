using System;
using System.Diagnostics;
using System.IO;
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
    public class DebugAssertTraceListener : TraceListener
    {
        public override void Write(string message) { }
        public override void WriteLine(string message)
        {
            if (message.StartsWith("Fail:"))
                throw new DebuAssertException(message);
        }
    }
    public class DebuAssertException : Exception
    {
        public bool IsWriteLine { get; }
        internal DebuAssertException() { }
        internal DebuAssertException(string message) : base(message) { }
    }
}
