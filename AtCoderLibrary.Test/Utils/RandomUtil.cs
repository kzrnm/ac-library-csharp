using System;
using FluentAssertions;

namespace AtCoder
{
    static class RandomUtil
    {
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
    }
}
