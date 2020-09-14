using System;
using AtCoder;

namespace AtCoder
{
    public static partial class Math
    {
        /// <summary>
        /// 畳み込みを mod <paramref name="m"/> で計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約：</para>
        /// <para>- 2≤<paramref name="m"/>≤2×10^9</para>
        /// <para>- <paramref name="m"/> は素数</para>
        /// <para>- 2^c | (<paramref name="m"/> - 1) かつ |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^c なる c が存在する</para>
        /// <para>計算量： O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|) + log<paramref name="m"/>)</para>
        /// </remarks>
        public static int[] Convolution(int[] a, int[] b, int m) => throw new NotImplementedException();

        /// <summary>
        /// 畳み込みを mod <paramref name="m"/> で計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約：</para>
        /// <para>- 2≤<paramref name="m"/>≤2×10^9</para>
        /// <para>- <paramref name="m"/> は素数</para>
        /// <para>- 2^c | (<paramref name="m"/> - 1) かつ |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^c なる c が存在する</para>
        /// <para>計算量： O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|) + log<paramref name="m"/>)</para>
        /// </remarks>
        public static uint[] Convolution(uint[] a, uint[] b, int m) => throw new NotImplementedException();

        /// <summary>
        /// 畳み込みを mod <paramref name="m"/> で計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約：</para>
        /// <para>- 2≤<paramref name="m"/>≤2×10^9</para>
        /// <para>- <paramref name="m"/> は素数</para>
        /// <para>- 2^c | (<paramref name="m"/> - 1) かつ |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^c なる c が存在する</para>
        /// <para>計算量： O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|) + log<paramref name="m"/>)</para>
        /// </remarks>
        public static long[] Convolution(long[] a, long[] b, int m) => throw new NotImplementedException();

        /// <summary>
        /// 畳み込みを mod <paramref name="m"/> で計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約：</para>
        /// <para>- 2≤<paramref name="m"/>≤2×10^9</para>
        /// <para>- <paramref name="m"/> は素数</para>
        /// <para>- 2^c | (<paramref name="m"/> - 1) かつ |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^c なる c が存在する</para>
        /// <para>計算量： O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|) + log<paramref name="m"/>)</para>
        /// </remarks>
        public static ulong[] Convolution(ulong[] a, ulong[] b, int m) => throw new NotImplementedException();

        /// <summary>
        /// 畳み込みを mod m（m は <see cref="DynamicModInt{T}.Mod"/>）で計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約：</para>
        /// <para>- 2≤m≤2×10^9</para>
        /// <para>- m は素数</para>
        /// <para>- 2^c | (m - 1) かつ |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^c なる c が存在する</para>
        /// <para>計算量： O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|) + logm)</para>
        /// </remarks>
        public static DynamicModInt<T>[] Convolution<T>(ReadOnlySpan<DynamicModInt<T>> a, ReadOnlySpan<DynamicModInt<T>> b)
            where T : struct, IDynamicModID
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 畳み込みを mod <typeparamref name="TMod"/> で計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約：</para>
        /// <para>- 2≤<typeparamref name="TMod"/>≤2×10^9</para>
        /// <para>- <typeparamref name="TMod"/> は素数</para>
        /// <para>- 2^c | (<typeparamref name="TMod"/> - 1) かつ |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^c なる c が存在する</para>
        /// <para>計算量： O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|) + log<typeparamref name="TMod"/>)</para>
        /// </remarks>
        public static StaticModInt<TMod>[] Convolution<TMod>(ReadOnlySpan<StaticModInt<TMod>> a, ReadOnlySpan<StaticModInt<TMod>> b)
            where TMod : struct, IStaticMod
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 畳み込みを計算します。
        /// </summary>
        /// <remarks>
        /// <para><paramref name="a"/>, <paramref name="b"/> の少なくとも一方が空の場合は空配列を返します。</para>
        /// <para>制約：</para>
        /// <para>- |<paramref name="a"/>| + |<paramref name="b"/>| - 1 ≤ 2^24</para>
        /// <para>- 畳み込んだ後の配列の要素が全て long に収まる</para>
        /// <para>計算量： O((|<paramref name="a"/>|+|<paramref name="b"/>|)log(|<paramref name="a"/>|+|<paramref name="b"/>|))</para>
        /// </remarks>
        public static long[] ConvolutionLong(ReadOnlySpan<long> a, ReadOnlySpan<long> b) => throw new NotImplementedException();

        private struct ConvModID : IDynamicModID { }
    }
}
