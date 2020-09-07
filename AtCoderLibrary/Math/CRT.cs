using System;
using System.Collections.Generic;
using System.Text;

namespace AtCoder
{
    public static partial class Math
    {
        /// <summary>
        /// 同じ長さの配列 r, m を渡します。この配列の長さを n とした時、x≡r[i] (mod m[i]),∀i∈{0,1,⋯,n−1} を解きます。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(nloglcm(<paramref name="m"/>))</para>
        /// </remarks>
        /// <returns>答えは(存在するならば) y,z(0≤y&lt;z=lcm(m[i])) を用いて x≡y(mod z) の形で書ける。答えがない場合は(0,0)、n=0 の時は(0,1)、それ以外の場合は(y,z)。</returns>
        public static (long, long) CRT(long[] r, long[] m) { throw new NotImplementedException(); }
    }
}
