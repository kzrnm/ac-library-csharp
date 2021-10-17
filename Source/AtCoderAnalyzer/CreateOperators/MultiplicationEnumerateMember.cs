using AtCoderAnalyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AtCoderAnalyzer.CreateOperators
{
    internal class MultiplicationEnumerateMember : OperatorEnumerateMember
    {
        internal MultiplicationEnumerateMember(ITypeSymbol typeSymbol) : base(typeSymbol) { }

        protected override SyntaxKind? GetSyntaxKind(IMethodSymbol symbol)
            => symbol switch
            {
                { Parameters: { Length: not 2 } } => null,
                { Name: "Multiply" } => SyntaxKind.MultiplyExpression,
                _ => null,
            };

        protected override PropertyDeclarationSyntax CreatePropertySyntax(IPropertySymbol symbol)
        {
            if (symbol.Name == "MultiplyIdentity")
                return PropertyDeclaration(symbol.Type.ToTypeSyntax(), symbol.Name)
                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                    .WithExpressionBody(ArrowExpressionClause(
                        LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(1)))
                    )
                    .WithSemicolonToken(SyntaxHelpers.SemicolonToken);
            return base.CreatePropertySyntax(symbol);
        }
    }
}
