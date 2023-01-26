using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#if NETCOREAPP3_1_OR_GREATER
using System.Runtime.Intrinsics.X86;
using System.Numerics;
#endif

namespace AtCoder.Internal
{
    public static class InternalMath
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

#if NET7_0_OR_GREATER
            ref var result = ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault(primitiveRootsCache, m, out var exists);
            if (!exists)
            {
                result = PrimitiveRootCalculate<TMod>();
            }
            return result;
#else
            if (primitiveRootsCache.TryGetValue(m, out var p))
            {
                return p;
            }

            return primitiveRootsCache[m] = PrimitiveRootCalculate<TMod>();
#endif
        }
        static int PrimitiveRootCalculate<TMod>() where TMod : struct, IStaticMod
        {
            var m = default(TMod).Mod;
            Span<uint> divs = stackalloc uint[20];
            divs[0] = 2;
            int cnt = 1;
            var x = m - 1;
            x >>= BitOperations.TrailingZeroCount(x);

            for (uint i = 3; (long)i * i <= x; i += 2)
            {
                if (x % i == 0)
                {
                    divs[cnt++] = i;
                    do
                    {
                        x /= i;
                    } while (x % i == 0);
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
            NEXT:;
            }
        }

        /// <summary>
        /// g=gcd(a,b),xa=g(mod b) となるような 0≤x&lt;b/g の(g, x)
        /// </summary>
        /// <remarks>
        /// <para>制約: 1≤<paramref name="b"/></para>
        /// </remarks>
        [MethodImpl(256)]
        public static (long, long) InvGcd(long a, long b)
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
            if (n <= 1) return false;
            if (n == 2 || n == 7 || n == 61) return true;
            if (n % 2 == 0) return false;
            long d = n - 1;
            while (d % 2 == 0) d /= 2;
            ReadOnlySpan<byte> bases = stackalloc byte[3] { 2, 7, 61 };
            foreach (long a in bases)
            {
                long t = d;
                long y = PowMod(a, t, n);
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
        /// <summary>
        /// <inheritdoc cref="MathLib.PowMod(long, long, int)" />
        /// </summary>
        [MethodImpl(256)]
        public static uint PowMod(long x, long n, int m)
        {
            Contract.Assert(0 <= n && 1 <= m, reason: $"0 <= {nameof(n)} && 1 <= {nameof(m)}");
            if (m == 1) return 0;
            var barrett = new Barrett((uint)m);
            uint r = 1, y = (uint)SafeMod(x, m);
            while (0 < n)
            {
                if ((n & 1) != 0) r = barrett.Mul(r, y);
                y = barrett.Mul(y, y);
                n >>= 1;
            }
            return r;
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

        /// <summary>
        /// <paramref name="a"/> * <paramref name="b"/> の上位 64 ビットを返します。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [MethodImpl(256)]
        public static ulong Mul128Bit(ulong a, ulong b)
        {
#if NETCOREAPP3_1_OR_GREATER
            if (Bmi2.X64.IsSupported)
                return Bmi2.X64.MultiplyNoFlags(a, b);
#endif
            return Mul128BitLogic(a, b);
        }

        /// <summary>
        /// <paramref name="a"/> * <paramref name="b"/> の上位 64 ビットを返します。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [MethodImpl(256)]
        internal static ulong Mul128BitLogic(ulong a, ulong b)
        {
            /*
             * ここでは 64 bit 整数 X の上位, 下位 32 bit をそれぞれ Xu ,Xd と表す
             * A * B を計算する。
             * 
             * 変数を下記のように定義する。
             * s := 1ul << 32
             * R := A * B
             * L  := Ad * Bd
             * M1 := Ad * Bu
             * M2 := Au * Bd
             * H  := Au * Bu
             * 
             * A * B = s * s * H + s * (M1 + M2) + L
             * であることを使って
             * R = s * s * x + y
             * と表せる x, y を求めたい (x, y < s * s)
             * 
             * A * B
             * = s * s * H + s * (s * M1u + M1d + s * M2u + M2d) + L
             * = s * s * H + s * (s * (M1u + M2u) + M1d + M2d) + L
             * = s * s * (H + M1u + M2u) + s * (M1d + M2d) + L
             * 
             * と表せるので、繰り上がりを考慮しなければ
             * x = H + M1u + M2u
             * y = s * (M1d + M2d) + L = s * (M1d + M2d + Lu) + Ld
             * となる
             * 
             * 繰り上がるのは
             * M1d + M2d + Lu の上位 32 bit なので
             * C := M1d + M2d + Lu
             * とすると
             * 
             * x = H + M1u + M2u + Cu
             * となる
             */
            var au = a >> 32;
            var ad = a & 0xFFFFFFFF;
            var bu = b >> 32;
            var bd = b & 0xFFFFFFFF;

            var l = ad * bd;
            var m1 = au * bd;
            var m2 = ad * bu;
            var h = au * bu;

            var lu = l >> 32;
            var m1d = m1 & 0xFFFFFFFF;
            var m2d = m2 & 0xFFFFFFFF;
            var c = m1d + m2d + lu;

            return h + (m1 >> 32) + (m2 >> 32) + (c >> 32);
        }
    }
}
