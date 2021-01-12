using System;
using AtCoder.Internal;
using FluentAssertions.Specialized;

namespace AtCoder
{
    public static class ContractAssertUtil
    {
        public static void ThrowContractAssert<TDelegate>(
            this DelegateAssertions<TDelegate> assertions,
            string because = "",
            params object[] becauseArgs) where TDelegate : Delegate
        {
            assertions.Throw<ContractAssertException>(because, becauseArgs);
        }
    }
}
