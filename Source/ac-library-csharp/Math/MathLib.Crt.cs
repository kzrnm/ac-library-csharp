using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    public static partial class MathLib
    {
        /// <inheritdoc cref="ModCalc.Crt(long[], long[])" />
        [MethodImpl(256)]
        public static (long y, long z) Crt(long[] r, long[] m)
            => ModCalc.Crt(r, m);
    }
}
