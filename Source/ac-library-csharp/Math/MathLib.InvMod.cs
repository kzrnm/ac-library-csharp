using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    public static partial class MathLib
    {
        /// <summary>
        /// <paramref name="x"/>y≡1(mod <paramref name="m"/>) なる y のうち、0≤y&lt;<paramref name="m"/> を満たすものを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: gcd(<paramref name="x"/>,<paramref name="m"/>)=1, 1≤<paramref name="m"/></para>
        /// <para>計算量: O(log<paramref name="m"/>)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static long InvMod(long x, long m)
        {
            Contract.Assert(1 <= m, reason: $"1 <= {nameof(m)}");
            var (g, res) = InternalMath.InvGcd(x, m);
            Contract.Assert(g == 1, reason: $"gcd({nameof(x)}, {nameof(m)}) must be 1.");
            return res;
        }
    }
}
