using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AtCoder;

public static class SuffixArrayString
{
    public static long Calc(int n)
    {
        long ans = 0;

        var c = new char[n];
        for (int i = 0; i < c.Length; i++)
            c[i] = (char)(i % 127);
        var s = new string(c);
        var sa = StringLib.SuffixArray(s);
        var lcp = StringLib.LCPArray(s, sa);
        ans = lcp[0];
        return ans;
    }
}
