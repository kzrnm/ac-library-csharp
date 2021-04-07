using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AtCoder;

public static class SCC
{
    public static long Calc(int n)
    {
        n >>= 3;
        long ans = 0;
        var graph = new SCCGraph(n);
        for (int i = 0; i < n; i++)
        {
            graph.AddEdge(i, (int)((1000000007L * i) % n));
        }
        ans = graph.SCC().Length;
        return ans;
    }
}
