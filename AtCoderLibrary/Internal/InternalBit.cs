using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ExtractLowestSetBit(int n)
        {
            if (Bmi1.IsSupported)
            {
                return (int)Bmi1.ExtractLowestSetBit((uint)n);
            }
            return n & -n;
        }
    }
}
