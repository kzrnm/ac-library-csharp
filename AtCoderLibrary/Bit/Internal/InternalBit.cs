using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace AtCoder.Internal
{
    public static class InternalBit
    {
        /// <summary>
        /// <paramref name="n"/> ≤ 2**x を満たす最小のx
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/></para>
        /// </remarks>
        public static int CeilPow2(int n)
        {
            var un = (uint)n;
            if (un <= 1) return 0;
            return BitOperations.Log2(un - 1) + 1;
        }

        /// <summary>
        /// _blsi_u32 OR <paramref name="n"/> &amp; -<paramref name="n"/>
        /// <para><paramref name="n"/>で立っているうちの最下位の 1 ビットのみを立てた整数を返す</para>
        /// </summary>
        /// <param name="n"></param>
        /// <returns><paramref name="n"/> &amp; -<paramref name="n"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ExtractLowestSetBit(int n)
        {
            if (Bmi1.IsSupported)
            {
                return (int)Bmi1.ExtractLowestSetBit((uint)n);
            }
            return n & -n;
        }

        /// <summary>
        /// (<paramref name="n"/> &amp; (1 &lt;&lt; x)) != 0 なる最小の非負整数 x を求めます。
        /// </summary>
        /// <remarks>
        /// <para>BSF: Bit Scan Forward</para>
        /// <para>制約: 1 ≤ <paramref name="n"/></para>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BSF(uint n)
        {
            Debug.Assert(n >= 1);
            return BitOperations.TrailingZeroCount(n);
        }
    }
}
