using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AtCoderAnalyzer.CreateOperators.Specified
{
    internal class DivisionEnumerateMember : OperatorEnumerateMember
    {
        internal DivisionEnumerateMember(SemanticModel semanticModel, ITypeSymbol typeSymbol) : base(semanticModel, typeSymbol) { }

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
