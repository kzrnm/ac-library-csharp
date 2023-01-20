﻿using System.Runtime.CompilerServices;
#if NETCOREAPP3_0_OR_GREATER
using System.Numerics;
using System.Runtime.Intrinsics.X86;
#endif

namespace AtCoder.Internal
{
    public static class InternalBit
    {

        /// <summary>
        /// _blsi_u32 OR <paramref name="n"/> &amp; -<paramref name="n"/>
        /// <para><paramref name="n"/>で立っているうちの最下位の 1 ビットのみを立てた整数を返す</para>
        /// </summary>
        /// <param name="n"></param>
        /// <returns><paramref name="n"/> &amp; -<paramref name="n"/></returns>
        [MethodImpl(256)]
        public static uint ExtractLowestSetBit(int n)
        {
#if NETCOREAPP3_0_OR_GREATER
            if (Bmi1.IsSupported)
            {
                return Bmi1.ExtractLowestSetBit((uint)n);
            }
#endif
            return (uint)(n & -n);
        }

        /// <summary>
        /// (<paramref name="n"/> &amp; (1 &lt;&lt; x)) != 0 なる最小の非負整数 x を求めます。
        /// </summary>
        /// <remarks>
        /// <para>BSF: Bit Scan Forward</para>
        /// <para>制約: 1 ≤ <paramref name="n"/></para>
        /// </remarks>
        [MethodImpl(256)]
        public static int BSF(uint n)
        {
            Contract.Assert(n > 0, reason: $"{nameof(n)} must positive");
            return BitOperations.TrailingZeroCount(n);
        }

        /// <summary>
        /// <paramref name="n"/> ≤ 2**x を満たす最小のx
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/></para>
        /// </remarks>
        [MethodImpl(256)]
        public static int CeilPow2(int n)
        {
            var un = (uint)n;
            if (un <= 1) return 0;
            return BitOperations.Log2(un - 1) + 1;
        }
    }
}
