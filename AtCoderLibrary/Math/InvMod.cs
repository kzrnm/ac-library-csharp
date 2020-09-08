using System.Diagnostics;
using AtCoder.Internal;

namespace AtCoder
{
    public static partial class Math
    {
        /// <summary>
        /// <paramref name="x"/>y≡1(mod <paramref name="m"/>) なる y のうち、0≤y&lt;<paramref name="m"/> を満たすものを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: gcd(<paramref name="x"/>,<paramref name="m"/>)=1, 1≤<paramref name="m"/></para>
        /// <para>計算量: O(log<paramref name="m"/>)</para>
        /// </remarks>
        public static long InvMod(long x, int m)
        {
            Debug.Assert(1 <= m);
            var (g, res) = InternalMath.InvGCD(x, m);
            Debug.Assert(g == 1);
            return res;
        }
    }
}
