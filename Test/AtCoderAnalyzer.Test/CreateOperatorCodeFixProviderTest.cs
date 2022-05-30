using System.Threading;
using System.Threading.Tasks;
using Xunit;
using VerifyCS = AtCoderAnalyzer.Test.CSharpCodeFixVerifier<
    AtCoderAnalyzer.CreateOperatorAnalyzer,
    AtCoderAnalyzer.CreateOperatorCodeFixProvider>;

namespace AtCoderAnalyzer.Test
{
    public class CreateOperatorCodeFixProviderTest
    {
        #region StaticModInt
        [Fact]
        public async Task StaticModInt_Using()
        {
            var source = @"
using AtCoder;
class Program
{
    StaticModInt<Op> notDefined;
    StaticModInt<Mod1000000007> defined;
}
";
            var fixedSource = @"
using AtCoder;
class Program
{
    StaticModInt<Op> notDefined;
    StaticModInt<Mod1000000007> defined;
}

struct Op : IStaticMod
{
    public uint Mod => default;

    public bool IsPrime => default;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(5, 5, 5, 21).WithArguments("Op"),
                fixedSource);
        }

        [Fact]
        public async Task StaticModInt_Qualified_Using()
        {
            var source = @"
using AtCoder;
class Program
{
    AtCoder.StaticModInt<Op> notDefined;
    AtCoder.StaticModInt<Mod1000000007> defined;
}
";
            var fixedSource = @"
using AtCoder;
class Program
{
    AtCoder.StaticModInt<Op> notDefined;
    AtCoder.StaticModInt<Mod1000000007> defined;
}

struct Op : IStaticMod
{
    public uint Mod => default;

    public bool IsPrime => default;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(5, 13, 5, 29).WithArguments("Op"),
                fixedSource);
        }

        [Fact]
        public async Task StaticModInt_Qualified()
        {
            var source = @"
class Program
{
    AtCoder.StaticModInt<Op> notDefined;
    AtCoder.StaticModInt<AtCoder.Mod1000000007> defined;
}
";
            var fixedSource = @"
class Program
{
    AtCoder.StaticModInt<Op> notDefined;
    AtCoder.StaticModInt<AtCoder.Mod1000000007> defined;
}

struct Op : AtCoder.IStaticMod
{
    public uint Mod => default;

    public bool IsPrime => default;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(4, 13, 4, 29).WithArguments("Op"),
                fixedSource);
        }
        #endregion StaticModInt

        #region Segtree
        [Fact]
        public async Task Segtree_Using()
        {
            var source = @"using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<int, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}
";
            var fixedSource = @"using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<int, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}

struct OpSeg : ISegtreeOperator<int>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y) => default;

    public int Identity => default;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(6, 5, 6, 24).WithArguments("OpSeg"),
                fixedSource);
        }

        [Fact]
        public async Task Segtree_Qualified_Using()
        {
            var source = @"using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    AtCoder.Segtree<int, MinOp> defined;
    AtCoder.Segtree<int, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}
";
            var fixedSource = @"using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    AtCoder.Segtree<int, MinOp> defined;
    AtCoder.Segtree<int, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}

struct OpSeg : ISegtreeOperator<int>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y) => default;

    public int Identity => default;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(6, 13, 6, 32).WithArguments("OpSeg"),
                fixedSource);
        }

        [Fact]
        public async Task Segtree_Qualified()
        {
            var source = @"
class Program
{
    AtCoder.Segtree<int, MinOp> defined;
    AtCoder.Segtree<int, OpSeg> notDefined;
}
struct MinOp : AtCoder.ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}
";
            var fixedSource = @"
class Program
{
    AtCoder.Segtree<int, MinOp> defined;
    AtCoder.Segtree<int, OpSeg> notDefined;
}
struct MinOp : AtCoder.ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}

struct OpSeg : AtCoder.ISegtreeOperator<int>
{
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y) => default;

    public int Identity => default;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(5, 13, 5, 32).WithArguments("OpSeg"),
                fixedSource);
        }

        [Fact]
        public async Task Segtree_Using_With_System_Runtime_CompilerServices()
        {
            var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<int, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}
";
            var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<int, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}

struct OpSeg : ISegtreeOperator<int>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y) => default;

    public int Identity => default;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(7, 5, 7, 24).WithArguments("OpSeg"),
                fixedSource);
        }
        #endregion Segtree

        #region LazySegtree
        [Fact]
        public async Task LazySegtree_Using()
        {
            var source = @"using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    LazySegtree<long, int, Op> defined;
    LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}
