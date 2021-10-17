using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AtCoderAnalyzer.CreateOperators
{
    internal class DivisionEnumerateMember : OperatorEnumerateMember
    {
        internal DivisionEnumerateMember(ITypeSymbol typeSymbol) : base(typeSymbol) { }

        protected override SyntaxKind? GetSyntaxKind(IMethodSymbol symbol)
            => symbol switch
            {
                { Parameters: { Length: not 2 } } => null,
                { Name: "Divide" } => SyntaxKind.DivideExpression,
                { Name: "Modulo" } => SyntaxKind.ModuloExpression,
                _ => null,
            };
    }
}
