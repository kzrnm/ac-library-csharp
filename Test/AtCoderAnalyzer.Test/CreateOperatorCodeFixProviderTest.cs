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
    public uint Mod => 1000000007;

    public bool IsPrime => true;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0003").WithSpan(5, 5, 5, 21).WithArguments("Op"),
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
    public uint Mod => 1000000007;

    public bool IsPrime => true;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0003").WithSpan(5, 13, 5, 29).WithArguments("Op"),
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
    public uint Mod => 1000000007;

    public bool IsPrime => true;
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0003").WithSpan(4, 13, 4, 29).WithArguments("Op"),
                fixedSource);
        }
        #endregion StaticModInt

        #region DynamicModInt
        [Fact]
        public async Task DynamicModInt_Using()
        {
            var source = @"
using AtCoder;
class Program
{
    DynamicModInt<Op> notDefined;
    DynamicModInt<ModID0> defined;
}
";
            var fixedSource = @"
using AtCoder;
class Program
{
    DynamicModInt<Op> notDefined;
    DynamicModInt<ModID0> defined;
}

struct Op : IDynamicModID
{
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0004").WithSpan(5, 5, 5, 22).WithArguments("Op"),
                fixedSource);
        }

        [Fact]
        public async Task DynamicModInt_Qualified_Using()
        {
            var source = @"
using AtCoder;
class Program
{
    AtCoder.DynamicModInt<Op> notDefined;
    AtCoder.DynamicModInt<ModID0> defined;
}
";
            var fixedSource = @"
using AtCoder;
class Program
{
    AtCoder.DynamicModInt<Op> notDefined;
    AtCoder.DynamicModInt<ModID0> defined;
}

struct Op : IDynamicModID
{
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0004").WithSpan(5, 13, 5, 30).WithArguments("Op"),
                fixedSource);
        }

        [Fact]
        public async Task DynamicModInt_Qualified()
        {
            var source = @"
class Program
{
    AtCoder.DynamicModInt<Op> notDefined;
    AtCoder.DynamicModInt<AtCoder.ModID0> defined;
}
";
            var fixedSource = @"
class Program
{
    AtCoder.DynamicModInt<Op> notDefined;
    AtCoder.DynamicModInt<AtCoder.ModID0> defined;
}

struct Op : AtCoder.IDynamicModID
{
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0004").WithSpan(4, 13, 4, 30).WithArguments("Op"),
                fixedSource);
        }
        #endregion DynamicModInt

        #region Segtree
        [Fact]
        public async Task Segtree_Using()
        {
            var source = @"
using AtCoder;
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
    public int Identity => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y)
    {
        return default;
    }
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0005").WithSpan(6, 5, 6, 24).WithArguments("OpSeg"),
                fixedSource);
        }

        [Fact]
        public async Task Segtree_Qualified_Using()
        {
            var source = @"
using AtCoder;
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
            var fixedSource = @"
using AtCoder;
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
    public int Identity => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y)
    {
        return default;
    }
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0005").WithSpan(6, 13, 6, 32).WithArguments("OpSeg"),
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
            var fixedSource = @"using System.Runtime.CompilerServices;

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
    public int Identity => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y)
    {
        return default;
    }
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0005").WithSpan(5, 13, 5, 32).WithArguments("OpSeg"),
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
    public int Identity => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y)
    {
        return default;
    }
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0005").WithSpan(7, 5, 7, 24).WithArguments("OpSeg"),
                fixedSource);
        }
        #endregion Segtree

        #region LazySegtree
        [Fact]
        public async Task LazySegtree_Using()
        {
            var source = @"
using AtCoder;
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
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int Composition(int f, int g) => 0;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Mapping(int f, long x) => 0L;
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public long Operate(long x, long y) => 0L;
}

struct OpSeg : ILazySegtreeOperator<(int v, int size), (int b, int c)>
{
    public (int v, int size) Identity => default;

    public (int b, int c) FIdentity => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int b, int c) Composition((int b, int c) f, (int b, int c) g)
    {
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Mapping((int b, int c) f, (int v, int size) x)
    {
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Operate((int v, int size) x, (int v, int size) y)
    {
        return default;
    }
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0006").WithSpan(6, 5, 6, 58).WithArguments("OpSeg"),
                fixedSource);
        }

        [Fact]
        public async Task LazySegtree_Qualified_Using()
        {
            var source = @"
using AtCoder;
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
            var fixedSource = @"
using AtCoder;
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
    public (int v, int size) Identity => default;

    public (int b, int c) FIdentity => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int b, int c) Composition((int b, int c) f, (int b, int c) g)
    {
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Mapping((int b, int c) f, (int v, int size) x)
    {
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Operate((int v, int size) x, (int v, int size) y)
    {
        return default;
    }
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0006").WithSpan(6, 13, 6, 66).WithArguments("OpSeg"),
                fixedSource);
        }

        [Fact]
        public async Task LazySegtree_Qualified()
        {
            var source = @"
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
    public (int v, int size) Identity => default;

    public (int b, int c) FIdentity => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int b, int c) Composition((int b, int c) f, (int b, int c) g)
    {
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Mapping((int b, int c) f, (int v, int size) x)
    {
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Operate((int v, int size) x, (int v, int size) y)
    {
        return default;
    }
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0006").WithSpan(5, 13, 5, 66).WithArguments("OpSeg"),
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
    public (int v, int size) Identity => default;

    public (int b, int c) FIdentity => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int b, int c) Composition((int b, int c) f, (int b, int c) g)
    {
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Mapping((int b, int c) f, (int v, int size) x)
    {
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int v, int size) Operate((int v, int size) x, (int v, int size) y)
    {
        return default;
    }
}";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic("AC0006").WithSpan(7, 5, 7, 58).WithArguments("OpSeg"),
                fixedSource);
        }
        #endregion LazySegtree

    }
}
