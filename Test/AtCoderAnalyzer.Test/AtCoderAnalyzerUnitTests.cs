using System.Threading.Tasks;
using Xunit;
using VerifyCS = AtCoderAnalyzer.Test.CSharpCodeFixVerifier<
    AtCoderAnalyzer.AtCoderAnalyzer,
    AtCoderAnalyzer.AtCoderCodeFixProvider>;

namespace AtCoderAnalyzer.Test
{
    public class AtCoderAnalyzerUnitTest
    {
        //No diagnostics expected to show up
        [Fact]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [Fact]
        public async Task TestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class {|#0:TypeName|}
        {   
        }
    }";

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";

            var expected = VerifyCS.Diagnostic("AtCoderAnalyzer").WithLocation(0).WithArguments("TypeName");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}
