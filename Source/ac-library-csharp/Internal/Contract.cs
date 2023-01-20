using System;
using System.Diagnostics;

namespace AtCoder.Internal
{

    public static class Contract
    {
        /// <summary>
        /// if <paramref name="condition"/> is false, throw exception.
        /// </summary>
        [Conditional("ATCODER_CONTRACT")]
        public static void Assert(bool condition, string reason = null)
        {
            if (!condition)
                throw new ContractAssertException(reason);
        }

        /// <summary>
        /// if <paramref name="conditionFunc"/>() is false, throw exception.
        /// </summary>
        [Conditional("ATCODER_CONTRACT")]
        public static void Assert(Func<bool> conditionFunc, string reason = null)
        {
            if (!conditionFunc())
                throw new ContractAssertException(reason);
        }
    }
}
