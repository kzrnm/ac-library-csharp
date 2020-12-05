# ac-library-csharp

README languages: [Japanese](README.ja.md)

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
## Table of Contents

- [Packages](#packages)
  - [ac-library-csharp](#ac-library-csharp)
- [Status](#status)
- [Getting started](#getting-started)
  - [Installation](#installation)
  - [Install analyzer(optional)](#install-analyzeroptional)
  - [output combinded source code](#output-combinded-source-code)
- [License](#license)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Packages

### ac-library-csharp

C# port of [AtCoder Library](https://github.com/atcoder/ac-library/)

## Status

[![NuGet version (ac-library-csharp)](https://img.shields.io/nuget/v/ac-library-csharp.svg?style=flat-square)](https://www.nuget.org/packages/ac-library-csharp/)
![test](https://github.com/naminodarie/ac-library-csharp/workflows/test/badge.svg?branch=master)

## Getting started

### Installation

```
Install-Package ac-library-csharp
```

### Install analyzer(optional)

```
Install-Package AtCoderAnalyzer
```

### output combinded source code

Output combinded source code for submitting.

Require **.NET 5 SDK** or **Visual Studio 16.8** or later because SourceExpander use Source Generators.

```
Install-Package SourceExpander
```

When you run `/home/any_directory/ac-library-csharp/Sample/SampleProject/Program.cs`, `SourceExpander.Expander.Expand()` creates `/home/any_directory/ac-library-csharp/Sample/SampleProject/Combined.csx`

------

Program.cs

```C#
using System;
using AtCoder;

class Program
{
    static void Main()
    {
        SourceExpander.Expander.Expand();

        // https://atcoder.jp/contests/practice2/tasks/practice2_a
        var line = Console.ReadLine().Split(' ');
        var n = int.Parse(line[0]);
        var Q = int.Parse(line[1]);

        var dsu = new DSU(n);

        for (int i = 0; i < Q; i++)
        {
            line = Console.ReadLine().Split(' ');
            int t = int.Parse(line[0]);
            int u = int.Parse(line[1]);
            int v = int.Parse(line[2]);
            if (t == 0)
            {
                dsu.Merge(u, v);
            }
            else
            {
                Console.WriteLine(dsu.Same(u, v) ? 1 : 0);
            }
        }
    }
}
```

Combined.csx

```C#
using AtCoder;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
class Program
{
    static void Main()
    {
        SourceExpander.Expander.Expand();

        // https://atcoder.jp/contests/practice2/tasks/practice2_j
        var line = Console.ReadLine().Split(' ');
        var N = int.Parse(line[0]);
        var Q = int.Parse(line[1]);
        var seg = new Segtree<long, Op>(Console.ReadLine().Split(' ').Select(long.Parse).ToArray());

        for (int q = 0; q < Q; q++)
        {
            line = Console.ReadLine().Split(' ');
            int t = int.Parse(line[0]);
            int l = int.Parse(line[1]) - 1;
            int r = int.Parse(line[2]);
            switch (t)
            {
                case 1:
                    seg[l] = r;
                    break;
                case 2:
                    Console.WriteLine(seg.Prod(l, r));
                    break;
                case 3:
                    Console.WriteLine(1 + seg.MaxRight(l, num => num < r));
                    break;
            }
        }
    }
}
struct Op : ISegtreeOperator<long>
{
    public long Identity => long.MinValue;
    public long Operate(long x, long y) => Math.Max(x, y);
}
#region Expanded
namespace AtCoder { public interface ISegtreeOperator<T> { T Identity { get; } T Operate(T x, T y); } [DebuggerTypeProxy(typeof(Segtree<,>.DebugView))] public class Segtree<TValue, TOp> where TOp : struct, ISegtreeOperator<TValue> { private static readonly TOp op = default; public int Length { get; } internal readonly int log; internal readonly int size; internal readonly TValue[] d; public Segtree(int n) { DebugUtil.Assert(0 <= n); AssertMonoid(op.Identity); Length = n; log = InternalBit.CeilPow2(n); size = 1 << log; d = new TValue[2 * size]; Array.Fill(d, op.Identity); } public Segtree(TValue[] v) : this(v.Length) { for (int i = 0; i < v.Length; i++) d[size + i] = v[i]; for (int i = size - 1; i >= 1; i--) { Update(i); } } [MethodImpl(MethodImplOptions.AggressiveInlining)] internal void Update(int k) => d[k] = op.Operate(d[2 * k], d[2 * k + 1]); public TValue this[int p] { [MethodImpl(MethodImplOptions.AggressiveInlining)] set { AssertMonoid(value); DebugUtil.Assert((uint)p < Length); p += size; d[p] = value; for (int i = 1; i <= log; i++) Update(p >> i); } [MethodImpl(MethodImplOptions.AggressiveInlining)] get { DebugUtil.Assert((uint)p < Length); AssertMonoid(d[p + size]); return d[p + size]; } } [MethodImpl(MethodImplOptions.AggressiveInlining)] public TValue Slice(int l, int len) => Prod(l, l + len); [MethodImpl(MethodImplOptions.AggressiveInlining)] public TValue Prod(int l, int r) { DebugUtil.Assert(0 <= l && l <= r && r <= Length); TValue sml = op.Identity, smr = op.Identity; l += size; r += size; while (l < r) { if ((l & 1) != 0) sml = op.Operate(sml, d[l++]); if ((r & 1) != 0) smr = op.Operate(d[--r], smr); l >>= 1; r >>= 1; } AssertMonoid(op.Operate(sml, smr)); return op.Operate(sml, smr); } public TValue AllProd => d[1]; public int MaxRight(int l, Predicate<TValue> f) { DebugUtil.Assert((uint)l <= Length); DebugUtil.Assert(f(op.Identity)); if (l == Length) return Length; l += size; var sm = op.Identity; do { while (l % 2 == 0) l >>= 1; if (!f(op.Operate(sm, d[l]))) { while (l < size) { l = (2 * l); if (f(op.Operate(sm, d[l]))) { sm = op.Operate(sm, d[l]); l++; } } return l - size; } sm = op.Operate(sm, d[l]); l++; } while ((l & -l) != l); return Length; } public int MinLeft(int r, Predicate<TValue> f) { DebugUtil.Assert((uint)r <= Length); DebugUtil.Assert(f(op.Identity)); if (r == 0) return 0; r += size; var sm = op.Identity; do { r--; while (r > 1 && (r % 2) != 0) r >>= 1; if (!f(op.Operate(d[r], sm))) { while (r < size) { r = (2 * r + 1); if (f(op.Operate(d[r], sm))) { sm = op.Operate(d[r], sm); r--; } } return r + 1 - size; } sm = op.Operate(d[r], sm); } while ((r & -r) != r); return 0; } [DebuggerDisplay("{" + nameof(value) + "}", Name = "{" + nameof(key) + ",nq}")] private struct DebugItem { public DebugItem(int l, int r, TValue value) { if (r - l == 1) key = $"[{l}]"; else key = $"[{l}-{r})"; this.value = value; } [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly string key; [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly TValue value; } private class DebugView { private readonly Segtree<TValue, TOp> segtree; public DebugView(Segtree<TValue, TOp> segtree) { this.segtree = segtree; } [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)] public DebugItem[] Items { get { var items = new List<DebugItem>(segtree.Length); for (int len = segtree.size; len > 0; len >>= 1) { int unit = segtree.size / len; for (int i = 0; i < len; i++) { int l = i * unit; int r = Math.Min(l + unit, segtree.Length); if (l < segtree.Length) items.Add(new DebugItem(l, r, segtree.d[i + len])); } } return items.ToArray(); } } } [Conditional("DEBUG")] public static void AssertMonoid(TValue value) { DebugUtil.Assert(op.Operate(value, op.Identity).Equals(value), $"{nameof(op.Operate)}({value}, {op.Identity}) != {value}"); DebugUtil.Assert(op.Operate(op.Identity, value).Equals(value), $"{nameof(op.Operate)}({op.Identity}, {value}) != {value}"); } } } 
namespace SourceExpander { public class Expander { [Conditional("EXPANDER")] public static void Expand(string inputFilePath = null, string outputFilePath = null, bool ignoreAnyError = true) { } public static string ExpandString(string inputFilePath = null, bool ignoreAnyError = true) { return ""; } } } 
namespace AtCoder.Internal { internal static class DebugUtil { [Conditional("DEBUG")] public static void Assert(bool condition, string reason = null) { if (!condition) throw new DebugAssertException(reason); } [Conditional("DEBUG")] public static void Assert(Func<bool> conditionFunc, string reason = null) { if (!conditionFunc()) throw new DebugAssertException(reason); } } } 
namespace AtCoder.Internal { public static class InternalBit { [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int ExtractLowestSetBit(int n) { if (Bmi1.IsSupported) { return (int)Bmi1.ExtractLowestSetBit((uint)n); } return n & -n; } [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int BSF(uint n) { DebugUtil.Assert(n >= 1); return BitOperations.TrailingZeroCount(n); } public static int CeilPow2(int n) { var un = (uint)n; if (un <= 1) return 0; return BitOperations.Log2(un - 1) + 1; } } } 
namespace AtCoder.Internal { public class DebugAssertException : Exception { public DebugAssertException() : base() { } public DebugAssertException(string message) : base(message) { } public DebugAssertException(string message, Exception innerException) : base(message, innerException) { } } } 
#endregion Expanded
```

## License

The files in `Source/AtCoderLibrary` are licensed under CC0 license.
Another files are licensed under MIT license.