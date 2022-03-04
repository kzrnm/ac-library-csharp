﻿using System.Runtime.CompilerServices;
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
        {
            Contract.Assert(0 <= n && 1 <= m, reason: $"0 <= {nameof(n)} && 1 <= {nameof(m)}");
            if (m == 1) return 0;
            Barrett barrett = new Barrett((uint)m);
            uint r = 1, y = (uint)InternalMath.SafeMod(x, m);
            while (0 < n)
            {
                if ((n & 1) != 0) r = barrett.Mul(r, y);
                y = barrett.Mul(y, y);
                n >>= 1;
            }
            return r;
        }
    }
}
