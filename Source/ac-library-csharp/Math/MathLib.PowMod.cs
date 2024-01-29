using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    public static partial class MathLib
    {
        /// <inheritdoc cref="ModCalc.PowMod(long, long, int)" />
        [MethodImpl(256)]
        public static long PowMod(long x, long n, int m)
            => ModCalc.PowMod(x, n, m);
    }
}
