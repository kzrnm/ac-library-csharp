using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AtCoderAnalyzer.CreateOperators.Specified
{
    internal class CompareOperatorEnumerateMember : OperatorEnumerateMember
    {
        internal CompareOperatorEnumerateMember(SemanticModel semanticModel, ITypeSymbol typeSymbol) : base(semanticModel, typeSymbol) { }

        protected override SyntaxKind? GetSyntaxKind(IMethodSymbol symbol)
            => symbol switch
            {
                { Parameters: { Length: not 2 } } => null,
                { Name: "GreaterThan" } => SyntaxKind.GreaterThanExpression,
                { Name: "GreaterThanOrEqual" } => SyntaxKind.GreaterThanOrEqualExpression,
                { Name: "LessThan" } => SyntaxKind.LessThanExpression,
                { Name: "LessThanOrEqual" } => SyntaxKind.LessThanOrEqualExpression,
                _ => null,
            };
    }
}
