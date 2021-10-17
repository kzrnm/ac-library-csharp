using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AtCoderAnalyzer.CreateOperators
{
    internal abstract class OperatorEnumerateMember : EnumerateMember
    {
        protected OperatorEnumerateMember(ITypeSymbol typeSymbol) : base(typeSymbol) { }
        protected abstract SyntaxKind? GetSyntaxKind(IMethodSymbol symbol);

        protected override MethodDeclarationSyntax CreateMethodSyntax(IMethodSymbol symbol)
        {
            if (GetSyntaxKind(symbol) is SyntaxKind kind)
            {
                var operatorCall = BinaryExpression(
                    kind,
                    IdentifierName(symbol.Parameters[0].Name),
                    IdentifierName(symbol.Parameters[1].Name));
                return CreateMethodSyntax(symbol, ArrowExpressionClause(operatorCall));
            }
            return base.CreateMethodSyntax(symbol);
        }
    }
}
