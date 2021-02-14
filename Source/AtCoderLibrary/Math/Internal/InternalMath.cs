using System;
using System.Collections.Generic;

namespace AtCoder.Internal
{
    public static partial class InternalMath
    {
        private static readonly Dictionary<int, int> primitiveRootsCache = new Dictionary<int, int>()
        {
            { 2, 1 },
            { 167772161, 3 },
            { 469762049, 3 },
            { 754974721, 11 },
            { 998244353, 3 }
        };

        /// <summary>
        /// <paramref name="m"/> の最小の原始根を求めます。
        /// </summary>
        /// <remarks>
        /// 制約: <paramref name="m"/> は素数
        /// </remarks>
        public static int PrimitiveRoot(int m)
        {
            Contract.Assert(m >= 2, reason: $"{nameof(m)} must be greater or equal 2");

            if (primitiveRootsCache.TryGetValue(m, out var p))
            {
                return p;
            }

            return primitiveRootsCache[m] = PrimitiveRootCalculate(m);
        }
        static int PrimitiveRootCalculate(int m)
        {
            Span<int> divs = stackalloc int[20];
            divs[0] = 2;
            int cnt = 1;
            int x = (m - 1) / 2;

            while (x % 2 == 0)
            {
                x >>= 1;
            }

            for (int i = 3; (long)i * i <= x; i += 2)
            {
                if (x % i == 0)
                {
                    divs[cnt++] = i;
                    while (x % i == 0)
                    {
                        x /= i;
                    }
                }
            }

            if (x > 1)
            {
                divs[cnt++] = x;
            }

            for (int g = 2; ; g++)
            {
                bool ok = true;
                for (int i = 0; i < cnt; i++)
                {
                    if (MathLib.PowMod(g, (m - 1) / divs[i], m) == 1)
                    {
                        ok = false;
                        break;
                    }
                }

                if (ok)
                {
                    return g;
                }
            }
        }

        /// <summary>
        /// g=gcd(a,b),xa=g(mod b) となるような 0≤x&lt;b/g の(g, x)
        /// </summary>
        /// <remarks>
        /// <para>制約: 1≤<paramref name="b"/></para>
        /// </remarks>
        public static (long, long) InvGCD(long a, long b)
        {
            a = SafeMod(a, b);
            if (a == 0) return (b, 0);

            long s = b, t = a;
            long m0 = 0, m1 = 1;

            long u;
            while (true)
            {
                if (t == 0)
                {
                    if (m0 < 0) m0 += b / s;
                    return (s, m0);
                }
                u = s / t;
                s -= t * u;
                m0 -= m1 * u;

                if (s == 0)
                {
                    if (m1 < 0) m1 += b / t;
                    return (t, m1);
                }
                u = t / s;
                t -= s * u;
                m1 -= m0 * u;
            }
        }

        public static long SafeMod(long x, long m)
        {
            x %= m;
            if (x < 0) x += m;
            return x;
        }

        /// <summary>
        /// <paramref name="n"/> が素数かを返します。
        /// </summary>
        public static bool IsPrime(int n)
        {
            Contract.Assert(0 <= n);
            Contract.Assert(0 <= n, reason: $"{nameof(n)} must not be negative.");
            if (n <= 1) return false;
            if (n == 2 || n == 7 || n == 61) return true;
            if (n % 2 == 0) return false;
            long d = n - 1;
            while (d % 2 == 0) d /= 2;
            ReadOnlySpan<long> bases = stackalloc long[3] { 2, 7, 61 };
            foreach (long a in bases)
            {
                long t = d;
                long y = MathLib.PowMod(a, t, n);
                while (t != n - 1 && y != 1 && y != n - 1)
                {
                    y = y * y % n;
                    t <<= 1;
                }
                if (y != n - 1 && t % 2 == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
