using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AtCoderAnalyzer.CreateOperators.Specified
{
    internal class UnaryNumEnumerateMember : EnumerateMember
    {
        internal UnaryNumEnumerateMember(SemanticModel semanticModel, ITypeSymbol typeSymbol) : base(semanticModel, typeSymbol) { }

        protected override MethodDeclarationSyntax CreateMethodSyntax(IMethodSymbol symbol)
        {
            if (symbol switch
            {
                { Parameters: { Length: not 1 } } => null,
                { Name: "Minus" } => SyntaxKind.UnaryMinusExpression,
                { Name: "Increment" } => SyntaxKind.PreIncrementExpression,
                { Name: "Decrement" } => SyntaxKind.PreDecrementExpression,
                _ => (SyntaxKind?)null,
            } is SyntaxKind kind)
            {
                var operatorCall = PrefixUnaryExpression(
                    kind,
                    IdentifierName(symbol.Parameters[0].Name));
                return CreateMethodSyntax(
                    SemanticModel, SemanticModel.SyntaxTree.Length - 1,
                    symbol, ArrowExpressionClause(operatorCall));
            }
            return base.CreateMethodSyntax(symbol);
        }
    }
}
