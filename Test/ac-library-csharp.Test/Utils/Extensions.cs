using System;
using System.Collections.Generic;
using System.IO;
using AtCoder.Internal;
using FluentAssertions;
using FluentAssertions.Primitives;
using FluentAssertions.Specialized;

namespace AtCoder
{
    static class Extensions
    {
        #region Contract
        public static void ThrowContractAssert<TDelegate, TAssertions>(
            this DelegateAssertions<TDelegate, TAssertions> assertions,
            string because = "",
            params object[] becauseArgs)
            where TDelegate : Delegate where TAssertions : DelegateAssertions<TDelegate, TAssertions>
        {
            assertions.Throw<ContractAssertException>(because, becauseArgs);
        }
        #endregion Contract

        #region Random
        public static uint NextUInt(this Random rnd) => unchecked((uint)rnd.Next());
        public static (int, int) NextPair(this Random rnd, int lower, int upper)
        {
            (upper - lower).Should().BeGreaterOrEqualTo(1);
            int a, b;
            do
            {
                a = rnd.Next(lower, upper);
                b = rnd.Next(lower, upper);
            } while (a == b);
            if (a > b) (a, b) = (b, a);
            return (a, b);
        }
        public static bool NextBool(this Random rnd) => rnd.Next(2) == 0;
        #endregion Random

        #region String
        public static IEnumerable<string> ToLines(this string str)
        {
            using var sr = new StringReader(str);
            var line = sr.ReadLine();
            while (line != null)
            {
                yield return line;
                line = sr.ReadLine();
            }
        }

        public static void EqualLines(this StringAssertions assertions, string expected)
        {
            assertions.Subject.ToLines().Should().Equal(expected.ToLines());
        }
        #endregion String
    }
}
