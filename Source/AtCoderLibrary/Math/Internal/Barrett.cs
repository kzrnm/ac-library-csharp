#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics.X86;
#endif
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
        internal readonly ulong IM;
        public Barrett(uint m)
        {
            Mod = m;
            IM = unchecked((ulong)-1) / m + 1;
        }

        /// <summary>
        /// <paramref name="a"/> * <paramref name="b"/> mod m
        /// </summary>
        [MethodImpl(256)]
        public uint Mul(uint a, uint b)
        {
            var z = (ulong)a * b;
#if NETCOREAPP3_0_OR_GREATER
            if (Bmi2.X64.IsSupported)
            {
                var x = Bmi2.X64.MultiplyNoFlags(z, IM);
                var v = unchecked((uint)(z - x * Mod));
                if (Mod <= v) v += Mod;
                return v;
            }
#endif
            return (uint)(z % Mod);
        }
    }
}
