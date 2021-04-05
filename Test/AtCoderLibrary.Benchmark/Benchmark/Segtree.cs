using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AtCoder;

public static class Segtree
{
    public static long Calc(int n)
    {
        long ans = 0;
        var seg = new Segtree<long, Op>(n);
        for (int i = 0; i < n; i++)
        {
            seg[i] = i;
        }
        for (int i = 0; 2 * i <= n; i++)
        {
            ans ^= seg[i..(2 * i)];
        }
        return ans;
    }
    struct Op : ISegtreeOperator<long>
    {
        public long Identity => 0;
        public long Operate(long x, long y) => x + y;
    }
}
