using System.Runtime.CompilerServices;

namespace AtCoder.Internal
{
    /// <summary>
    /// Fast moduler by barrett reduction
    /// <seealso href="https://en.wikipedia.org/wiki/Barrett_reduction"/>
    /// </summary>
    public class Barrett(uint m)
    {
        public readonly uint Mod = m;
        public readonly ulong IM = ulong.MaxValue / m + 1;

        /// <summary>
        /// <paramref name="a"/> * <paramref name="b"/> mod m
        /// </summary>
        [MethodImpl(256)]
        public uint Mul(uint a, uint b) => Reduce((ulong)a * b);

        [MethodImpl(256)]
        public uint Reduce(ulong z)
        {
            var x = System.Math.BigMul(IM, z, out _);
            var y = x * Mod;
            if (z < y) return (uint)(z - y + Mod);
            return (uint)(z - y);
        }

        /// <summary>
        /// <paramref name="x"/>^<paramref name="n"/> mod m を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        [MethodImpl(256)]
        public uint Pow(long x, long n)
        {
            Contract.Assert(0 <= n, $"{nameof(n)} must be positive.");
            return Pow(x, (ulong)n);
        }

        /// <summary>
        /// <paramref name="x"/>^<paramref name="n"/> mod m を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        [MethodImpl(256)]
        public uint Pow(long x, ulong n)
        {
            if (Mod == 1) return 0;
            uint r = 1, y = (uint)ModCalc.SafeMod(x, Mod);
            while (n > 0)
            {
                if ((n & 1) != 0) r = Mul(r, y);
                y = Mul(y, y);
                n >>= 1;
            }
            return r;
        }
    }
}
