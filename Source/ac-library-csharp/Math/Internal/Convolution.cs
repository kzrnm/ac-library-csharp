using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
#if NETCOREAPP3_1_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace AtCoder.Internal
{
    public static class ConvolutionImpl
    {
        /// <summary>
        /// 畳み込みを mod <typeparamref name="TMod"/> で計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- 2≤<typeparamref name="TMod"/>≤2×10^9</para>
        /// <para>- <typeparamref name="TMod"/> は素数</para>
        /// <para>- 2^c | (<typeparamref name="TMod"/> - 1) かつ |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^c なる c が存在する</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|) + log<typeparamref name="TMod"/>)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static StaticModInt<TMod>[] Convolution<TMod>(ReadOnlySpan<StaticModInt<TMod>> a, ReadOnlySpan<StaticModInt<TMod>> b)
            where TMod : struct, IStaticMod
        {
            var n = a.Length;
            var m = b.Length;
            if (n == 0 || m == 0)
                return Array.Empty<StaticModInt<TMod>>();
            if (Math.Min(n, m) <= 60)
                return ConvolutionNaive(a, b);
            return ConvolutionFFT(a, b);
        }

        [MethodImpl(256)]
        private static StaticModInt<TMod>[] ConvolutionFFT<TMod>(ReadOnlySpan<StaticModInt<TMod>> a, ReadOnlySpan<StaticModInt<TMod>> b)
            where TMod : struct, IStaticMod
        {
            int n = a.Length, m = b.Length;
            int z = 1 << InternalBit.CeilPow2(n + m - 1);
            var rt = new StaticModInt<TMod>[z];
            var b2 = new StaticModInt<TMod>[z];
            a.CopyTo(rt);
            b.CopyTo(b2);

            ConvolutionFFTInner(rt, b2);
            Array.Resize(ref rt, n + m - 1);
            var iz = new StaticModInt<TMod>(z).Inv();
            for (int i = 0; i < rt.Length; i++)
                rt[i] *= iz;

            return rt;
        }

        [MethodImpl(256)]
        private static void ConvolutionFFTInner<TMod>(StaticModInt<TMod>[] a, Span<StaticModInt<TMod>> b)
            where TMod : struct, IStaticMod
        {
            Butterfly<TMod>.Calculate(a);
            Butterfly<TMod>.Calculate(b);
            for (int i = 0; i < a.Length; i++)
                a[i] *= b[i];
            Butterfly<TMod>.CalculateInv(a);
        }
        [MethodImpl(256)]
        private static StaticModInt<TMod>[] ConvolutionNaive<TMod>(ReadOnlySpan<StaticModInt<TMod>> a, ReadOnlySpan<StaticModInt<TMod>> b)
            where TMod : struct, IStaticMod
        {
            if (a.Length < b.Length)
            {
                var temp = a;
                a = b;
                b = temp;
            }

            var ans = new StaticModInt<TMod>[a.Length + b.Length - 1];
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    ans[i + j] += a[i] * b[j];
                }
            }

            return ans;
        }

        /// <summary>
        /// 畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24 = 16,777,216</para>
        /// <para>- 畳み込んだ後の配列の要素が全て long に収まる</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        [MethodImpl(256)]
        public static long[] ConvolutionLong(ReadOnlySpan<long> a, ReadOnlySpan<long> b)
        {
            unchecked
            {
                var n = a.Length;
                var m = b.Length;

                if (n == 0 || m == 0)
                {
                    return Array.Empty<long>();
                }

                const ulong Mod1 = 754974721;  // 2^24
                const ulong Mod2 = 167772161;  // 2^25
                const ulong Mod3 = 469762049;  // 2^26
                const ulong M2M3 = Mod2 * Mod3;
                const ulong M1M3 = Mod1 * Mod3;
                const ulong M1M2 = Mod1 * Mod2;
                // (m1 * m2 * m3) % 2^64
                const ulong M1M2M3 = Mod1 * Mod2 * Mod3;

                const ulong i1 = 190329765;
                const ulong i2 = 58587104;
                const ulong i3 = 187290749;

                Debug.Assert(default(FFTMod1).Mod == Mod1);
                Debug.Assert(default(FFTMod2).Mod == Mod2);
                Debug.Assert(default(FFTMod3).Mod == Mod3);
                Debug.Assert(i1 == (ulong)ModCalc.InvGcd((long)M2M3, (long)Mod1).Item2);
                Debug.Assert(i2 == (ulong)ModCalc.InvGcd((long)M1M3, (long)Mod2).Item2);
                Debug.Assert(i3 == (ulong)ModCalc.InvGcd((long)M1M2, (long)Mod3).Item2);

                var c1 = Convolution<FFTMod1>(ToModInt<FFTMod1>(a), ToModInt<FFTMod1>(b));
                var c2 = Convolution<FFTMod2>(ToModInt<FFTMod2>(a), ToModInt<FFTMod2>(b));
                var c3 = Convolution<FFTMod3>(ToModInt<FFTMod3>(a), ToModInt<FFTMod3>(b));

                var c = new long[n + m - 1];

                //ReadOnlySpan<ulong> offset = stackalloc ulong[] { 0, 0, M1M2M3, 2 * M1M2M3, 3 * M1M2M3 };

                for (int i = 0; i < c.Length; i++)
                {
                    ulong x = 0;
                    x += ((ulong)c1[i].Value * i1) % Mod1 * M2M3;
                    x += ((ulong)c2[i].Value * i2) % Mod2 * M1M3;
                    x += ((ulong)c3[i].Value * i3) % Mod3 * M1M2;

                    long diff = c1[i].Value - ModCalc.SafeMod((long)x, (long)Mod1);
                    if (diff < 0)
                    {
                        diff += (long)Mod1;
                    }

                    // 真値を r, 得られた値を x, M1M2M3 % 2^64 = M', B = 2^63 として、
                    // r = x,
                    //     x -  M' + (0 or 2B),
                    //     x - 2M' + (0 or 2B or 4B),
                    //     x - 3M' + (0 or 2B or 4B or 6B)
                    // のいずれかが成り立つ、らしい
                    // -> see atcoder/convolution.hpp
                    switch (diff % 5)
                    {
                        case 2:
                            x -= M1M2M3;
                            break;
                        case 3:
                            x -= 2 * M1M2M3;
                            break;
                        case 4:
                            x -= 3 * M1M2M3;
                            break;
                    }
                    c[i] = (long)x;
                }

                return c;
            }
        }
        static StaticModInt<T>[] ToModInt<T>(ReadOnlySpan<long> a) where T : struct, IStaticMod
        {
            if (a.IsEmpty) return Array.Empty<StaticModInt<T>>();

            var c = new StaticModInt<T>[a.Length];
#if NETCOREAPP3_1_OR_GREATER
            ref var ap = ref MemoryMarshal.GetReference(a);
            for (int i = 0; i < c.Length; i++)
                c[i] = Unsafe.Add(ref ap, i);
#else
            for (int i = 0; i < c.Length; i++)
                c[i] = a[i];
#endif
            return c;
        }

        private readonly struct FFTMod1 : IStaticMod
        {
            public uint Mod => 754974721;
            public bool IsPrime => true;
        }

        private readonly struct FFTMod2 : IStaticMod
        {
            public uint Mod => 167772161;
            public bool IsPrime => true;
        }

        private readonly struct FFTMod3 : IStaticMod
        {
            public uint Mod => 469762049;
            public bool IsPrime => true;
        }
    }
}
