using System;
using System.Collections.Generic;
using System.Text;

namespace AtCoder
{
    public static partial class MathLib
    {
        /// <summary>
        /// sum_{i=0}^{<paramref name="n"/>-1} floor(<paramref name="a"/>*i+<paramref name="b"/>/<paramref name="m"/>) を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>, <paramref name="m"/>≤10^9, 0≤<paramref name="a"/>, <paramref name="b"/>&lt;<paramref name="m"/></para>
        /// <para>計算量: O(log(n+m+a+b))</para>
        /// </remarks>
        /// <returns></returns>
        public static long FloorSum(long n, long m, long a, long b) 
        {
            long ans = 0;
            while (true)
            {
                if (a >= m)
                {
                    ans += (n - 1) * n * (a / m) / 2;
                    a %= m;
                }
                if (b >= m)
                {
                    ans += n * (b / m);
                    b %= m;
                }

                long yMax = (a * n + b) / m;
                long xMax = yMax * m - b;
                if (yMax == 0) return ans;
                ans += (n - (xMax + a - 1) / a) * yMax;
                (n, m, a, b) = (yMax, a, m, (a - xMax % a) % a);
            }
        }
    }
}
