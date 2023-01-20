using System;
using AtCoder.Internal;
using FluentAssertions.Specialized;

namespace AtCoder
{
    public static class ContractAssertUtil
    {
        public static void ThrowContractAssert<TDelegate, TAssertions>(
            this DelegateAssertions<TDelegate, TAssertions> assertions,
            string because = "",
            params object[] becauseArgs)
            where TDelegate : Delegate where TAssertions : DelegateAssertions<TDelegate, TAssertions>
        {
            assertions.Throw<ContractAssertException>(because, becauseArgs);
        }
    }
}
