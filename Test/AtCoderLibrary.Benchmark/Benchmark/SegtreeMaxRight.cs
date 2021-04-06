using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AtCoder;

public static class SegtreeMaxRight
{
    public static long Calc(int n)
    {
        n >>= 1;
        long ans = 0;
        var seg = new Segtree<long, Op>(n);
        for (int i = 0; i < n; i++)
        {
            seg[i] = i;
        }
        for (int i = 0; 2 * i <= n; i++)
        {
            ans ^= seg.MaxRight(i, l => l * 3 / 2 <= i);
        }
        return ans;
    }
    struct Op : ISegtreeOperator<long>
    {
        public long Identity => 0;
        public long Operate(long x, long y) => x + y;
    }
}
