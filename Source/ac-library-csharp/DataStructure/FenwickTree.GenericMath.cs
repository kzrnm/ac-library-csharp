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
    /// <typeparam name="TValue">配列要素の型</typeparam>
    [DebuggerTypeProxy(typeof(FenwickTree<>.DebugView))]
    public class FenwickTree<TValue>
        where TValue : IAdditionOperators<TValue, TValue, TValue>, ISubtractionOperators<TValue, TValue, TValue>, IAdditiveIdentity<TValue, TValue>
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly TValue[] data;

        public int Length { get; }

        /// <summary>
        /// 長さ <paramref name="n"/> の配列aを持つ <see cref="FenwickTree{TValue, TOp}"/> クラスの新しいインスタンスを作ります。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        /// <param name="n">配列の長さ</param>
        public FenwickTree(int n)
        {
            Length = n;
            data = new TValue[n + 1];
        }

        /// <summary>
        /// a[<paramref name="p"/>] += <paramref name="x"/> を行います。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="p"/>&lt;n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [MethodImpl(256)]
        public void Add(int p, TValue x)
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
        public TValue Sum(int l, int r)
        {
            Contract.Assert(0U <= (uint)l && (uint)l <= (uint)r && (uint)r <= (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(l)} && {nameof(l)} <= {nameof(r)} && {nameof(r)} <= Length");
            return Sum(r) - Sum(l);
        }

        [MethodImpl(256)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TValue Sum(int r)
        {
            TValue s = TValue.AdditiveIdentity;
            for (; r > 0; r &= r - 1)
            {
                s += data[r];
            }
            return s;
        }

        [MethodImpl(256)]
        public TValue Slice(int l, int len) => Sum(l, l + len);

        [DebuggerDisplay("Value = {" + nameof(value) + "}, Sum = {" + nameof(sum) + "}")]
        internal struct DebugItem
        {
            public DebugItem(TValue value, TValue sum)
            {
                this.sum = sum;
                this.value = value;
            }
            public readonly TValue value;
            public readonly TValue sum;
        }
        internal class DebugView
        {
            private readonly FenwickTree<TValue> fenwickTree;
            public DebugView(FenwickTree<TValue> fenwickTree)
            {
                this.fenwickTree = fenwickTree;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugItem[] Items
            {
                get
                {
                    var data = fenwickTree.data;
                    var items = new DebugItem[data.Length - 1];
                    items[0] = new DebugItem(data[1], data[1]);
                    for (int i = 2; i < data.Length; i++)
                    {
                        int length = (int)InternalBit.ExtractLowestSetBit(i);
                        var pr = i - length - 1;
                        var sum = data[i] + (0 <= pr ? items[pr].sum : TValue.AdditiveIdentity);
                        var val = sum - items[i - 2].sum;
                        items[i - 1] = new DebugItem(val, sum);
                    }
                    return items;
                }
            }
        }
    }
}