";
            var fixedSource = @"using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    LazySegtree<long, int, Op> defined;
    LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}

struct OpSeg : ILazySegtreeOperator<(int v, int size), (int b, int c)>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Operate((int v, int size) x, (int v, int size) y) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Mapping((int b, int c) f, (int v, int size) x) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int b, int c) Composition((int b, int c) nf, (int b, int c) cf) => default;

    public (int v, int size) Identity => default;

    public (int b, int c) FIdentity => default;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(6, 5, 6, 58).WithArguments("OpSeg"),
                fixedSource);
        }

        [Fact]
        public async Task LazySegtree_Qualified_Using()
        {
            var source = @"using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    AtCoder.LazySegtree<long, int, Op> defined;
    AtCoder.LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : AtCoder.ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}
";
            var fixedSource = @"using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    AtCoder.LazySegtree<long, int, Op> defined;
    AtCoder.LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : AtCoder.ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}

struct OpSeg : ILazySegtreeOperator<(int v, int size), (int b, int c)>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Operate((int v, int size) x, (int v, int size) y) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Mapping((int b, int c) f, (int v, int size) x) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int b, int c) Composition((int b, int c) nf, (int b, int c) cf) => default;

    public (int v, int size) Identity => default;

    public (int b, int c) FIdentity => default;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(6, 13, 6, 66).WithArguments("OpSeg"),
                fixedSource);
        }

        [Fact]
        public async Task LazySegtree_Qualified()
        {
            var source = @"using System.Runtime.CompilerServices;

class Program
{
    AtCoder.LazySegtree<long, int, Op> defined;
    AtCoder.LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : AtCoder.ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}
";
            var fixedSource = @"using System.Runtime.CompilerServices;

class Program
{
    AtCoder.LazySegtree<long, int, Op> defined;
    AtCoder.LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : AtCoder.ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}

struct OpSeg : AtCoder.ILazySegtreeOperator<(int v, int size), (int b, int c)>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Operate((int v, int size) x, (int v, int size) y) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Mapping((int b, int c) f, (int v, int size) x) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int b, int c) Composition((int b, int c) nf, (int b, int c) cf) => default;

    public (int v, int size) Identity => default;

    public (int b, int c) FIdentity => default;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(6, 13, 6, 66).WithArguments("OpSeg"),
                fixedSource);
        }

        [Fact]
        public async Task LazySegtree_Using_With_System_Runtime_CompilerServices()
        {
            var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    LazySegtree<long, int, Op> defined;
    LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}
";
            var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    LazySegtree<long, int, Op> defined;
    LazySegtree<(int v, int size), (int b, int c), OpSeg> notDefined;
}
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}

