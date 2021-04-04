using System.Collections.Generic;
using AtCoder.Internal;
using FluentAssertions;
using Xunit;

namespace AtCoder
{
    static class MathUtil
    {
        public static long Gcd(long a, long b)
        {
            (0 <= a && 0 <= b).Should().BeTrue();
            if (b == 0) return a;
            return Gcd(b, a % b);
        }

        public static bool IsPrimeNaive(long n)
        {
            (0 <= n && n <= int.MaxValue).Should().BeTrue();
            if (n == 0 || n == 1) return false;
            for (long i = 2; i * i <= n; i++)
            {
                if (n % i == 0) return false;
            }
            return true;
        }
        public static long PowModNaive(long x, ulong n, uint mod)
        {

            ulong y = (ulong)(x % mod + mod) % mod;
            ulong z = 1;
            for (ulong i = 0; i < n; i++)
            {
                z = (z * y) % mod;
            }
            return (long)(z % mod);
        }

        public static long FloorSumNative(long n, long m, long a, long b)
        {
            long sum = 0;
            for (long i = 0; i < n; i++)
            {
                long z = a * i + b;
                sum += (z - InternalMath.SafeMod(z, m)) / m;
            }
            return sum;
        }

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
            (1 <= g && g < m).Should().BeTrue();
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
        private bool IsPrimitiveRootNative(int m, int g)
        {
            (1 <= g && g < m).Should().BeTrue();
            int x = 1;
            for (int i = 1; i <= m - 2; i++)
            {
                x = (int)((long)x * g % m);
                // x == n^i
                if (x == 1) return false;
            }
            x = (int)((long)x * g % m);
            x.Should().Be(1);
            return true;
        }

        [Fact]
        public void IsPrimitiveRootTest()
        {
            for (int m = 2; m <= 500; m++)
            {
                if (!InternalMath.IsPrime(m)) continue;
                for (int g = 1; g < m; g++)
                {
                    MathUtil.IsPrimitiveRoot(m, g).Should().Be(IsPrimitiveRootNative(m, g));
                }
            }
        }
        [Fact]
        public void FactorsTest()
        {
            for (int m = 1; m <= 50000; m++)
            {
                var f = MathUtil.Factors(m);
                int m2 = m;
                foreach (var x in f)
                {
                    (m % x).Should().Be(0);
                    while (m2 % x == 0) m2 /= x;
                }
                m2.Should().Be(1);
            }
        }

    }
}
