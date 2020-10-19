using System;

namespace AtCoder.Internal
{
    public class DebugAssertException : Exception
    {
        public DebugAssertException() : base() { }
        public DebugAssertException(string message) : base(message) { }
        public DebugAssertException(string message, Exception innerException) : base(message, innerException) { }
    }
}
