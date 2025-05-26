using System.Collections.Generic;
using AtCoder.Internal;
using Shouldly;
using Xunit;

namespace AtCoder
{
    static class MathUtil
    {
        public static List<int> Factors(int m)
        {
            var result = new List<int>();
            for (int i = 2; (long)i * i <= m; i++)
            {
                if (m % i == 0)
                {
                    result.Add(i);
                    while (m % i == 0)
                    {
                        m /= i;
                    }
                }
            }
            if (m > 1) result.Add(m);
            return result;
        }

        public static bool IsPrimitiveRoot(int m, int g)
        {
            (1 <= g && g < m).ShouldBeTrue();
            foreach (var x in Factors(m - 1))
            {
                if (MathLib.PowMod(g, (m - 1) / x, m) == 1)
                    return false;
            }
            return true;
        }
    }

    public class MathUtilTest
    {
        static bool IsPrimitiveRootNative(int m, int g)
        {
            (1 <= g && g < m).ShouldBeTrue();
            int x = 1;
            for (int i = 1; i <= m - 2; i++)
            {
                x = (int)((long)x * g % m);
                // x == n^i
                if (x == 1) return false;
            }
            x = (int)((long)x * g % m);
            x.ShouldBe(1);
            return true;
        }

        [Fact]
        public void IsPrimitiveRootTest()
        {
            const int size = Global.IsCi ? 500 : 250;
            for (int m = 2; m <= size; m++)
            {
                if (!InternalMath.IsPrime(m)) continue;
                for (int g = 1; g < m; g++)
                {
                    MathUtil.IsPrimitiveRoot(m, g).ShouldBe(IsPrimitiveRootNative(m, g));
                }
            }
        }
        [Fact]
        public void FactorsTest()
        {
            const int size = Global.IsCi ? 50000 : 10000;
            for (int m = 1; m <= size; m++)
            {
                var f = MathUtil.Factors(m);
                int m2 = m;
                foreach (var x in f)
                {
                    (m % x).ShouldBe(0);
                    while (m2 % x == 0) m2 /= x;
                }
                m2.ShouldBe(1);
            }
        }

    }
}
