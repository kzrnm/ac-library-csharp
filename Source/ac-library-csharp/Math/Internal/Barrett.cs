using System.Runtime.CompilerServices;

namespace AtCoder.Internal
{
    /// <summary>
    /// Fast moduler by barrett reduction
    /// <seealso href="https://en.wikipedia.org/wiki/Barrett_reduction"/>
    /// </summary>
    public class Barrett
    {
        public readonly uint Mod;
        public readonly ulong IM;
        public Barrett(uint m)
        {
            Mod = m;
            IM = unchecked((ulong)-1) / m + 1;
        }

        /// <summary>
        /// <paramref name="a"/> * <paramref name="b"/> mod m
        /// </summary>
        [MethodImpl(256)]
        public uint Mul(uint a, uint b) => Reduce((ulong)a * b);

        [MethodImpl(256)]
        public uint Reduce(ulong z)
        {
            var x = InternalMath.Mul128Bit(z, IM);
            var v = unchecked((uint)(z - x * Mod));
            if (Mod <= v) v += Mod;
            return v;
        }
    }
}
