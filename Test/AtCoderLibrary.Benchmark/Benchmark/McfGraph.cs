using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AtCoder;

public static class McfGraph
{
    public static long Calc(int n)
    {
        n >>= 12;
        long ans = 0;
        var graph = new McfGraphLong(2 * n + 2);
        for (int i = 0; i < n; i++)
        {
            graph.AddEdge(2 * n, i, n - i, n + i);
            graph.AddEdge(i, i + n, n, n);
            graph.AddEdge(i + n, 2 * n + 1, i, n + i);
        }
        ans = graph.Flow(2 * n, 2 * n + 1).cost;
        return ans;
    }
}
