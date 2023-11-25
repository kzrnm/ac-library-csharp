using System.Runtime.CompilerServices;

namespace AtCoder.Internal
{
    public static class FloorSum
    {
        /// <summary>
        /// sum_{i=0}^{<paramref name="n"/>-1} floor((<paramref name="a"/>*i+<paramref name="b"/>)/<paramref name="m"/>) を返します。答えがオーバーフローしたならば  mod2^64 で等しい値を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約:</para>
        /// <para> 0≤<paramref name="n"/>&lt;2^32</para>
        /// <para> 1≤<paramref name="m"/>&lt;2^32</para>
        /// <para>計算量: O(log(m))</para>
        /// </remarks>
        /// <returns></returns>
        [MethodImpl(256)]
        public static long FloorSumSigned(long n, long m, long a, long b)
        {
            Contract.Assert(0 <= n && n < (1L << 32));
            Contract.Assert(1 <= m && m < (1L << 32));
            var nn = (ulong)n;
            var mm = (ulong)m;
            ulong aa, bb;
            ulong ans = 0;
            if (a < 0)
            {
                var a2 = (ulong)ModCalc.SafeMod(a, m);
                ans -= nn * (nn - 1) / 2 * ((a2 - (ulong)a) / mm);
                aa = a2;
            }
            else aa = (ulong)a;
            if (b < 0)
            {
                var b2 = (ulong)ModCalc.SafeMod(b, m);
                ans -= nn * ((b2 - (ulong)b) / mm);
                bb = b2;
            }
            else bb = (ulong)b;

            return (long)(ans + FloorSumUnsigned(nn, mm, aa, bb));
        }

        [MethodImpl(256)]
        public static ulong FloorSumUnsigned(ulong n, ulong m, ulong a, ulong b)
        {
            ulong ans = 0;
            while (true)
            {
                if (a >= m)
                {
                    ans += (n - 1) * n / 2 * (a / m);
                    a %= m;
                }
                if (b >= m)
                {
                    ans += n * (b / m);
                    b %= m;
                }

                ulong yMax = a * n + b;
                if (yMax < m) return ans;
                (n, m, a, b) = (yMax / m, a, m, yMax % m);
            }
        }
    }
}
