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

        public override SyntaxNode VisitQualifiedName(QualifiedNameSyntax node)
        {
            var right = (SimpleNameSyntax)Visit(node.Right);
            if (Usings.Contains(node.Left.ToString()))
                return right;
            return SyntaxFactory.QualifiedName(node.Left, right);
        }
    }
}
