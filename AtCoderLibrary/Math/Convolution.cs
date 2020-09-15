using System;
using System.Linq;
using AtCoder;

namespace AtCoder
{
    public static partial class Math
    {
        /// <summary>
        /// 畳み込みを mod <paramref name="m"/> = 998244353 で計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- 2≤<paramref name="m"/>≤2×10^9</para>
        /// <para>- <paramref name="m"/> は素数</para>
        /// <para>- 2^c | (<paramref name="m"/> - 1) かつ |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^c なる c が存在する</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|) + log<paramref name="m"/>)</para>
        /// </remarks>
        public static int[] Convolution(int[] a, int[] b) => Convolution<Mod998244353>(a, b);

        /// <summary>
        /// 畳み込みを mod <paramref name="m"/> = 998244353 で計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- 2≤<paramref name="m"/>≤2×10^9</para>
        /// <para>- <paramref name="m"/> は素数</para>
        /// <para>- 2^c | (<paramref name="m"/> - 1) かつ |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^c なる c が存在する</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|) + log<paramref name="m"/>)</para>
        /// </remarks>
        public static uint[] Convolution(uint[] a, uint[] b) => Convolution<Mod998244353>(a, b);

        /// <summary>
        /// 畳み込みを mod <paramref name="m"/> = 998244353 で計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- 2≤<paramref name="m"/>≤2×10^9</para>
        /// <para>- <paramref name="m"/> は素数</para>
        /// <para>- 2^c | (<paramref name="m"/> - 1) かつ |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^c なる c が存在する</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|) + log<paramref name="m"/>)</para>
        /// </remarks>
        public static long[] Convolution(long[] a, long[] b) => Convolution<Mod998244353>(a, b);

