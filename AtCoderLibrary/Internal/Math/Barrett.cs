using System.Runtime.Intrinsics.X86;

namespace AtCoder.Internal
{
    /// <summary>
    /// Fast moduler by barrett reduction
    /// <seealso href="https://en.wikipedia.org/wiki/Barrett_reduction"/>
    /// </summary>
    public class Barrett
    {
        public uint Mod { get; private set; }
        internal readonly ulong IM;
        public Barrett(uint m)
        {
            Mod = m;
            IM = unchecked((ulong)-1) / m + 1;
        }

        /// <summary>
        /// <paramref name="a"/> * <paramref name="b"/> mod m
        /// </summary>
        public uint Mul(uint a, uint b)
        {
            ulong z = a;
            z *= b;
            if (!Bmi2.X64.IsSupported) return (uint)(z % Mod);
            var x = Bmi2.X64.MultiplyNoFlags(z, IM);
            var v = unchecked((uint)(z - x * Mod));
            if (Mod <= v) v += Mod;
            return v;
        }
    }
}
