using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    /// <summary>
    /// 長さ N の配列に対し、
    /// <list type="bullet">
    /// <item>
    /// <description>要素の 1 点変更</description>
    /// </item>
    /// <item>
    /// <description>区間の要素の総和</description>
    /// </item>
    /// </list>
    /// <para>を O(log⁡N) で求めることが出来るデータ構造です。</para>
    /// </summary>
    /// <typeparam name="T">配列要素の型</typeparam>
    [DebuggerTypeProxy(typeof(FenwickTree<>.DebugView))]
    public class FenwickTree<T>
        where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly T[] data;

        public int Length { get; }

        /// <summary>
        /// 長さ <paramref name="n"/> の配列aを持つ <see cref="FenwickTree{TValue}"/> クラスの新しいインスタンスを作ります。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        /// <param name="n">配列の長さ</param>
        public FenwickTree(int n)
        {
            Length = n;
            data = new T[n + 1];
        }

        /// <summary>
        /// a[<paramref name="p"/>] += <paramref name="x"/> を行います。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="p"/>&lt;n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [MethodImpl(256)]
        public void Add(int p, T x)
        {
            Contract.Assert((uint)p < (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(p)} && {nameof(p)} < Length");
            for (++p; p < data.Length; p += (int)InternalBit.ExtractLowestSetBit(p))
            {
                data[p] += x;
            }
        }

        /// <summary>
        /// a[<paramref name="l"/>] + a[<paramref name="l"/> - 1] + ... + a[<paramref name="r"/> - 1] を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="l"/>≤<paramref name="r"/>≤n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        /// <returns>a[<paramref name="l"/>] + a[<paramref name="l"/> - 1] + ... + a[<paramref name="r"/> - 1]</returns>
        [MethodImpl(256)]
        public T Sum(int l, int r)
        {
            Contract.Assert((uint)l <= (uint)r && (uint)r <= (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(l)} && {nameof(l)} <= {nameof(r)} && {nameof(r)} <= Length");
            return Sum(r) - Sum(l);
        }

        [MethodImpl(256)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public T Sum(int r)
        {
            T s = T.AdditiveIdentity;
            for (; r > 0; r &= r - 1)
            {
                s += data[r];
            }
            return s;
        }

        [MethodImpl(256)]
        public T Slice(int l, int len) => Sum(l, l + len);

#if EMBEDDING
        [SourceExpander.NotEmbeddingSource]
#endif
        [DebuggerDisplay("Value = {" + nameof(Value) + "}, Sum = {" + nameof(Sum) + "}")]
        internal readonly struct DebugItem
        {
            public DebugItem(T value, T sum)
            {
                Value = value;
                Sum = sum;
            }
            public T Value { get; }
            public T Sum { get; }
        }
#if EMBEDDING
        [SourceExpander.NotEmbeddingSource]
#endif
        private class DebugView
        {
            private readonly FenwickTree<T> fw;
            public DebugView(FenwickTree<T> fenwickTree)
            {
                fw = fenwickTree;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugItem[] Items
            {
                get
                {
                    var data = fw.data;
                    var items = new DebugItem[data.Length - 1];
                    if (items.Length == 0) return System.Array.Empty<DebugItem>();

                    items[0] = new DebugItem(data[1], data[1]);
                    for (int i = 2; i < data.Length; i++)
                    {
                        int length = (int)InternalBit.ExtractLowestSetBit(i);
                        var pr = i - length - 1;
                        var sum = data[i] + (0 <= pr ? items[pr].Sum : T.AdditiveIdentity);
                        var val = sum - items[i - 2].Sum;
                        items[i - 1] = new DebugItem(val, sum);
                    }
                    return items;
                }
            }
        }
    }
}
