using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AtCoder.Internal
{
    public static partial class InternalMath
    {
        private static readonly Dictionary<uint, int> primitiveRootsCache = new Dictionary<uint, int>()
        {
            { 2, 1 },
            { 167772161, 3 },
            { 469762049, 3 },
            { 754974721, 11 },
            { 998244353, 3 }
        };

        /// <summary>
        /// <typeparamref name="TMod"/> の最小の原始根を求めます。
        /// </summary>
        /// <remarks>
        /// 制約: <typeparamref name="TMod"/> は素数
        /// </remarks>
        [MethodImpl(256)]
        public static int PrimitiveRoot<TMod>() where TMod : struct, IStaticMod
        {
            uint m = default(TMod).Mod;
            Contract.Assert(m >= 2, reason: $"{nameof(m)} must be greater or equal 2");
            Contract.Assert(default(TMod).IsPrime, reason: $"{nameof(m)} must be prime number");

            if (primitiveRootsCache.TryGetValue(m, out var p))
            {
                return p;
            }

            return primitiveRootsCache[m] = PrimitiveRootCalculate<TMod>();
        }
        static int PrimitiveRootCalculate<TMod>() where TMod : struct, IStaticMod
        {
            int m = (int)default(TMod).Mod;
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
            divs = divs.Slice(0, cnt);

            for (int g = 2; ; g++)
            {
                foreach (var d in divs)
                    if (new StaticModInt<TMod>(g).Pow((m - 1) / d).Value == 1)
                        goto NEXT;
                return g;
            NEXT: { }
            }
        }

        /// <summary>
        /// g=gcd(a,b),xa=g(mod b) となるような 0≤x&lt;b/g の(g, x)
        /// </summary>
        /// <remarks>
        /// <para>制約: 1≤<paramref name="b"/></para>
        /// </remarks>
        [MethodImpl(256)]
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

        [MethodImpl(256)]
        public static long SafeMod(long x, long m)
        {
            x %= m;
            if (x < 0) x += m;
            return x;
        }

        /// <summary>
        /// <paramref name="n"/> が素数かを返します。
        /// </summary>
        [MethodImpl(256)]
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

        [MethodImpl(256)]
        public static ulong FloorSumUnsigned(ulong n, ulong m, ulong a, ulong b)
        {
            ulong ans = 0;
            while (true)
            {
                if (a >= m)
                {
                    ans += (n - 1) * n / 2 * (a / m);
                    a %= m;
                }
                if (b >= m)
                {
                    ans += n * (b / m);
                    b %= m;
                }

                ulong yMax = a * n + b;
                if (yMax < m) return ans;
                (n, m, a, b) = (yMax / m, a, m, yMax % m);
            }
        }
    }
}
