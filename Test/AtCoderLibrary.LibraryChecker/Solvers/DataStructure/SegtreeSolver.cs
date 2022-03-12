﻿using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.DataStructure
{
    public class SegtreeSolver : Solver
    {
        public override string Name => "point_set_range_composite";
        public override double TimeoutSecond => 5;
        public override void Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int n = cr;
            int q = cr;

            var seg = new Segtree<(StaticModInt<Mod998244353> a, StaticModInt<Mod998244353> b), SegtreeSolverOp>(
                cr.Repeat(n).Select(cr => (StaticModInt<Mod998244353>.Raw(cr), StaticModInt<Mod998244353>.Raw(cr))));
            for (int i = 0; i < q; i++)
            {
                int t = cr;
                if (t == 0)
                {
                    int p = cr;
                    int c = cr;
                    int d = cr;
                    seg[p] = (StaticModInt<Mod998244353>.Raw(c), StaticModInt<Mod998244353>.Raw(d));
                }
                else
                {
                    int l = cr;
                    int r = cr;
                    int x = cr;
                    var (a, b) = seg[l..r];
                    cw.WriteLine(a * x + b);
                }
            }
        }
    }
    struct SegtreeSolverOp : ISegtreeOperator<(StaticModInt<Mod998244353> a, StaticModInt<Mod998244353> b)>
    {
        public (StaticModInt<Mod998244353> a, StaticModInt<Mod998244353> b)
            Operate((StaticModInt<Mod998244353> a, StaticModInt<Mod998244353> b) x, (StaticModInt<Mod998244353> a, StaticModInt<Mod998244353> b) y)
            => (x.a * y.a, y.a * x.b + y.b);

        public (StaticModInt<Mod998244353> a, StaticModInt<Mod998244353> b) Identity => (StaticModInt<Mod998244353>.Raw(1), new StaticModInt<Mod998244353>());
    }
}
