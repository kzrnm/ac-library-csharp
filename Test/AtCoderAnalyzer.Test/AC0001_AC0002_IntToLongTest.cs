using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using VerifyCS = AtCoderAnalyzer.Test.CSharpCodeFixVerifier<
    AtCoderAnalyzer.AC0001_AC0002_IntToLongAnalyzer,
    AtCoderAnalyzer.AC0001_AC0002_IntToLongCodeFixProvider>;

namespace AtCoderAnalyzer.Test
{
    public class AC0001_AC0002_IntToLongTest
    {
        [Fact]
        public async Task Empty()
        {
            var source = @"
class Program
{
    long Long = 0;
    long LongProperty => 0;
    long Return() => 0;
    void Arg(long arg){ }
    void Arg2(){ Arg(0); }
}
";

            await VerifyCS.VerifyAnalyzerAsync(source);
        }


        [Fact]
        public async Task Mixed()
        {
            var source = @"
class Program
{
    static void F(int num)
    {
        long v;
        v = num * 2 * 3;
        v = 4 * 5 * num;
        v = num * 6L * 7;
        v = num * 8 * 9L;
        v += 10 * num;
        v = 11 * num + num / 12 - num * 13 + 14 - 15;

        v *= num;
        v <<= num;
    }
}
";

            var fixedSource = @"
class Program
{
    static void F(int num)
    {
        long v;
        v = (long)num * 2 * 3;
        v = 4L * 5 * num;
        v = num * 6L * 7;
        v = (long)num * 8 * 9L;
        v += 10L * num;
        v = 11L * num + num / 12 - (long)num * 13 + 14 - 15;

        v *= num;
        v <<= num;
    }
}
";
            await VerifyCS.VerifyCodeFixAsync(source,
                new DiagnosticResult[] {
                    VerifyCS.Diagnostic("AC0001").WithSpan(7, 13, 7, 20).WithArguments("num * 2"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(7, 13, 7, 24).WithArguments("num * 2 * 3"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(8, 13, 8, 18).WithArguments("4 * 5"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(8, 13, 8, 24).WithArguments("4 * 5 * num"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(10, 13, 10, 20).WithArguments("num * 8"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(11, 14, 11, 22).WithArguments("10 * num"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(12, 13, 12, 21).WithArguments("11 * num"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(12, 35, 12, 43).WithArguments("num * 13"),
                },
                fixedSource);
        }

        [Fact]
        public async Task Literal()
        {
            var source = @"
class Multiply
{
    long Long = 1000 * 1001;
    long LongProperty => 1002 * 1003;
    long Return() => 1004 * 1005;
    void Arg(long arg){ }
    void Arg2(){ Arg(1006 * 1007); }
}
class LeftShift
{
    long Long = 1000 << 1;
    long LongProperty => 1002 << 3;
    long Return() => 1004 << 5;
    void Arg(long arg){ }
    void Arg2(){ Arg(1006 << 7); }
}
";

            var fixedSource = @"
class Multiply
{
    long Long = 1000L * 1001;
    long LongProperty => 1002L * 1003;
    long Return() => 1004L * 1005;
    void Arg(long arg){ }
    void Arg2(){ Arg(1006L * 1007); }
}
class LeftShift
{
    long Long = 1000L << 1;
    long LongProperty => 1002L << 3;
    long Return() => 1004L << 5;
    void Arg(long arg){ }
    void Arg2(){ Arg(1006L << 7); }
}
";
            await VerifyCS.VerifyCodeFixAsync(source,
                new DiagnosticResult[] {
                    VerifyCS.Diagnostic("AC0001").WithSpan(4, 17, 4, 28).WithArguments("1000 * 1001"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(5, 26, 5, 37).WithArguments("1002 * 1003"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(6, 22, 6, 33).WithArguments("1004 * 1005"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(8, 22, 8, 33).WithArguments("1006 * 1007"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(12, 17, 12, 26).WithArguments("1000 << 1"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(13, 26, 13, 35).WithArguments("1002 << 3"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(14, 22, 14, 31).WithArguments("1004 << 5"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(16, 22, 16, 31).WithArguments("1006 << 7"),
                },
                fixedSource);
        }


        [Fact]
        public async Task Const()
        {
            var source = @"
class Multiply
{
    const int num = 0;
    long Long = num * 1001;
    long LongProperty => num * 1003;
    long Return() => num * 1005;
    void Arg(long arg){ }
    void Arg2(){ Arg(num * 1007); }
}
class LeftShift
{
    const int num = 0;
    long Long = num << 1;
    long LongProperty => num << 3;
    long Return() => num << 5;
    void Arg(long arg){ }
    void Arg2(){ Arg(num << 7); }
}
";

            var fixedSource = @"
class Multiply
{
    const int num = 0;
    long Long = (long)num * 1001;
    long LongProperty => (long)num * 1003;
    long Return() => (long)num * 1005;
    void Arg(long arg){ }
    void Arg2(){ Arg((long)num * 1007); }
}
class LeftShift
{
    const int num = 0;
    long Long = (long)num << 1;
    long LongProperty => (long)num << 3;
    long Return() => (long)num << 5;
    void Arg(long arg){ }
    void Arg2(){ Arg((long)num << 7); }
}
";
            await VerifyCS.VerifyCodeFixAsync(source,
                new DiagnosticResult[] {
                    VerifyCS.Diagnostic("AC0001").WithSpan(5, 17, 5, 27).WithArguments("num * 1001"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(6, 26, 6, 36).WithArguments("num * 1003"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(7, 22, 7, 32).WithArguments("num * 1005"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(9, 22, 9, 32).WithArguments("num * 1007"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(14, 17, 14, 25).WithArguments("num << 1"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(15, 26, 15, 34).WithArguments("num << 3"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(16, 22, 16, 30).WithArguments("num << 5"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(18, 22, 18, 30).WithArguments("num << 7"),
                },
                fixedSource);
        }


        [Fact]
        public async Task Variable()
        {
            var source = @"
class Multiply
{
    static int num = 0;
    long Long = num * 1001;
    long LongProperty => num * 1003;
    long Return() => num * 1005;
    void Arg(long arg){ }
    void Arg2(){ Arg(num * 1007); }
}
class LeftShift
{
    static int num = 0;
    long Long = num << 1;
    long LongProperty => num << 3;
    long Return() => num << 5;
    void Arg(long arg){ }
    void Arg2(){ Arg(num << 7); }
}
";

            var fixedSource = @"
class Multiply
{
    static int num = 0;
    long Long = (long)num * 1001;
    long LongProperty => (long)num * 1003;
    long Return() => (long)num * 1005;
    void Arg(long arg){ }
    void Arg2(){ Arg((long)num * 1007); }
}
class LeftShift
{
    static int num = 0;
    long Long = (long)num << 1;
    long LongProperty => (long)num << 3;
    long Return() => (long)num << 5;
    void Arg(long arg){ }
    void Arg2(){ Arg((long)num << 7); }
}
";
            await VerifyCS.VerifyCodeFixAsync(source,
                new DiagnosticResult[] {
                    VerifyCS.Diagnostic("AC0001").WithSpan(5, 17, 5, 27).WithArguments("num * 1001"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(6, 26, 6, 36).WithArguments("num * 1003"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(7, 22, 7, 32).WithArguments("num * 1005"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(9, 22, 9, 32).WithArguments("num * 1007"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(14, 17, 14, 25).WithArguments("num << 1"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(15, 26, 15, 34).WithArguments("num << 3"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(16, 22, 16, 30).WithArguments("num << 5"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(18, 22, 18, 30).WithArguments("num << 7"),
                },
                fixedSource);
        }

        [Fact]
        public async Task VariableShort()
        {
            var source = @"
class Multiply
{
    static short num = 0;
    long Long = num * 1001;
    long LongProperty => num * 1003;
    long Return() => num * 1005;
    void Arg(long arg){ }
    void Arg2(){ Arg(num * 1007); }
}
class LeftShift
{
    static short num = 0;
    long Long = num << 1;
    long LongProperty => num << 3;
    long Return() => num << 5;
    void Arg(long arg){ }
    void Arg2(){ Arg(num << 7); }
}
";

            var fixedSource = @"
class Multiply
{
    static short num = 0;
    long Long = (long)num * 1001;
    long LongProperty => (long)num * 1003;
    long Return() => (long)num * 1005;
    void Arg(long arg){ }
    void Arg2(){ Arg((long)num * 1007); }
}
class LeftShift
{
    static short num = 0;
    long Long = (long)num << 1;
    long LongProperty => (long)num << 3;
    long Return() => (long)num << 5;
    void Arg(long arg){ }
    void Arg2(){ Arg((long)num << 7); }
}
";
            await VerifyCS.VerifyCodeFixAsync(source,
                new DiagnosticResult[] {
                    VerifyCS.Diagnostic("AC0001").WithSpan(5, 17, 5, 27).WithArguments("num * 1001"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(6, 26, 6, 36).WithArguments("num * 1003"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(7, 22, 7, 32).WithArguments("num * 1005"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(9, 22, 9, 32).WithArguments("num * 1007"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(14, 17, 14, 25).WithArguments("num << 1"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(15, 26, 15, 34).WithArguments("num << 3"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(16, 22, 16, 30).WithArguments("num << 5"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(18, 22, 18, 30).WithArguments("num << 7"),
                },
                fixedSource);
        }

    }
}
