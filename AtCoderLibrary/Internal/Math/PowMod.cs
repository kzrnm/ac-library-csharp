using System.Diagnostics;

namespace AtCoder.Internal
{
    public static partial class InternalMath
    {
        /// <summary>
        /// (<paramref name="x"/>^<paramref name="n"/>) mod <paramref name="m"/> を返します。
        /// </summary>
        /// <remarks>
        /// 制約: 0≤<paramref name="n"/>, 1≤<paramref name="m"/>
        /// </remarks>
        public static long PowMod(long x, long n, int m)
        {
            Debug.Assert(n >= 0);
            Debug.Assert(m >= 1);

            if (m == 1)
            {
                return 0;
            }

            uint _m = (uint)m;
            ulong r = 1;
            ulong y = (ulong)SafeMod(x, m);

            while (n > 0)
            {
                if ((n & 1) > 0)
                {
                    r = (r * y) % _m;
                }

                y = (y * y) % _m;
                n >>= 1;
            }

            return (long)r;
        }
    }
}
