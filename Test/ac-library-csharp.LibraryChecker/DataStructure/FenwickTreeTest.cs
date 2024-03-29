﻿using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.DataStructure
{

    internal class FenwickTreeTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/point_add_range_sum";
        public override void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            int[] a = cr.Repeat(N);
#if NET7_0_OR_GREATER
            var fw = new FenwickTree<long>(N);
#else
            var fw = new LongFenwickTree(N);
#endif
            for (int i = 0; i < a.Length; i++)
            {
                fw.Add(i, a[i]);
            }
            for (int i = 0; i < Q; i++)
            {
                int t = cr;
                int l = cr;
                int r = cr;
                if (t == 0)
                    fw.Add(l, r);
                else
                    cw.WriteLine(fw[l..r]);
            }
        }
    }
}
