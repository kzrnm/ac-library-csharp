#if GENERIC_MATH
using System.Numerics;
#endif

namespace AtCoder
{
    public interface IModInt<T>
#if GENERIC_MATH
     : INumberBase<T> where T : INumberBase<T>
#endif
    {
        /// <summary>
        /// 自身を x として、 xy≡1 なる y を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: gcd(x, mod) = 1</para>
        /// </remarks>
        T Inv();

        /// <summary>
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        T Pow(ulong n);
    }
}
