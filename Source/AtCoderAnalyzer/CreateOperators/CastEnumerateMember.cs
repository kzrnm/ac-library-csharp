using AtCoderAnalyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AtCoderAnalyzer.CreateOperators
{
    internal class CastEnumerateMember : EnumerateMember
    {
        internal CastEnumerateMember(ITypeSymbol typeSymbol) : base(typeSymbol) { }
        protected override MethodDeclarationSyntax CreateMethodSyntax(IMethodSymbol symbol)
        {
            if (symbol is
                {
                    Name: "Cast",
                    Parameters: { Length: 1 }
                })
            {
                return CreateMethodSyntax(
                    symbol,
                    ArrowExpressionClause(CastExpression(symbol.ReturnType.ToTypeSyntax(), IdentifierName(symbol.Parameters[0].Name))));
            }
            return base.CreateMethodSyntax(symbol);
        }
    }
}
