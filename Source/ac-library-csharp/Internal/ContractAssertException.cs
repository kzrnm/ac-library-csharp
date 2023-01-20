using System;

namespace AtCoder.Internal
{
    public class ContractAssertException : Exception
    {
        public ContractAssertException() : base() { }
        public ContractAssertException(string message) : base(message) { }
        public ContractAssertException(string message, Exception innerException) : base(message, innerException) { }
    }
}
