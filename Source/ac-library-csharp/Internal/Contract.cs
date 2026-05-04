using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AtCoder.Internal
{

    public static class Contract
    {
        /// <summary>
        /// if <paramref name="condition"/> is false, throw exception.
        /// </summary>
        [Conditional("ATCODER_CONTRACT")]
        [MethodImpl(256)]
        public static void Assert(bool condition, DefaultInterpolatedStringHandler reason)
        {
            if (!condition)
                Throw(reason.ToStringAndClear());
        }

        /// <summary>
        /// if <paramref name="condition"/> is false, throw exception.
        /// </summary>
        [Conditional("ATCODER_CONTRACT")]
        [MethodImpl(256)]
        public static void Assert(bool condition, string reason = null)
        {
            if (!condition)
                Throw(reason);
        }

        /// <summary>
        /// if <paramref name="conditionFunc"/>() is false, throw exception.
        /// </summary>
        [Conditional("ATCODER_CONTRACT")]
        [MethodImpl(256)]
        public static void Assert(Func<bool> conditionFunc, string reason = null)
        {
            if (!conditionFunc())
                Throw(reason);
        }
        static void Throw(string reason) => throw new ContractAssertException(reason);
    }
}
