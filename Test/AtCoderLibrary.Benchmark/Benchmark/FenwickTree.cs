using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AtCoder;

public static class FenwickTree
{
    public static long Calc(int n)
    {
        n <<= 1;
        long ans = 0;
        var fw = new LongFenwickTree(n);
        for (int i = 0; i < n; i++)
        {
            fw.Add(i, i + 1234);
        }
        for (int i = 0; 2 * i <= n; i++)
        {
            ans ^= fw[i..(2 * i)];
        }
        return ans;
    }
}
