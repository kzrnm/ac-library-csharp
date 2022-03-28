using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AtCoderAnalyzer.CreateOperators.Specified
{
    internal class AdditionEnumerateMember : OperatorEnumerateMember
    {
        internal AdditionEnumerateMember(ITypeSymbol typeSymbol) : base(typeSymbol) { }

        protected override SyntaxKind? GetSyntaxKind(IMethodSymbol symbol)
            => symbol switch
            {
                { Parameters: { Length: not 2 } } => null,
                { Name: "Add" } => SyntaxKind.AddExpression,
                _ => null,
            };
    }
}
