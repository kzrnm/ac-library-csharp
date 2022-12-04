using Kzrnm.Competitive.IO;

namespace AtCoder.Solvers.DataStructure
{
    internal class LazySegtreeTest : BaseSover
    {
        public override string Url => "https://judge.yosupo.jp/problem/range_affine_range_sum";
        public override void Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            var seg = new LazySegtree<(uint v, uint len), (uint b, uint c), LazySegtreeSolverOp>(cr.Repeat(N).Select(cr => ((uint)cr.Int(), 1U)));
            for (int q = 0; q < Q; q++)
            {
                int t = cr;
                int l = cr;
                int r = cr;
                if (t == 0)
                {
                    uint b = (uint)cr.Int();
                    uint c = (uint)cr.Int();
                    seg.Apply(l, r, (b, c));
                }
                else
                    cw.WriteLine(seg[l..r].v);
            }
        }
    }
    struct LazySegtreeSolverOp : ILazySegtreeOperator<(uint v, uint len), (uint b, uint c)>
    {
        const uint MOD = 998244353;
        public (uint v, uint len) Operate((uint v, uint len) x, (uint v, uint len) y) => (SafeAdd(x.v, y.v), SafeAdd(x.len, y.len));
        public (uint v, uint len) Mapping((uint b, uint c) f, (uint v, uint len) x) => (SafeAdd(SafeMul(f.b, x.v), SafeMul(f.c, x.len)), x.len);
        public (uint b, uint c) Composition((uint b, uint c) f, (uint b, uint c) g) => (SafeMul(f.b, g.b), SafeAdd(SafeMul(f.b, g.c), f.c));
        public (uint v, uint len) Identity => (0, 0);
        public (uint b, uint c) FIdentity => (1, 0);
        static uint SafeAdd(uint a, uint b)
        {
            var r = a + b;
            return r < MOD ? r : r - MOD;
        }
        static uint SafeMul(uint a, uint b) => (uint)((ulong)a * b % MOD);
    }
}
