using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    /// <summary>
    /// モノイドを定義するインターフェイスです。
    /// </summary>
    /// <typeparam name="T">操作を行う型。</typeparam>
    public interface IMonoidOperator<T>
    {
        /// <summary>
        /// Operate(x, <paramref name="Identity"/>) = xを満たす単位元。
        /// </summary>
        T Identity { get; }
        /// <summary>
        /// 結合律 Operate(Operate(a, b), c) = Operate(a, Operate(b, c)) を満たす写像。
        /// </summary>
        T Operate(T x, T y);
    }

    /// <summary>
    /// 長さ N の配列に対し、
    /// <list type="bullet">
    /// <item>
    /// <description>要素の 1 点変更</description>
    /// </item>
    /// <item>
    /// <description>区間の要素の総積の取得</description>
    /// </item>
    /// </list>
    /// <para>を O(log N) で求めることが出来るデータ構造です。</para>
    /// </summary>
    [DebuggerTypeProxy(typeof(Segtree<,>.DebugView))]
    public class Segtree<TValue, TOp> where TOp : struct, IMonoidOperator<TValue>
    {
        private static readonly TOp op = default;

        /// <summary>
        /// 数列 a の長さ n を返します。
        /// </summary>
        public int Length { get; }

        private readonly int log;
        private readonly int size;
        private readonly TValue[] d;


        /// <summary>
        /// 長さ <paramref name="n"/> の数列 a　を持つ <see cref="Segtree{TValue, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <see cref="TOp.Identity"/> です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        /// <param name="n">配列の長さ</param>
        public Segtree(int n)
        {
            Debug.Assert(0 <= n);
            AssertMonoid(op.Identity);
            Length = n;
            log = InternalBit.CeilPow2(n);
            size = 1 << log;
            d = new TValue[2 * size];
            Array.Fill(d, op.Identity);
        }

        /// <summary>
        /// 長さ n=<paramref name="v"/>.Length の数列 a　を持つ <see cref="Segtree{TValue, TOp}"/> クラスの新しいインスタンスを作ります。初期値は <paramref name="v"/> です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        /// <param name="n">配列の長さ</param>
        public Segtree(TValue[] v) : this(v.Length)
        {
            for (int i = 0; i < v.Length; i++) d[size + i] = v[i];
            for (int i = size - 1; i >= 1; i--)
            {
                Update(i);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Update(int k) => d[k] = op.Operate(d[2 * k], d[2 * k + 1]);

        /// <summary>
        /// a[<paramref name="p"/>] を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="p"/>&lt;n</para>
        /// <para>計算量(set): O(log n)</para>
        /// <para>計算量(get): O(1)</para>
        /// </remarks>
        /// <returns></returns>
        public TValue this[int p]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                AssertMonoid(value);
                Debug.Assert((uint)p < Length);
                p += size;
                d[p] = value;
                for (int i = 1; i <= log; i++) Update(p >> i);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                Debug.Assert((uint)p < Length);
                AssertMonoid(d[p + size]);
                return d[p + size];
            }
        }

        /// <summary>
        /// <see cref="TOp.Operate"/>(a[<paramref name="l"/>], ..., a[<paramref name="r"/> - 1]) を返します。<paramref name="l"/> = <paramref name="r"/> のときは　<see cref="TOp.Identity"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="l"/>≤<paramref name="r"/>≤n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        /// <returns><see cref="TOp.Operate"/>(a[<paramref name="l"/>], ..., a[<paramref name="r"/> - 1])</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue Prod(int l, int r)
        {
            Debug.Assert(0 <= l && l <= r && r <= Length);
            TValue sml = op.Identity, smr = op.Identity;
            l += size;
            r += size;

            while (l < r)
            {
                if ((l & 1) != 0) sml = op.Operate(sml, d[l++]);
                if ((r & 1) != 0) smr = op.Operate(d[--r], smr);
                l >>= 1;
                r >>= 1;
            }
            AssertMonoid(op.Operate(sml, smr));
            return op.Operate(sml, smr);
        }

        /// <summary>
        /// <see cref="TOp.Operate"/>(a[0], ..., a[n - 1]) を返します。n = 0 のときは　<see cref="TOp.Identity"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        /// <returns><see cref="TOp.Operate"/>(a[0], ..., a[n - 1])</returns>
        public TValue AllProd => d[1];

        /// <summary>
        /// 以下の条件を両方満たす r を(いずれか一つ)返します。
        /// <list type="bullet">
        /// <item>
        /// <description>r = <paramref name="l"/> もしくは <paramref name="f"/>(op(a[<paramref name="l"/>], a[<paramref name="l"/> + 1], ..., a[r - 1])) = true</description>
        /// </item>
        /// <item>
        /// <description>r = n もしくは <paramref name="f"/>(op(a[<paramref name="l"/>], a[<paramref name="l"/> + 1], ..., a[r])) = false</description>
        /// </item>
        /// </list>
        /// <para><paramref name="f"/> が単調だとすれば、<paramref name="f"/>(op(a[<paramref name="l"/>], a[<paramref name="l"/> + 1], ..., a[r - 1])) = true となる最大の r、と解釈することが可能です。</para>
        /// </summary>
        /// <remarks>
        /// 制約
        /// <list type="bullet">
        /// <item>
        /// <description><paramref name="f"/> を同じ引数で呼んだ時、返り値は等しい(=副作用はない)。</description>
        /// </item>
        /// <item>
        /// <description><paramref name="f"/>(<see cref="TOp.Identity"/>) = true</description>
        /// </item>
        /// <item>
        /// <description>0≤<paramref name="l"/>≤n</description>
        /// </item>
        /// </list>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        public int MaxRight(int l, Predicate<TValue> f)
        {
            Debug.Assert((uint)l <= Length);
            Debug.Assert(f(op.Identity));
            if (l == Length) return Length;
            l += size;
            var sm = op.Identity;
            do
            {
                while (l % 2 == 0) l >>= 1;
                if (!f(op.Operate(sm, d[l])))
                {
                    while (l < size)
                    {
                        l = (2 * l);
                        if (f(op.Operate(sm, d[l])))
                        {
                            sm = op.Operate(sm, d[l]);
                            l++;
                        }
                    }
                    return l - size;
                }
                sm = op.Operate(sm, d[l]);
                l++;
            } while ((l & -l) != l);
            return Length;
        }

        /// <summary>
        /// 以下の条件を両方満たす r を(いずれか一つ)返します。
        /// <list type="bullet">
        /// <item>
        /// <description>l = <paramref name="r"/> もしくは <paramref name="f"/>(op(a[l], a[l + 1], ..., a[<paramref name="r"/> - 1])) = true</description>
        /// </item>
        /// <item>
        /// <description>l = 0 もしくは <paramref name="f"/>(op(a[l - 1], a[l], ..., a[<paramref name="r"/> - 1])) = false</description>
        /// </item>
        /// </list>
        /// <para><paramref name="f"/> が単調だとすれば、<paramref name="f"/>(op(a[l], a[l + 1], ..., a[<paramref name="r"/> - 1])) = true となる最小の l、と解釈することが可能です。</para>
        /// </summary>
        /// <remarks>
        /// 制約
        /// <list type="bullet">
        /// <item>
        /// <description><paramref name="f"/> を同じ引数で呼んだ時、返り値は等しい(=副作用はない)。</description>
        /// </item>
        /// <item>
        /// <description><paramref name="f"/>(<see cref="TOp.Identity"/>) = true</description>
        /// </item>
        /// <item>
        /// <description>0≤<paramref name="r"/>≤n</description>
        /// </item>
        /// </list>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        public int MinLeft(int r, Predicate<TValue> f)
        {
            Debug.Assert((uint)r <= Length);
            Debug.Assert(f(op.Identity));
            if (r == 0) return 0;
            r += size;
            var sm = op.Identity;
            do
            {
                r--;
                while (r > 1 && (r % 2) != 0) r >>= 1;
                if (!f(op.Operate(d[r], sm)))
                {
                    while (r < size)
                    {
                        r = (2 * r + 1);
                        if (f(op.Operate(d[r], sm)))
                        {
                            sm = op.Operate(d[r], sm);
                            r--;
                        }
                    }
                    return r + 1 - size;
                }
                sm = op.Operate(d[r], sm);
            } while ((r & -r) != r);
            return 0;
        }


        [DebuggerDisplay("{" + nameof(value) + "}", Name = "{" + nameof(key) + ",nq}")]
        private struct DebugItem
        {
            public DebugItem(int l, int r, TValue value)
            {
                if (r - l == 1)
                    key = $"[{l}]";
                else
                    key = $"[{l}-{r})";
                this.value = value;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly string key;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly TValue value;
        }
        private class DebugView
        {
            private readonly Segtree<TValue, TOp> segtree;
            public DebugView(Segtree<TValue, TOp> segtree)
            {
                this.segtree = segtree;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugItem[] Items
            {
                get
                {
                    var items = new List<DebugItem>(segtree.Length);
                    for (int len = segtree.size; len > 0; len >>= 1)
                    {
                        int unit = segtree.size / len;
                        for (int i = 0; i < len; i++)
                        {
                            int l = i * unit;
                            int r = Math.Min(l + unit, segtree.Length);
                            if (l < segtree.Length)
                                items.Add(new DebugItem(l, r, segtree.d[i + len]));
                        }
                    }
                    return items.ToArray();
                }
            }
        }

        /// <summary>
        /// Debug ビルドにおいて、Monoid が正しいかチェックする。
        /// </summary>
        /// <param name="value"></param>
        [Conditional("DEBUG")]
        public static void AssertMonoid(TValue value)
        {
            Debug.Assert(op.Operate(value, op.Identity).Equals(value),
                $"{nameof(op.Operate)}({value}, {op.Identity}) != {value}");
            Debug.Assert(op.Operate(op.Identity, value).Equals(value),
                $"{nameof(op.Operate)}({op.Identity}, {value}) != {value}");
        }
    }
}