        /// <summary>
        /// 畳み込みを mod <paramref name="m"/> = 998244353 で計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約:</para>
        /// <para>- 2≤<paramref name="m"/>≤2×10^9</para>
        /// <para>- <paramref name="m"/> は素数</para>
        /// <para>- 2^c | (<paramref name="m"/> - 1) かつ |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^c なる c が存在する</para>
        /// <para>計算量: O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|) + log<paramref name="m"/>)</para>
        /// </remarks>
        public static ulong[] Convolution(ulong[] a, ulong[] b) => Convolution<Mod998244353>(a, b);

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
        public static int[] Convolution<TMod>(int[] a, int[] b) where TMod : struct, IStaticMod
        {
            var n = a.Length;
            var m = b.Length;
            if (n == 0 || m == 0)
            {
                return Array.Empty<int>();
            }

            if (System.Math.Min(n, m) <= 60)
            {
                var c = ConvolutionNaive<TMod>(a.Select(ai => new StaticModInt<TMod>(ai)).ToArray(),
                                               b.Select(bi => new StaticModInt<TMod>(bi)).ToArray());
                return c.Select(ci => ci.Value).ToArray();
            }
            else
            {
                int z = 1 << Internal.InternalMath.CeilPow2(n + m - 1);

                var aTemp = new StaticModInt<TMod>[z];
                for (int i = 0; i < a.Length; i++)
                {
                    aTemp[i] = new StaticModInt<TMod>(a[i]);
                }

                var bTemp = new StaticModInt<TMod>[z];
                for (int i = 0; i < b.Length; i++)
                {
                    bTemp[i] = new StaticModInt<TMod>(b[i]);
                }

                var c = Convolution<TMod>(aTemp, bTemp, n, m, z)[0..(n + m - 1)];
                var result = new int[c.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = c[i].Value;
                }
                return result;
            }
        }


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
        public static uint[] Convolution<TMod>(uint[] a, uint[] b) where TMod : struct, IStaticMod
        {
            var n = a.Length;
            var m = b.Length;
            if (n == 0 || m == 0)
            {
                return Array.Empty<uint>();
            }

            if (System.Math.Min(n, m) <= 60)
            {
                var c = ConvolutionNaive<TMod>(a.Select(ai => new StaticModInt<TMod>(ai)).ToArray(),
                                               b.Select(bi => new StaticModInt<TMod>(bi)).ToArray());
                return c.Select(ci => (uint)ci.Value).ToArray();
            }
            else
            {
                int z = 1 << Internal.InternalMath.CeilPow2(n + m - 1);

                var aTemp = new StaticModInt<TMod>[z];
                for (int i = 0; i < a.Length; i++)
                {
                    aTemp[i] = new StaticModInt<TMod>(a[i]);
                }

                var bTemp = new StaticModInt<TMod>[z];
                for (int i = 0; i < b.Length; i++)
                {
                    bTemp[i] = new StaticModInt<TMod>(b[i]);
                }

                var c = Convolution<TMod>(aTemp, bTemp, n, m, z)[0..(n + m - 1)];
                var result = new uint[c.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = (uint)c[i].Value;
                }
                return result;
            }
        }

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
        public static long[] Convolution<TMod>(long[] a, long[] b) where TMod : struct, IStaticMod
        {
            var n = a.Length;
            var m = b.Length;
            if (n == 0 || m == 0)
            {
                return Array.Empty<long>();
            }

            if (System.Math.Min(n, m) <= 60)
            {
                var c = ConvolutionNaive<TMod>(a.Select(ai => new StaticModInt<TMod>(ai)).ToArray(),
                                               b.Select(bi => new StaticModInt<TMod>(bi)).ToArray());
                return c.Select(ci => (long)ci.Value).ToArray();
            }
            else
            {
                int z = 1 << Internal.InternalMath.CeilPow2(n + m - 1);

                var aTemp = new StaticModInt<TMod>[z];
                for (int i = 0; i < a.Length; i++)
                {
                    aTemp[i] = new StaticModInt<TMod>(a[i]);
                }

                var bTemp = new StaticModInt<TMod>[z];
                for (int i = 0; i < b.Length; i++)
                {
                    bTemp[i] = new StaticModInt<TMod>(b[i]);
                }

                var c = Convolution<TMod>(aTemp, bTemp, n, m, z)[0..(n + m - 1)];
                var result = new long[c.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = c[i].Value;
                }
                return result;
            }
        }

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
        public static ulong[] Convolution<TMod>(ulong[] a, ulong[] b) where TMod : struct, IStaticMod
        {
            var n = a.Length;
            var m = b.Length;
            if (n == 0 || m == 0)
            {
                return Array.Empty<ulong>();
            }

            if (System.Math.Min(n, m) <= 60)
            {
                var c = ConvolutionNaive<TMod>(a.Select(TakeMod).ToArray(),
                                               b.Select(TakeMod).ToArray());
                return c.Select(ci => (ulong)ci.Value).ToArray();
            }
            else
            {
                int z = 1 << Internal.InternalMath.CeilPow2(n + m - 1);

                var aTemp = new StaticModInt<TMod>[z];
                for (int i = 0; i < a.Length; i++)
                {
                    aTemp[i] = TakeMod(a[i]);
                }

                var bTemp = new StaticModInt<TMod>[z];
                for (int i = 0; i < b.Length; i++)
                {
                    bTemp[i] = TakeMod(b[i]);
                }

                var c = Convolution<TMod>(aTemp, bTemp, n, m, z)[0..(n + m - 1)];
                var result = new ulong[c.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = (ulong)c[i].Value;
                }
                return result;
            }

            StaticModInt<TMod> TakeMod(ulong x) => StaticModInt<TMod>.Raw((int)(x % default(TMod).Mod));
        }

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
        public static StaticModInt<TMod>[] Convolution<TMod>(StaticModInt<TMod>[] a, StaticModInt<TMod>[] b)
            where TMod : struct, IStaticMod
        {
            var temp = Convolution((ReadOnlySpan<StaticModInt<TMod>>)a, b);
            return temp.ToArray();
        }

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
        public static Span<StaticModInt<TMod>> Convolution<TMod>(ReadOnlySpan<StaticModInt<TMod>> a, ReadOnlySpan<StaticModInt<TMod>> b)
            where TMod : struct, IStaticMod
        {
            var n = a.Length;
            var m = b.Length;
            if (n == 0 || m == 0)
            {
                return Array.Empty<StaticModInt<TMod>>();
            }

            if (System.Math.Min(n, m) <= 60)
            {
                return ConvolutionNaive(a, b);
            }

            int z = 1 << Internal.InternalMath.CeilPow2(n + m - 1);

            var aTemp = new StaticModInt<TMod>[z];
            a.CopyTo(aTemp);

            var bTemp = new StaticModInt<TMod>[z];
            b.CopyTo(bTemp);

            return Convolution(aTemp.AsSpan(), bTemp.AsSpan(), n, m, z);
        }

