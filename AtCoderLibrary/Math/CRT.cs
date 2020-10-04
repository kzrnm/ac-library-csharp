using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AtCoder
{
    public static partial class MathLib
    {
        /// <summary>
        /// 同じ長さ n の配列 <paramref name="r"/>, <paramref name="m"/> について、x≡<paramref name="r"/>[i] (mod <paramref name="m"/>[i]),∀i∈{0,1,⋯,n−1} を解きます。
        /// </summary>
        /// <remarks>
        /// <para>制約: |<paramref name="r"/>|=|<paramref name="m"/>|, 1≤<paramref name="m"/>[i], lcm(m[i]) が ll に収まる</para>
        /// <para>計算量: O(nloglcm(<paramref name="m"/>))</para>
        /// </remarks>
        /// <returns>答えは(存在するならば) y,z(0≤y&lt;z=lcm(<paramref name="m"/>[i])) を用いて x≡y(mod z) の形で書ける。答えがない場合は(0,0)、n=0 の時は(0,1)、それ以外の場合は(y,z)。</returns>
        public static (long, long) CRT(long[] r, long[] m) 
        {
            Debug.Assert(r.Length == m.Length);

            long r0 = 0, m0 = 1;
            for (int i = 0; i < m.Length; i++)
            {
                Debug.Assert(1 <= m[i]);
                long r1 = InternalMath.SafeMod(r[i], m[i]);
                long m1 = m[i];
                if (m0 < m1) {
                    (r0, r1) = (r1, r0);
                    (m0, m1) = (m1, m0);
                }
                if (m0 % m1 == 0) {
                    if (r0 % m1 != r1) return (0, 0);
                    continue;
                }
                var (g, im) = InternalMath.InvGCD(m0, m1);

                long u1 = (m1 / g);
                if ((r1 - r0) % g != 0) return (0, 0);

                long x = (r1 - r0) / g % u1 * im % u1;
                r0 += x * m0;
                m0 *= u1;
                if (r0 < 0) r0 += m0;
            }
            return (r0, m0);
        }
    }
}
