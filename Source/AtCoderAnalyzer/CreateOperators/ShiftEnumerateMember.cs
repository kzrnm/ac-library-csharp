using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AtCoderAnalyzer.CreateOperators
{
    internal class ShiftEnumerateMember : OperatorEnumerateMember
    {
        internal ShiftEnumerateMember(ITypeSymbol typeSymbol) : base(typeSymbol) { }

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
