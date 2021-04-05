using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AtCoder;

public static class ZAlgorithm
{
    public static long Calc(int n)
    {
        long ans = 0;

        n <<= 3;
        var s = new long[n];
        for (int i = 0; i < s.Length; i++)
            s[i] = long.MaxValue + i % 2 == 0 ? i : -i;
        var z = StringLib.ZAlgorithm(s);
        ans = z[0];
        return ans;
    }
}
