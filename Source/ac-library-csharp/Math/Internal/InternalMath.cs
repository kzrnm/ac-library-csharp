using System;
using System.Runtime.CompilerServices;


namespace AtCoder.Internal
{
    public static class InternalMath
    {
        /// <inheritdoc cref="ModPrimitiveRoot.PrimitiveRoot{TMod}" />
        [MethodImpl(256)]
        public static int PrimitiveRoot<TMod>() where TMod : struct, IStaticMod => ModPrimitiveRoot.PrimitiveRoot<TMod>();

        /// <inheritdoc cref="ModCalc.InvGcd(long, long)" />
        [MethodImpl(256)]
        public static (long, long) InvGcd(long a, long b)
            => ModCalc.InvGcd(a, b);

        /// <inheritdoc cref="ModCalc.SafeMod(long, long)" />
        [MethodImpl(256)]
        public static long SafeMod(long x, long m)
            => ModCalc.SafeMod(x, m);

        /// <inheritdoc cref="ModCalc.PowMod(long, long, int)" />
        [MethodImpl(256)]
        public static uint PowMod(long x, long n, int m)
            => ModCalc.PowMod(x, n, m);

        /// <inheritdoc cref="Mul128.Mul128Bit(ulong, ulong)" />
        [MethodImpl(256)]
        public static ulong Mul128Bit(ulong a, ulong b) => Mul128.Mul128Bit(a, b);

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

        /// <inheritdoc cref="FloorSum.FloorSumUnsigned(ulong, ulong, ulong, ulong)" />
        [MethodImpl(256)]
        public static ulong FloorSumUnsigned(ulong n, ulong m, ulong a, ulong b)
            => FloorSum.FloorSumUnsigned(n, m, a, b);
    }
}
