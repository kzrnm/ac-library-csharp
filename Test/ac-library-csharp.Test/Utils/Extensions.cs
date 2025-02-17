using System;
using System.Collections.Generic;
using System.IO;
using Shouldly;

namespace AtCoder
{
    static class Extensions
    {
        #region Random
        public static uint NextUInt(this Random rnd) => unchecked((uint)rnd.Next());
        public static (int, int) NextPair(this Random rnd, int lower, int upper)
        {
            (upper - lower).ShouldBeGreaterThanOrEqualTo(1);
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

        public static void ShouldEqualLines(this string actual, string expected)
        {
            actual.ToLines().ShouldBe(expected.ToLines());
        }
        #endregion String
    }
}
