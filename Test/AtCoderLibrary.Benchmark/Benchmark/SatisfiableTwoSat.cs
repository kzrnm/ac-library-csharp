using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AtCoder;

public static class SatisfiableTwoSat
{
    public static long Calc(int n)
    {
        n >>= 1;
        long ans = 0;
        var twoSat = new TwoSat(n);
        for (int i = 0; i < n; i++)
        {
            twoSat.AddClause(i, true, (i + 1) % n, false);
        }
        var ok = twoSat.Satisfiable();
        ans = twoSat.Answer()[0] ? 1 : -1;
        return ans;
    }
}
