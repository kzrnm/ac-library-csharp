using System.Diagnostics;
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
    public class IntFenwickTree : FenwickTree<int, IntOperator> { public IntFenwickTree(int n) : base(n) { } }

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
    public class UIntFenwickTree : FenwickTree<uint, UIntOperator> { public UIntFenwickTree(int n) : base(n) { } }

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
    public class LongFenwickTree : FenwickTree<long, LongOperator> { public LongFenwickTree(int n) : base(n) { } }

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
    public class ULongFenwickTree : FenwickTree<ulong, ULongOperator> { public ULongFenwickTree(int n) : base(n) { } }

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
    public class ModIntFenwickTree<T> : FenwickTree<StaticModInt<T>, StaticModIntOperator<T>> where T : struct, IStaticMod { public ModIntFenwickTree(int n) : base(n) { } }


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
    /// <typeparam name="TOp">配列要素の操作を表す型</typeparam>
    [DebuggerTypeProxy(typeof(FenwickTree<,>.DebugView))]
    public class FenwickTree<TValue, TOp>
        where TOp : IAddOperator<TValue>, ISubtractOperator<TValue>
    {
        private static readonly TOp op = default;
        private readonly TValue[] data;

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
            Debug.Assert(unchecked((uint)n <= 100_000_000));
            data = new TValue[n + 1];
        }

        /// <summary>
        /// a[<paramref name="p"/>] += <paramref name="x"/> を行います。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="p"/>&lt;n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(int p, TValue x)
        {
            Debug.Assert(unchecked((uint)p < data.Length));
            for (p++; p < data.Length; p += InternalBit.ExtractLowestSetBit(p))
            {
                data[p] = op.Add(data[p], x);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue Sum(int l, int r)
        {
            Debug.Assert(0 <= l && l <= r && r < data.Length);
            return op.Subtract(Sum(r), Sum(l));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TValue Sum(int r)
        {
            TValue s = default;
            for (; r > 0; r -= InternalBit.ExtractLowestSetBit(r))
            {
                s = op.Add(s, data[r]);
            }
            return s;
        }


        [DebuggerDisplay("Value = {" + nameof(value) + "}, Sum = {" + nameof(sum) + "}")]
        internal struct DebugItem
        {
            public DebugItem(TValue value, TValue sum)
            {
                this.sum = sum;
                this.value = value;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public readonly TValue value;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public readonly TValue sum;
        }
        internal class DebugView
        {
            private readonly FenwickTree<TValue, TOp> fenwickTree;
            public DebugView(FenwickTree<TValue, TOp> fenwickTree)
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
                        int length = InternalBit.ExtractLowestSetBit(i);
                        var pr = i - length - 1;
                        var sum = op.Add(data[i], 0 <= pr ? items[pr].sum : default);
                        var val = op.Subtract(sum, items[i - 2].sum);
                        items[i - 1] = new DebugItem(val, sum);
                    }
                    return items;
                }
            }
        }
    }
}
