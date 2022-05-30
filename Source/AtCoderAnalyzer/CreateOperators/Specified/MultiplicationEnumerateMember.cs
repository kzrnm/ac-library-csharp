using AtCoderAnalyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AtCoderAnalyzer.CreateOperators.Specified
{
    internal class MultiplicationEnumerateMember : OperatorEnumerateMember
    {
        internal MultiplicationEnumerateMember(SemanticModel semanticModel, ITypeSymbol typeSymbol, AtCoderAnalyzerConfig config) : base(semanticModel, typeSymbol, config) { }

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
                return PropertyDeclaration(symbol.Type.ToTypeSyntax(SemanticModel, SemanticModel.SyntaxTree.Length - 1), symbol.Name)
                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                    .WithExpressionBody(ArrowExpressionClause(
                        LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(1)))
                    )
                    .WithSemicolonToken(SyntaxHelpers.SemicolonToken);
            return base.CreatePropertySyntax(symbol);
        }
    }
}
