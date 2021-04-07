using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AtCoder;

public static class Convolution377487361
{
    private struct Mod377487361 : IStaticMod
    {
        public uint Mod => 377487361;
        public bool IsPrime => true;
    }
    public static long Calc(int n)
    {
        n >>= 2;
        var a = new StaticModInt<Mod377487361>[n];
        var b = new StaticModInt<Mod377487361>[n];
        for (int i = 0; i < n; i++)
        {
            a[i] = i + 1234;
            b[i] = i + 5678;
        }

        var c = MathLib.Convolution(a, b);
        return c[0].Value;
    }
}
