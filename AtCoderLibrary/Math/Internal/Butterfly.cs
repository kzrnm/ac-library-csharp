using System;

namespace AtCoder.Internal
{
    public static class Butterfly<T> where T : struct, IStaticMod
    {
        /// <summary>
        /// sumE[i] = ies[0] * ... * ies[i - 1] * es[i]
        /// </summary>
        private static StaticModInt<T>[] sumE = CalcurateSumE();

        /// <summary>
        /// sumIE[i] = es[0] * ... * es[i - 1] * ies[i]
        /// </summary>
        private static StaticModInt<T>[] sumIE = CalcurateSumIE();

        public static void Calculate(Span<StaticModInt<T>> a)
        {
            var n = a.Length;
            var h = InternalBit.CeilPow2(n);

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

                    for (int i = 0; i < p; i++)
                    {
                        var l = a[i + offset];
                        var r = a[i + offset + p] * now;
                        a[i + offset] = l + r;
                        a[i + offset + p] = l - r;
                    }
                    now *= sumE[InternalBit.BSF(~(uint)s)];
                }
            }
        }

        public static void CalculateInv(Span<StaticModInt<T>> a)
        {
            var n = a.Length;
            var h = InternalBit.CeilPow2(n);

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

                    for (int i = 0; i < p; i++)
                    {
                        var l = a[i + offset];
                        var r = a[i + offset + p];
                        a[i + offset] = l + r;
                        a[i + offset + p] = StaticModInt<T>.Raw(
                            unchecked((int)((ulong)(default(T).Mod + l.Value - r.Value) * (ulong)iNow.Value % default(T).Mod)));
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

            var sumE = new StaticModInt<T>[cnt2 - 2];

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
            for (int i = 0; i < sumE.Length; i++)
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

            var sumIE = new StaticModInt<T>[cnt2 - 2];

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
            for (int i = 0; i < sumIE.Length; i++)
            {
                sumIE[i] = ies[i] * now;
                now *= es[i];
            }

            return sumIE;
        }
    }
}
