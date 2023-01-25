using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    public static partial class MathLib
    {
        /// <summary>
        /// <paramref name="x"/>^<paramref name="n"/> mod <paramref name="m"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>, 1≤<paramref name="m"/></para>
        /// <para>計算量: O(log<paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static long PowMod(long x, long n, int m)
            => InternalMath.PowMod(x, n, m);
    }
}
