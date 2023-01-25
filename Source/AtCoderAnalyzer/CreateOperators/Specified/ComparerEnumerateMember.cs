using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AtCoderAnalyzer.CreateOperators.Specified
{
    internal class ComparerEnumerateMember : EnumerateMember
    {
        internal ComparerEnumerateMember(SemanticModel semanticModel, ITypeSymbol typeSymbol, AtCoderAnalyzerConfig config) : base(semanticModel, typeSymbol, config) { }
        protected override MethodDeclarationSyntax CreateMethodSyntax(IMethodSymbol symbol, bool isStatic)
        {
            if (symbol is
                {
                    Name: nameof(IComparer<int>.Compare),
                    Parameters: { Length: 2 }
                })
            {
                var caller = MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(symbol.Parameters[0].Name),
                    IdentifierName(nameof(IComparable<int>.CompareTo)));
                var args = ArgumentList(SingletonSeparatedList(Argument(IdentifierName(symbol.Parameters[1].Name))));
                var invocation = InvocationExpression(caller, args);
                return CreateMethodSyntax(
                    SemanticModel, SemanticModel.SyntaxTree.Length - 1,
                    symbol, isStatic, ArrowExpressionClause(invocation));
            }
            return base.CreateMethodSyntax(symbol, isStatic);
        }
    }
}
