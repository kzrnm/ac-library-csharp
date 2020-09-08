using System;
using System.Collections.Generic;
using System.Text;

namespace AtCoder
{
    public static partial class Math
    {
        /// <summary>
        /// 同じ長さ n の配列 <paramref name="r"/>, <paramref name="m"/> について、x≡<paramref name="r"/>[i] (mod <paramref name="m"/>[i]),∀i∈{0,1,⋯,n−1} を解きます。
        /// </summary>
        /// <remarks>
        /// <para>制約: |<paramref name="r"/>|=|<paramref name="m"/>|, 1≤<paramref name="m"/>[i], lcm(m[i]) が ll に収まる</para>
        /// <para>計算量: O(nloglcm(<paramref name="m"/>))</para>
        /// </remarks>
        /// <returns>答えは(存在するならば) y,z(0≤y&lt;z=lcm(<paramref name="m"/>[i])) を用いて x≡y(mod z) の形で書ける。答えがない場合は(0,0)、n=0 の時は(0,1)、それ以外の場合は(y,z)。</returns>
        public static (long, long) CRT(long[] r, long[] m) { throw new NotImplementedException(); }
    }
}
