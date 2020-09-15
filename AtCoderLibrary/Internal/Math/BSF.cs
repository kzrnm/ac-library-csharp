using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Diagnostics;

namespace AtCoder.Internal
{
    public static partial class InternalMath
    {
        /// <summary>
        /// (<paramref name="n"/> &amp; (1 &lt;&lt; x)) != 0 なる最小の非負整数 x を求めます。
        /// </summary>
        /// <remarks>
        /// <para>BSF: Bit Scan Forward</para>
        /// <para>制約: 1 ≤ <paramref name="n"/></para>
        /// </remarks>
        public static int BSF(uint n)
        {
            Debug.Assert(n >= 1);
            if (Bmi1.IsSupported)
            {
                // O(1)
                return (int)Bmi1.TrailingZeroCount(n);
            }
            else if (Popcnt.IsSupported)
            {
                // O(1)
                return (int)Popcnt.PopCount(~n & (n - 1));
            }
            else
            {
                // O(logn)
                return BitOperations.TrailingZeroCount(n);
            }
        }
    }
}

