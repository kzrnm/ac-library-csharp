using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtCoderAnalyzer.Helpers
{
    internal class SimplifyTypeSyntaxRewriter : CSharpSyntaxRewriter
    {
        private ImmutableHashSet<string> Usings { get; }
        public SimplifyTypeSyntaxRewriter(ImmutableHashSet<string> usings) => Usings = usings;

        public override SyntaxNode Visit(SyntaxNode node)
        {
            if (node is QualifiedNameSyntax qualified && Usings.Contains(qualified.Left.ToString()))
                return qualified.Right;
            return base.Visit(node);
        }
    }
}
