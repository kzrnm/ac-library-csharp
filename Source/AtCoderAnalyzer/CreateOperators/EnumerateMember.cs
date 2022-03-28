﻿using System.Collections.Generic;
using AtCoderAnalyzer.CreateOperators.Specified;
using AtCoderAnalyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AtCoderAnalyzer.CreateOperators
{
    internal class EnumerateMember
    {
        protected SemanticModel SemanticModel { get; }
        protected ITypeSymbol TypeSymbol { get; }
        private readonly INamedTypeSymbol methodImplAttribute;
        private readonly INamedTypeSymbol methodImplOptions;
        protected EnumerateMember(SemanticModel semanticModel, ITypeSymbol typeSymbol)
        {
            SemanticModel = semanticModel;
            TypeSymbol = typeSymbol;
            methodImplAttribute = semanticModel.Compilation.GetTypeByMetadataName(
                    Constants.System_Runtime_CompilerServices_MethodImplAttribute);
            methodImplOptions = semanticModel.Compilation.GetTypeByMetadataName(
                Constants.System_Runtime_CompilerServices_MethodImplOptions);
        }
        public static EnumerateMember Create(SemanticModel semanticModel, ITypeSymbol typeSymbol) => typeSymbol.OriginalDefinition.ToDisplayString() switch
        {
            "AtCoder.Operators.IAdditionOperator<T>" => new AdditionEnumerateMember(semanticModel, typeSymbol),
            "AtCoder.Operators.ISubtractOperator<T>" => new SubtractEnumerateMember(semanticModel, typeSymbol),
            "AtCoder.Operators.IMultiplicationOperator<T>" => new MultiplicationEnumerateMember(semanticModel, typeSymbol),
            "AtCoder.Operators.IDivisionOperator<T>" => new DivisionEnumerateMember(semanticModel, typeSymbol),
            "AtCoder.Operators.IUnaryNumOperator<T>" => new UnaryNumEnumerateMember(semanticModel, typeSymbol),
            "AtCoder.Operators.ICompareOperator<T>" => new CompareOperatorEnumerateMember(semanticModel, typeSymbol),
            "AtCoder.Operators.IMinMaxValue<T>" => new MinMaxValueEnumerateMember(semanticModel, typeSymbol),
            "AtCoder.Operators.IShiftOperator<T>" => new ShiftEnumerateMember(semanticModel, typeSymbol),
            "AtCoder.Operators.ICastOperator<TFrom, TTo>" => new CastEnumerateMember(semanticModel, typeSymbol),
            "System.Collections.Generic.IComparer<T>" => new ComparerEnumerateMember(semanticModel, typeSymbol),
            _ => new EnumerateMember(semanticModel, typeSymbol),
        };

        public IEnumerable<(MemberDeclarationSyntax Syntax, bool IsMethod)> EnumerateMemberSyntax()
        {
            foreach (var member in TypeSymbol.GetMembers())
            {
                if (!member.IsAbstract)
                    continue;
                if (member is IPropertySymbol property)
                    yield return (CreatePropertySyntax(property), false);
                else if (member is IMethodSymbol method && method.MethodKind == MethodKind.Ordinary)
                    yield return (CreateMethodSyntax(method), true);
            }
        }

        protected virtual PropertyDeclarationSyntax CreatePropertySyntax(IPropertySymbol symbol)
        {
            var dec = PropertyDeclaration(symbol.Type.ToTypeSyntax(SemanticModel, SemanticModel.SyntaxTree.Length - 1), symbol.Name)
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
                return CreateMethodSyntax(SemanticModel, SemanticModel.SyntaxTree.Length - 1, symbol, Block());
            else
                return CreateMethodSyntax(SemanticModel, SemanticModel.SyntaxTree.Length - 1, symbol, SyntaxHelpers.ArrowDefault);
        }

        private MethodDeclarationSyntax CommonMethodDeclaration(IMethodSymbol symbol, SemanticModel semanticModel, int position)
            => MethodDeclaration(symbol.ReturnType.ToTypeSyntax(semanticModel, semanticModel.SyntaxTree.Length), symbol.Name)
                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                    .WithAttributeLists(SingletonList(
                        AttributeList(
                                SingletonSeparatedList(
                                    SyntaxHelpers.AggressiveInliningAttribute(
                                        methodImplAttribute.ToMinimalDisplayString(semanticModel, position),
                                        methodImplOptions.ToMinimalDisplayString(semanticModel, position))))))
                    .WithParameterList(symbol.ToParameterListSyntax(semanticModel, semanticModel.SyntaxTree.Length));
        protected MethodDeclarationSyntax CreateMethodSyntax(SemanticModel semanticModel, int position, IMethodSymbol symbol, BlockSyntax block)
        {
            return CommonMethodDeclaration(symbol, semanticModel, position)
                .WithBody(block);
        }
        protected MethodDeclarationSyntax CreateMethodSyntax(SemanticModel semanticModel, int position, IMethodSymbol symbol, ArrowExpressionClauseSyntax arrowExpressionClause)
        {
            return CommonMethodDeclaration(symbol, semanticModel, position)
                .WithExpressionBody(arrowExpressionClause)
                .WithSemicolonToken(SyntaxHelpers.SemicolonToken);
        }
    }
}