struct OpSeg : ILazySegtreeOperator<(int v, int size), (int b, int c)>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Operate((int v, int size) x, (int v, int size) y) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Mapping((int b, int c) f, (int v, int size) x) => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int b, int c) Composition((int b, int c) nf, (int b, int c) cf) => default;

    public (int v, int size) Identity => default;

    public (int b, int c) FIdentity => default;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(7, 5, 7, 58).WithArguments("OpSeg"),
                fixedSource);
        }
        #endregion LazySegtree

        #region Others
        [Fact]
        public async Task ICompare()
        {
            var source = @"using System;
using AtCoder;
public class Generic<T, TOp> where TOp : System.Collections.Generic.IComparer<T> { }
class Program
{
    Generic<short, Op> notDefined;
    Type Type = typeof(Generic<,>);
}
";
            var fixedSource = @"using System;
using AtCoder;
public class Generic<T, TOp> where TOp : System.Collections.Generic.IComparer<T> { }
class Program
{
    Generic<short, Op> notDefined;
    Type Type = typeof(Generic<,>);
}

struct Op : System.Collections.Generic.IComparer<short>
{
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Compare(short x, short y) => x.CompareTo(y);
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(6, 5, 6, 23).WithArguments("Op"),
                fixedSource);
        }

        [Fact]
        public async Task AnyDefinedType()
        {
            var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IAny<T> {
    void Init(ref T val, out bool success, params int[] nums);
    T Prop1 { set; get; }
    T Prop2 { get; set; }
}
public class Generic<T, TOp> where TOp : IAny<T> { }
class Program
{
    Generic<(int, long), Op> notDefined;
    Generic<(int, long), Def<(int, long)>> defined;
    System.Type Type = typeof(Generic<,>);
}
struct Def<T> : IAny<T> {
    public void Init(ref T val, out bool success, params int[] nums) { success = true; }
    public T Prop1 { set; get; }
    public T Prop2 { set; get; }
}
";
            var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IAny<T> {
    void Init(ref T val, out bool success, params int[] nums);
    T Prop1 { set; get; }
    T Prop2 { get; set; }
}
public class Generic<T, TOp> where TOp : IAny<T> { }
class Program
{
    Generic<(int, long), Op> notDefined;
    Generic<(int, long), Def<(int, long)>> defined;
    System.Type Type = typeof(Generic<,>);
}
struct Def<T> : IAny<T> {
    public void Init(ref T val, out bool success, params int[] nums) { success = true; }
    public T Prop1 { set; get; }
    public T Prop2 { set; get; }
}

struct Op : IAny<(int, long)>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Init(ref (int, long) val, out bool success, params int[] nums)
    {
    }

    public (int, long) Prop1 { set; get; }
    public (int, long) Prop2 { set; get; }
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(13, 5, 13, 29).WithArguments("Op"),
               fixedSource);
        }

        [Fact]
        public async Task AnyDefinedMethod()
        {
            var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IAny<T> {
    void Init();
    T Prop1 { set; get; }
    T Prop2 { get; set; }
}
class Program
{
    static void M<T, TOp>() where TOp : struct, IAny<T> {}
    static void Run()
    {
        M<(int n, long m), Op>();
    }
}
";
            var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;
[IsOperator]
public interface IAny<T> {
    void Init();
    T Prop1 { set; get; }
    T Prop2 { get; set; }
}
class Program
{
    static void M<T, TOp>() where TOp : struct, IAny<T> {}
    static void Run()
    {
        M<(int n, long m), Op>();
    }
}

struct Op : IAny<(int n, long m)>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Init()
    {
    }

    public (int n, long m) Prop1 { set; get; }
    public (int n, long m) Prop2 { set; get; }
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(15, 9, 15, 31).WithArguments("Op"),
               fixedSource);
        }

        [Fact]
        public async Task Array()
        {
            var source = @"
using AtCoder;
using System.Runtime.CompilerServices;

[IsOperator]
public interface IAny<T> {
    T Prop { get; }
}
class Program
{
    static void M<T, TOp>() where TOp : struct, IAny<T> {}
    static void Run()
    {
        M<System.Numerics.BigInteger[], BigOp>();
    }
}
";
            var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;

[IsOperator]
public interface IAny<T> {
    T Prop { get; }
}
class Program
{
    static void M<T, TOp>() where TOp : struct, IAny<T> {}
    static void Run()
    {
        M<System.Numerics.BigInteger[], BigOp>();
    }
}

struct BigOp : IAny<System.Numerics.BigInteger[]>
{
    public System.Numerics.BigInteger[] Prop => default;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(14, 9, 14, 47).WithArguments("BigOp"),
                fixedSource);
        }

        [Fact]
        public async Task Generic()
        {
            var source = @"
using AtCoder;
using System.Runtime.CompilerServices;

[IsOperator]
public interface IAny<T> {
    T Prop { get; }
}
class Program
{
    static void M<T, TOp>() where TOp : struct, IAny<T> {}
    static void Run()
    {
        M<StaticModInt<Mod1000000007>, ModOp>();
    }
}
";
            var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;

[IsOperator]
public interface IAny<T> {
    T Prop { get; }
}
class Program
{
    static void M<T, TOp>() where TOp : struct, IAny<T> {}
    static void Run()
    {
        M<StaticModInt<Mod1000000007>, ModOp>();
    }
}

struct ModOp : IAny<StaticModInt<Mod1000000007>>
{
    public StaticModInt<Mod1000000007> Prop => default;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(14, 9, 14, 46).WithArguments("ModOp"),
                fixedSource);
        }

        [Fact]
        public async Task NumOperatorAndShiftOperator()
        {
            var source = @"
using AtCoder.Operators;
using System.Runtime.CompilerServices;
class Program
{
    void M<T>() where T : INumOperator<int>, IShiftOperator<int>, INumOperator<float>, ICastOperator<short, char> { }
    public void Main()
    {
        M<Op>();
    }
}
";
            var fixedSource = @"
using AtCoder.Operators;
using System.Runtime.CompilerServices;
class Program
{
    void M<T>() where T : INumOperator<int>, IShiftOperator<int>, INumOperator<float>, ICastOperator<short, char> { }
    public void Main()
    {
        M<Op>();
    }
}

struct Op : ICastOperator<short, char>, INumOperator<float>, INumOperator<int>, IShiftOperator<int>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public char Cast(short y) => (char)y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Add(float x, float y) => x + y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Subtract(float x, float y) => x - y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Divide(float x, float y) => x / y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Modulo(float x, float y) => x % y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Multiply(float x, float y) => x * y;

    public float MultiplyIdentity => 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Minus(float x) => -x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Increment(float x) => ++x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Decrement(float x) => --x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GreaterThan(float x, float y) => x > y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GreaterThanOrEqual(float x, float y) => x >= y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(float x, float y) => x < y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThanOrEqual(float x, float y) => x <= y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(float x, float y) => x.CompareTo(y);

    public float MinValue => float.MinValue;

    public float MaxValue => float.MaxValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Add(int x, int y) => x + y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Subtract(int x, int y) => x - y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Divide(int x, int y) => x / y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Modulo(int x, int y) => x % y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Multiply(int x, int y) => x * y;

    public int MultiplyIdentity => 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Minus(int x) => -x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Increment(int x) => ++x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Decrement(int x) => --x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GreaterThan(int x, int y) => x > y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GreaterThanOrEqual(int x, int y) => x >= y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(int x, int y) => x < y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThanOrEqual(int x, int y) => x <= y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(int x, int y) => x.CompareTo(y);

    public int MinValue => int.MinValue;

    public int MaxValue => int.MaxValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int LeftShift(int x, int y) => x << y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int RightShift(int x, int y) => x >> y;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(9, 9, 9, 14).WithArguments("Op"),
                fixedSource);
        }

        [Fact]
        public async Task UsingAlias()
        {
            var source = @"using AtCoder;
using System.Runtime.CompilerServices;
using ModInt = StaticModInt<Mod1000000007>;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<ModInt, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}
";
            var fixedSource = @"using AtCoder;
