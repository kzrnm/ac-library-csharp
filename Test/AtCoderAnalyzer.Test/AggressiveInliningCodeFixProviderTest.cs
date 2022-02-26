using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using VerifyCS = AtCoderAnalyzer.Test.CSharpCodeFixVerifier<
    AtCoderAnalyzer.AggressiveInliningAnalyzer,
    AtCoderAnalyzer.AggressiveInliningCodeFixProvider>;

namespace AtCoderAnalyzer.Test
{
    public class AggressiveInliningCodeFixProviderTest
    {
        [Fact]
        public async Task Empty()
        {
            var source = @"
using System.Collections.Generic;
struct IntComparer : IComparer<int>
{
    public int Compare(int x,  int y) => x.CompareTo(y);
}
";
            await VerifyCS.VerifyAnalyzerAsync(source);
        }

        [Fact]
        public async Task NumOperator()
        {
            var source = @"
using AtCoder.Operators;
using System;
using System.Runtime.CompilerServices;
struct BoolOp : INumOperator<bool>
{
    public bool MinValue => true;
    public bool MaxValue => false;
    public bool Add(bool x, bool y) => x || y;
    public int Compare(bool x, bool y) => x.CompareTo(y);
    public bool Decrement(bool x) => false;
    public bool Divide(bool x, bool y) => throw new NotImplementedException();
    public bool Equals(bool x, bool y) => x == y;
    public int GetHashCode(bool obj) => obj.GetHashCode();
    public bool GreaterThan(bool x, bool y) => x && !y;
    public bool GreaterThanOrEqual(bool x, bool y) => x || !y;
    public bool Increment(bool x) => true;
    public bool LessThan(bool x, bool y) => y && !x;
    public bool LessThanOrEqual(bool x, bool y) => y || !x;
    public bool Minus(bool x) => false;
    public bool Modulo(bool x, bool y) => true ? true : throw new NotImplementedException();
    public bool Multiply(bool x, bool y)
    {
        throw new NotImplementedException();
    }
    public bool Subtract(bool x, bool y)
    {
        return default(bool?) ?? throw new NotImplementedException();
    }
}
";

            var fixedSource = @"
using AtCoder.Operators;
using System;
using System.Runtime.CompilerServices;
struct BoolOp : INumOperator<bool>
{
    public bool MinValue => true;
    public bool MaxValue => false;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Add(bool x, bool y) => x || y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(bool x, bool y) => x.CompareTo(y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Decrement(bool x) => false;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Divide(bool x, bool y) => throw new NotImplementedException();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(bool x, bool y) => x == y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetHashCode(bool obj) => obj.GetHashCode();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GreaterThan(bool x, bool y) => x && !y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GreaterThanOrEqual(bool x, bool y) => x || !y;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Increment(bool x) => true;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(bool x, bool y) => y && !x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThanOrEqual(bool x, bool y) => y || !x;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Minus(bool x) => false;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Modulo(bool x, bool y) => true ? true : throw new NotImplementedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Multiply(bool x, bool y)
    {
        throw new NotImplementedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Subtract(bool x, bool y)
    {
        return default(bool?) ?? throw new NotImplementedException();
    }
}
";
            await VerifyCS.VerifyCodeFixAsync(source, new DiagnosticResult[] {
                VerifyCS.Diagnostic().WithSpan(5, 1, 30, 2).WithArguments(
                    "Add, Compare, Decrement, Divide, Equals, GetHashCode, GreaterThan, GreaterThanOrEqual, Increment, LessThan, LessThanOrEqual, Minus, Modulo, Multiply, Subtract"),
            }, fixedSource);
        }

        [Fact]
        public async Task SegtreeOperator()
        {
            var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
struct OpSeg : ISegtreeOperator<int>
{
    public int Identity => default;
    public int Operate(int x, int y) => System.Math.Max(x, y);
}
";
            var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;
struct OpSeg : ISegtreeOperator<int>
{
    public int Identity => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y) => System.Math.Max(x, y);
}
";
            await VerifyCS.VerifyCodeFixAsync(source, new DiagnosticResult[]
            {
                VerifyCS.Diagnostic("AC0007").WithSpan(4, 1, 8, 2).WithArguments("Operate"),
            }, fixedSource);
        }

        [Fact]
        public async Task SegtreeOperator_With_AggressiveInlining()
        {
            var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
struct OpSeg : ISegtreeOperator<int>
{
    public int Identity => default;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y) => System.Math.Max(x, y);
}
";
            await VerifyCS.VerifyAnalyzerAsync(source);
        }

        [Fact]
        public async Task LazySegtreeOperator()
        {
            var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    public int Composition(int f, int g) => 0;
    public long Mapping(int f, long x) => 0L;
    public long Operate(long x, long y) => 0L;
}
";
            var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;
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
            await VerifyCS.VerifyCodeFixAsync(source, new DiagnosticResult[]
            {
                VerifyCS.Diagnostic().WithSpan(4, 1, 11, 2).WithArguments("Composition, Mapping, Operate"),
            }, fixedSource);
        }

        [Fact]
        public async Task LazySegtreeOperator_Without_Using()
        {
            var source = @"
using AtCoder;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    public int Composition(int f, int g) => 0;
    public long Mapping(int f, long x) => 0L;
    public long Operate(long x, long y) => 0L;
}
";
            var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;

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
            await VerifyCS.VerifyCodeFixAsync(source, new DiagnosticResult[]
            {
VerifyCS.Diagnostic().WithSpan(3, 1, 10, 2).WithArguments("Composition, Mapping, Operate"),
            }, fixedSource);
        }

        [Fact]
        public async Task LazySegtreeOperator_With_AggressiveInlining()
        {
            var source = @"
using AtCoder;
using System.Runtime.CompilerServices;
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
            await VerifyCS.VerifyAnalyzerAsync(source);
        }

        [Fact]
        public async Task LazySegtreeOperator_With_Qualified_AggressiveInlining()
        {
            var source = @"
using AtCoder;
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
            await VerifyCS.VerifyAnalyzerAsync(source);
        }

        [Fact]
        public async Task LazySegtreeOperator_With_Alias_AggressiveInlining()
        {
            var source = @"
using AtCoder;
using MI = System.Runtime.CompilerServices.MethodImplAttribute;
struct Op : ILazySegtreeOperator<long, int>
{
    public long Identity => 0L;
    public int FIdentity => 0;
    [MI(256)]
    public int Composition(int f, int g) => 0;
    [MI(256)]
    public long Mapping(int f, long x) => 0L;
    [MI(256)]
    public long Operate(long x, long y) => 0L;
}
";
            await VerifyCS.VerifyAnalyzerAsync(source);
        }


        [Fact]
        public async Task AnyDefinedType()
        {
            var source = @"
using AtCoder;
[IsOperator]
public interface IAny<T> {
    T Fun1();
    string Fun2(T v);
}
struct Def<T> : IAny<T> {
    public T Fun1() => default;
    public string Fun2(T v)
    {
        return v.ToString();
    }
}
";
            var fixedSource = @"
using AtCoder;
using System.Runtime.CompilerServices;

[IsOperator]
public interface IAny<T> {
    T Fun1();
    string Fun2(T v);
}
struct Def<T> : IAny<T> {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Fun1() => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Fun2(T v)
    {
        return v.ToString();
    }
}
";
            await VerifyCS.VerifyCodeFixAsync(source,
                VerifyCS.Diagnostic().WithSpan(8, 1, 14, 2).WithArguments("Fun1, Fun2"),
                fixedSource);
        }
    }
}
