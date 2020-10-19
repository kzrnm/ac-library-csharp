using System;
using System.Diagnostics;

namespace AtCoder.Internal
{
    internal static class DebugUtil
    {
        /// <summary>
        /// if <paramref name="condition"/> is false, throw exception.
        /// </summary>
        [Conditional("DEBUG")]
        public static void Assert(bool condition, string reason = null) { if (!condition) throw new DebugAssertException(reason); }

        /// <summary>
        /// if <paramref name="conditionFunc"/>() is false, throw exception.
        /// </summary>
        [Conditional("DEBUG")]
        public static void Assert(Func<bool> conditionFunc, string reason = null) { if (!conditionFunc()) throw new DebugAssertException(reason); }
    }
}
