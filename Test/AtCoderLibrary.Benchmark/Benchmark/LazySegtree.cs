using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AtCoder;

public static class LazySegtree
{
    public static long Calc(int n)
    {
        n >>= 3;
        long ans = 0;
        var seg = new LazySegtree<long, long, Op>(n);
        for (int i = 0; i < n; i++)
        {
            seg.Apply(i, Math.Min(2 * i, n), i);
        }
        for (int i = 0; 2 * i <= n; i++)
        {
            ans ^= seg[i..(2 * i)];
        }
        return ans;
    }
    struct Op : ILazySegtreeOperator<long, long>
    {
        public long Identity => 0;
        public long FIdentity => 0;
        public long Composition(long f, long g) => f + g;
        public long Mapping(long f, long x) => x + f;
        public long Operate(long x, long y) => Math.Max(x, y);
    }
}
