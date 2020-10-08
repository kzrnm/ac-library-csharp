using System;
using System.Diagnostics;
using System.Threading;

namespace AtCoder
{
    public sealed class DebugAssertSemaphore : DefaultTraceListener
    {
        static DebugAssertSemaphore()
        {
            Trace.Listeners.Clear();
        }
        private static readonly SemaphoreSlim sem = new SemaphoreSlim(1, 1);
        public DebugAssertSemaphore()
        {
#if !DEBUG
            Skip.If(true, "not defined DEBUG symbol");
#endif
            sem.Wait();
            Trace.Listeners.Add(this);
        }

        public override void Fail(string message, string detailMessage)
        {
            throw new DebugAssertException(message, detailMessage);
        }
        public override void Fail(string message)
        {
            Fail(message, null);
        }

        protected override void Dispose(bool disposing)
        {
            Trace.Listeners.Remove(this);
            sem.Release();
            base.Dispose(disposing);
        }
    }
    public class DebugAssertException : Exception
    {
        public string DetailMessage { get; }
        internal DebugAssertException(string message, string detailMessage) : base(message)
        {
            DetailMessage = detailMessage;
        }
    }
}
