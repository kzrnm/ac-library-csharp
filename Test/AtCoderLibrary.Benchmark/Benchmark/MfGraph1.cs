using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AtCoder;

public static class MfGraph1
{
    public static long Calc(int n)
    {
        n >>= 5;
        long ans = 0;
        var graph = new MFGraphInt(2 * n + 2);
        for (int i = 0; i < n; i++)
        {
            graph.AddEdge(2 * n, i, 1);
            graph.AddEdge(i, i + n, 1);
            graph.AddEdge(i + n, 2 * n + 1, 1);
        }
        ans = graph.Flow(2 * n, 2 * n + 1);
        return ans;
    }
}
