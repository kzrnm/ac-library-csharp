using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;

namespace AtCoderAnalyzer
{
    public class AtCoderCodeFixProvider : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds
            => ImmutableArray.Create<string>();

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {

        }
    }
}
