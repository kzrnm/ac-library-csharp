using System.Collections.Generic;
using AtCoderAnalyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AtCoderAnalyzer.CreateOperators
{
    internal class EnumerateMember
    {
        protected ITypeSymbol TypeSymbol { get; }
        protected EnumerateMember(ITypeSymbol typeSymbol)
        {
            TypeSymbol = typeSymbol;
        }
        public static EnumerateMember Create(ITypeSymbol typeSymbol)
        {
            return typeSymbol.OriginalDefinition.ToDisplayString() switch
            {
                "AtCoder.Operators.IAdditionOperator<T>" => new AdditionEnumerateMember(typeSymbol),
                "AtCoder.Operators.ISubtractOperator<T>" => new SubtractEnumerateMember(typeSymbol),
                "AtCoder.Operators.IMultiplicationOperator<T>" => new MultiplicationEnumerateMember(typeSymbol),
                "AtCoder.Operators.IDivisionOperator<T>" => new DivisionEnumerateMember(typeSymbol),
                "AtCoder.Operators.IUnaryNumOperator<T>" => new UnaryNumEnumerateMember(typeSymbol),
                "AtCoder.Operators.ICompareOperator<T>" => new CompareOperatorEnumerateMember(typeSymbol),
                "AtCoder.Operators.IMinMaxValue<T>" => new MinMaxValueEnumerateMember(typeSymbol),
                "AtCoder.Operators.IShiftOperator<T>" => new ShiftEnumerateMember(typeSymbol),
                "AtCoder.Operators.ICastOperator<TFrom, TTo>" => new CastEnumerateMember(typeSymbol),
                "System.Collections.Generic.IComparer<T>" => new ComparerEnumerateMember(typeSymbol),
                _ => new EnumerateMember(typeSymbol),
            };
        }

        public IEnumerable<(MemberDeclarationSyntax Syntax, bool IsMethod)> EnumerateMemberSyntax()
        {
            foreach (var member in TypeSymbol.GetMembers())
            {
                if (!member.IsAbstract)
                    continue;
                if (member is IPropertySymbol property)
                {
                    yield return (CreatePropertySyntax(property), false);
                }
                else if (member is IMethodSymbol method && method.MethodKind == MethodKind.Ordinary)
                {
                    yield return (CreateMethodSyntax(method), true);
                }
            }
        }

        protected virtual PropertyDeclarationSyntax CreatePropertySyntax(IPropertySymbol symbol)
        {
            var dec = PropertyDeclaration(symbol.Type.ToTypeSyntax(), symbol.Name)
                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)));

            if (symbol.SetMethod == null)
                return dec
                    .WithExpressionBody(SyntaxHelpers.ArrowDefault)
                    .WithSemicolonToken(SyntaxHelpers.SemicolonToken);
            else if (symbol.GetMethod == null)
                return dec.AddAccessorListAccessors(new AccessorDeclarationSyntax[]
                {
                    AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxHelpers.SemicolonToken),
                });

            return dec.AddAccessorListAccessors(new AccessorDeclarationSyntax[]
            {
                AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxHelpers.SemicolonToken),
                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxHelpers.SemicolonToken)
            });
        }
        protected virtual MethodDeclarationSyntax CreateMethodSyntax(IMethodSymbol symbol)
        {
            if (symbol.ReturnsVoid)
                return CreateMethodSyntax(symbol, Block());
            else
                return CreateMethodSyntax(symbol, SyntaxHelpers.ArrowDefault);
        }

        private static MethodDeclarationSyntax CommonMethodDeclaration(IMethodSymbol symbol)
            => MethodDeclaration(symbol.ReturnType.ToTypeSyntax(), symbol.Name)
                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                    .WithAttributeLists(SingletonList(SyntaxHelpers.AggressiveInliningAttributeList))
                    .WithParameterList(symbol.ToParameterListSyntax());
        protected static MethodDeclarationSyntax CreateMethodSyntax(IMethodSymbol symbol, BlockSyntax block)
        {
            return CommonMethodDeclaration(symbol)
                .WithBody(block);
        }
        protected static MethodDeclarationSyntax CreateMethodSyntax(IMethodSymbol symbol, ArrowExpressionClauseSyntax arrowExpressionClause)
        {
            return CommonMethodDeclaration(symbol)
                .WithExpressionBody(arrowExpressionClause)
                .WithSemicolonToken(SyntaxHelpers.SemicolonToken);
        }
    }
}
