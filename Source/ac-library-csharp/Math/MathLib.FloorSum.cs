using System.Runtime.CompilerServices;

namespace AtCoder
{
    public static partial class MathLib
    {
        /// <inheritdoc cref="Internal.FloorSum.FloorSumSigned(long, long, long, long)" />
        [MethodImpl(256)]
        public static long FloorSum(long n, long m, long a, long b)
            => Internal.FloorSum.FloorSumSigned(n, m, a, b);
    }
}
