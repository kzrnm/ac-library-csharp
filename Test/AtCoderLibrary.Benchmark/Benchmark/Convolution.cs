using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AtCoder;

public static class Convolution
{
    public static StaticModInt<Mod998244353> Calc(int n)
    {
        n >>= 2;
        var a = new StaticModInt<Mod998244353>[n];
        var b = new StaticModInt<Mod998244353>[n];
        for (int i = 0; i < n; i++)
        {
            a[i] = i + 1234;
            b[i] = i + 5678;
        }

        var c = MathLib.Convolution(a, b);
        return c[0];
    }
}
