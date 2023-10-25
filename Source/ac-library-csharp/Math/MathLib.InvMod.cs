using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    public static partial class MathLib
    {
        /// <inheritdoc cref="ModCalc.InvMod(long, long)" />
        [MethodImpl(256)]
        public static long InvMod(long x, long m)
            => ModCalc.InvMod(x, m);
    }
}