using System.Runtime.CompilerServices;
using ModInt = StaticModInt<Mod1000000007>;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<ModInt, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}

struct OpSeg : ISegtreeOperator<ModInt>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ModInt Operate(ModInt x, ModInt y) => default;

    public ModInt Identity => default;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0008").WithSpan(7, 5, 7, 27).WithArguments("OpSeg"),
                fixedSource);
        }

        [Fact]
        public async Task MethodImplAlias()
        {
            var source = @"using AtCoder;
using System.Runtime.CompilerServices;
using MI = System.Runtime.CompilerServices.MethodImplAttribute;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<long, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}
";
            var fixedSource = @"using AtCoder;
using System.Runtime.CompilerServices;
using MI = System.Runtime.CompilerServices.MethodImplAttribute;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<long, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}

struct OpSeg : ISegtreeOperator<long>
{
    [MI(MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => default;

    public long Identity => default;
}";

            var test = new VerifyCS.Test
            {
                TestCode = source,
                FixedCode = fixedSource,
            };
            test.ExpectedDiagnostics.Add(VerifyCS.Diagnostic("AC0008").WithSpan(7, 5, 7, 25).WithArguments("OpSeg"));
            await test.RunAsync(CancellationToken.None);
        }

        [Fact]
        public async Task MethodImpl256()
        {
            var source = @"using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<long, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}
";
            var fixedSource = @"using AtCoder;
using System.Runtime.CompilerServices;
class Program
{
    Segtree<int, MinOp> defined;
    Segtree<long, OpSeg> notDefined;
}
struct MinOp : ISegtreeOperator<int>
{
    public int Identity => 0;

    public int Operate(int x, int y)
    {
        return System.Math.Min(x, y);
    }
}

struct OpSeg : ISegtreeOperator<long>
{
    [MethodImpl(256)]
    public long Operate(long x, long y) => default;

    public long Identity => default;
}";

            var test = new VerifyCS.Test
            {
                TestCode = source,
                FixedCode = fixedSource,
                TestState =
                {
                    AnalyzerConfigFiles =
                    {
                        ("/.editorconfig", @"
is_global = true
build_property.AtCoderAnalyzer_UseMethodImplNumeric = true
")
                    },
                },
            };
            test.ExpectedDiagnostics.Add(VerifyCS.Diagnostic("AC0008").WithSpan(6, 5, 6, 25).WithArguments("OpSeg"));
            await test.RunAsync(CancellationToken.None);
        }
        #endregion Others
    }
}
