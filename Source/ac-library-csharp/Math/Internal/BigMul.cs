using System.Runtime.CompilerServices;
#if NETCOREAPP3_1_OR_GREATER
using System.Runtime.Intrinsics.X86;
#endif

namespace AtCoder.Internal
{
    public static class Mul128
    {
        /// <summary>
        /// <paramref name="a"/> * <paramref name="b"/> の上位 64 ビットを返します。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [MethodImpl(256)]
        public static ulong Mul128Bit(ulong a, ulong b)
        {
#if NETCOREAPP3_1_OR_GREATER
            if (Bmi2.X64.IsSupported)
                return Bmi2.X64.MultiplyNoFlags(a, b);
#endif
            return Mul128BitLogic(a, b);
        }

        /// <summary>
        /// <paramref name="a"/> * <paramref name="b"/> の上位 64 ビットを返します。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [MethodImpl(256)]
        internal static ulong Mul128BitLogic(ulong a, ulong b)
        {
            /*
             * ここでは 64 bit 整数 X の上位, 下位 32 bit をそれぞれ Xu ,Xd と表す
             * A * B を計算する。
             * 
             * 変数を下記のように定義する。
             * s := 1ul << 32
             * R := A * B
             * L  := Ad * Bd
             * M1 := Ad * Bu
             * M2 := Au * Bd
             * H  := Au * Bu
             * 
             * A * B = s * s * H + s * (M1 + M2) + L
             * であることを使って
             * R = s * s * x + y
             * と表せる x, y を求めたい (x, y < s * s)
             * 
             * A * B
             * = s * s * H + s * (s * M1u + M1d + s * M2u + M2d) + L
             * = s * s * H + s * (s * (M1u + M2u) + M1d + M2d) + L
             * = s * s * (H + M1u + M2u) + s * (M1d + M2d) + L
             * 
             * と表せるので、繰り上がりを考慮しなければ
             * x = H + M1u + M2u
             * y = s * (M1d + M2d) + L = s * (M1d + M2d + Lu) + Ld
             * となる
             * 
             * 繰り上がるのは
             * M1d + M2d + Lu の上位 32 bit なので
             * C := M1d + M2d + Lu
             * とすると
             * 
             * x = H + M1u + M2u + Cu
             * となる
             */
            var au = a >> 32;
            var ad = a & 0xFFFFFFFF;
            var bu = b >> 32;
            var bd = b & 0xFFFFFFFF;

            var l = ad * bd;
            var m1 = au * bd;
            var m2 = ad * bu;
            var h = au * bu;

            var lu = l >> 32;
            var m1d = m1 & 0xFFFFFFFF;
            var m2d = m2 & 0xFFFFFFFF;
            var c = m1d + m2d + lu;

            return h + (m1 >> 32) + (m2 >> 32) + (c >> 32);
        }
    }
}