        private static Span<StaticModInt<TMod>> Convolution<TMod>(Span<StaticModInt<TMod>> a, Span<StaticModInt<TMod>> b, int n, int m, int z)
            where TMod : struct, IStaticMod
        {
            Internal.Butterfly<TMod>.Calculate(a);
            Internal.Butterfly<TMod>.Calculate(b);

            for (int i = 0; i < a.Length; i++)
            {
                a[i] *= b[i];
            }

            Internal.Butterfly<TMod>.CalculateInv(a);
            var result = a[0..(n + m - 1)];
            var iz = new StaticModInt<TMod>(z).Inv();
            foreach (ref var r in result)
            {
                r *= iz;
            }

            return result;
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

                const ulong Mod1 = 754974721;
                const ulong Mod2 = 167772161;
                const ulong Mod3 = 469762049;
                const ulong M2M3 = Mod2 * Mod3;
                const ulong M1M3 = Mod1 * Mod3;
                const ulong M1M2 = Mod1 * Mod2;
                // (m1 * m2 * m3) % 2^64
                const ulong M1M2M3 = Mod1 * Mod2 * Mod3;

                ulong i1 = (ulong)Internal.InternalMath.InvGCD((long)M2M3, (long)Mod1).Item2;
                ulong i2 = (ulong)Internal.InternalMath.InvGCD((long)M1M3, (long)Mod2).Item2;
                ulong i3 = (ulong)Internal.InternalMath.InvGCD((long)M1M2, (long)Mod3).Item2;

                var c1 = Convolution<FFTMod1>(a, b);
                var c2 = Convolution<FFTMod2>(a, b);
                var c3 = Convolution<FFTMod3>(a, b);

                var c = new long[n + m - 1];

                Span<ulong> offset = stackalloc ulong[] { 0, 0, M1M2M3, 2 * M1M2M3, 3 * M1M2M3 };

                for (int i = 0; i < c.Length; i++)
                {
                    ulong x = 0;
                    x += (c1[i] * i1) % Mod1 * M2M3;
                    x += (c2[i] * i2) % Mod2 * M1M3;
                    x += (c3[i] * i3) % Mod3 * M1M2;

                    long diff = (long)c1[i] - Internal.InternalMath.SafeMod((long)x, (long)Mod1);
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
                    x -= offset[(int)(diff % offset.Length)];
                    c[i] = (long)x;
                }

                return c;
            }


            ulong[] Convolution<TMod>(ReadOnlySpan<long> a, ReadOnlySpan<long> b) where TMod : struct, IStaticMod
            {
                int z = 1 << Internal.InternalMath.CeilPow2(a.Length + b.Length - 1);

                var aTemp = new StaticModInt<TMod>[z];
                for (int i = 0; i < a.Length; i++)
                {
                    aTemp[i] = new StaticModInt<TMod>(a[i]);
                }

                var bTemp = new StaticModInt<TMod>[z];
                for (int i = 0; i < b.Length; i++)
                {
                    bTemp[i] = new StaticModInt<TMod>(b[i]);
                }

                var c = AtCoder.Math.Convolution<TMod>(aTemp, bTemp, a.Length, b.Length, z);
                var result = new ulong[c.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = (ulong)c[i].Value;
                }

                return result;
            }
        }

        private static StaticModInt<TMod>[] ConvolutionNaive<TMod>(ReadOnlySpan<StaticModInt<TMod>> a, ReadOnlySpan<StaticModInt<TMod>> b)
            where TMod : struct, IStaticMod
        {
            if (a.Length < b.Length)
            {
                // ref 構造体のため型引数として使えない
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
