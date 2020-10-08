using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AtCoder.Internal
{
    public static class Butterfly<T> where T : struct, IStaticMod
    {
        /// <summary>
        /// sumE[i] = ies[0] * ... * ies[i - 1] * es[i]
        /// </summary>
        private static readonly StaticModInt<T>[] sumE = CalcurateSumE();

        /// <summary>
        /// sumIE[i] = es[0] * ... * es[i - 1] * ies[i]
        /// </summary>
        private static readonly StaticModInt<T>[] sumIE = CalcurateSumIE();

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void Calculate(Span<StaticModInt<T>> a)
        {
            CheckPow2(a.Length);
            var n = a.Length;
            var h = InternalBit.CeilPow2(n);

            var regLength = Vector<uint>.Count;

            // 全要素がmodのVector<uint>を作成（比較および加減算用）
            var modV = new Vector<uint>(default(T).Mod);

            for (int ph = 1; ph <= h; ph++)
            {
                // ブロックサイズの半分
                int w = 1 << (ph - 1);

                // ブロック数
                int p = 1 << (h - ph);

                var now = StaticModInt<T>.Raw(1);


                // 各ブロックの s 段目
                for (int s = 0; s < w; s++)
                {
                    int offset = s << (h - ph + 1);
                    var ls = a.Slice(offset, p);
                    var rs = a.Slice(offset + p, p);

                    if (p < regLength)
                    {
                        for (int i = 0; i < p; i++)
                        {
                            var l = ls[i];
                            var r = rs[i] * now;
                            ls[i] = l + r;
                            rs[i] = l - r;
                        }
                    }
                    else
                    {
                        foreach (ref var r in rs)
                        {
                            r *= now;
                        }

                        // uintとして再解釈
                        var lu = MemoryMarshal.Cast<StaticModInt<T>, uint>(ls);
                        var ru = MemoryMarshal.Cast<StaticModInt<T>, uint>(rs);

                        for (int i = 0; i < lu.Length; i += regLength)
                        {
                            var luSliced = lu.Slice(i);
                            var ruSliced = ru.Slice(i);
                            var u = new Vector<uint>(luSliced);
                            var v = new Vector<uint>(ruSliced);
                            var add = u + v;
                            var sub = u - v;

                            var ge = Vector.GreaterThanOrEqual(add, modV);
                            add = Vector.ConditionalSelect(ge, add - modV, add);

                            ge = Vector.GreaterThanOrEqual(sub, modV);
                            sub = Vector.ConditionalSelect(ge, sub + modV, sub);

                            add.CopyTo(luSliced);
                            sub.CopyTo(ruSliced);
                        }
                    }

                    now *= sumE[InternalBit.BSF(~(uint)s)];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void CalculateInv(Span<StaticModInt<T>> a)
        {
            CheckPow2(a.Length);
            var n = a.Length;
            var h = InternalBit.CeilPow2(n);

            var regLength = Vector<uint>.Count;

            // 全要素がmodのVector<uint>を作成（比較および加減算用）
            var modV = new Vector<uint>(default(T).Mod);

            for (int ph = h; ph >= 1; ph--)
            {
                // ブロックサイズの半分
                int w = 1 << (ph - 1);

                // ブロック数
                int p = 1 << (h - ph);

                var iNow = StaticModInt<T>.Raw(1);

                // 各ブロックの s 段目
                for (int s = 0; s < w; s++)
                {
                    int offset = s << (h - ph + 1);

                    var ls = a.Slice(offset, p);
                    var rs = a.Slice(offset + p, p);

                    if (p < regLength)
                    {
                        for (int i = 0; i < p; i++)
                        {
                            var l = ls[i];
                            var r = rs[i];
                            ls[i] = l + r;
                            rs[i] = StaticModInt<T>.Raw(
                                (int)((ulong)(default(T).Mod + l.Value - r.Value) * (ulong)iNow.Value % default(T).Mod));
                        }
                    }
                    else
                    {
                        // uintとして再解釈
                        var lu = MemoryMarshal.Cast<StaticModInt<T>, uint>(ls);
                        var ru = MemoryMarshal.Cast<StaticModInt<T>, uint>(rs);

                        for (int i = 0; i < lu.Length; i += regLength)
                        {
                            var luSliced = lu.Slice(i);
                            var ruSliced = ru.Slice(i);
                            var u = new Vector<uint>(luSliced);
                            var v = new Vector<uint>(ruSliced);
                            var add = u + v;
                            var sub = u - v;

                            var ge = Vector.GreaterThanOrEqual(add, modV);
                            add = Vector.ConditionalSelect(ge, add - modV, add);

                            // こちらは後で余りを取るのでマスク不要
                            sub += modV;

                            add.CopyTo(luSliced);
                            sub.CopyTo(ruSliced);
                        }

                        foreach (ref var r in rs)
                        {
                            r *= iNow;
                        }
                    }
                    iNow *= sumIE[InternalBit.BSF(~(uint)s)];
                }
            }
        }

        private static StaticModInt<T>[] CalcurateSumE()
        {
            int g = InternalMath.PrimitiveRoot((int)default(T).Mod);
            int cnt2 = InternalBit.BSF(default(T).Mod - 1);
            var e = new StaticModInt<T>(g).Pow((default(T).Mod - 1) >> cnt2);
            var ie = e.Inv();

            var sumE = new StaticModInt<T>[30];

            // es[i]^(2^(2+i)) == 1
            Span<StaticModInt<T>> es = stackalloc StaticModInt<T>[cnt2 - 1];
            Span<StaticModInt<T>> ies = stackalloc StaticModInt<T>[cnt2 - 1];

            for (int i = es.Length - 1; i >= 0; i--)
            {
                // e^(2^(2+i)) == 1
                es[i] = e;
                ies[i] = ie;
                e *= e;
                ie *= ie;
            }

            var now = StaticModInt<T>.Raw(1);
            for (int i = 0; i <= cnt2 - 2; i++)
            {
                sumE[i] = es[i] * now;
                now *= ies[i];
            }

            return sumE;
        }

        private static StaticModInt<T>[] CalcurateSumIE()
        {
            int g = InternalMath.PrimitiveRoot((int)default(T).Mod);
            int cnt2 = InternalBit.BSF(default(T).Mod - 1);
            var e = new StaticModInt<T>(g).Pow((default(T).Mod - 1) >> cnt2);
            var ie = e.Inv();

            var sumIE = new StaticModInt<T>[30];

            // es[i]^(2^(2+i)) == 1
            Span<StaticModInt<T>> es = stackalloc StaticModInt<T>[cnt2 - 1];
            Span<StaticModInt<T>> ies = stackalloc StaticModInt<T>[cnt2 - 1];

            for (int i = es.Length - 1; i >= 0; i--)
            {
                // e^(2^(2+i)) == 1
                es[i] = e;
                ies[i] = ie;
                e *= e;
                ie *= ie;
            }

            var now = StaticModInt<T>.Raw(1);
            for (int i = 0; i <= cnt2 - 2; i++)
            {
                sumIE[i] = ies[i] * now;
                now *= es[i];
            }

            return sumIE;
        }

        [Conditional("DEBUG")]
        private static void CheckPow2(int n)
        {
            if (BitOperations.PopCount((uint)n) != 1)
            {
                throw new ArgumentException("配列長は2のべき乗でなければなりません。");
            }
        }
    }
}
