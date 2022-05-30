using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AtCoderAnalyzer.CreateOperators.Specified
{
    internal class ShiftEnumerateMember : OperatorEnumerateMember
    {
        internal ShiftEnumerateMember(SemanticModel semanticModel, ITypeSymbol typeSymbol, AtCoderAnalyzerConfig config) : base(semanticModel, typeSymbol, config) { }
        protected override SyntaxKind? GetSyntaxKind(IMethodSymbol symbol)
            => symbol switch
            {
                { Parameters: { Length: not 2 } } => null,
                { Name: "LeftShift" } => SyntaxKind.LeftShiftExpression,
                { Name: "RightShift" } => SyntaxKind.RightShiftExpression,
                _ => null,
            };
    }
}
