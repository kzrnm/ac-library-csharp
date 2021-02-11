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
            if (Usings.Contains(node.Left.ToString()))
                return base.Visit(node.Right);
            return base.VisitQualifiedName(node);
        }
    }
}
