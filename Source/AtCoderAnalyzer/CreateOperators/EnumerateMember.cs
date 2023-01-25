using System.Collections.Generic;
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
        private readonly AtCoderAnalyzerConfig config;
        protected EnumerateMember(SemanticModel semanticModel, ITypeSymbol typeSymbol, AtCoderAnalyzerConfig config)
        {
            SemanticModel = semanticModel;
            TypeSymbol = typeSymbol;
            methodImplAttribute = semanticModel.Compilation.GetTypeByMetadataName(
                    Constants.System_Runtime_CompilerServices_MethodImplAttribute);
            methodImplOptions = semanticModel.Compilation.GetTypeByMetadataName(
                Constants.System_Runtime_CompilerServices_MethodImplOptions);
            this.config = config;
        }
        public static EnumerateMember Create(SemanticModel semanticModel, ITypeSymbol typeSymbol, AtCoderAnalyzerConfig config) => typeSymbol.OriginalDefinition.ToDisplayString() switch
        {
            "AtCoder.Operators.IAdditionOperator<T>" => new AdditionEnumerateMember(semanticModel, typeSymbol, config),
            "AtCoder.Operators.ISubtractOperator<T>" => new SubtractEnumerateMember(semanticModel, typeSymbol, config),
            "AtCoder.Operators.IMultiplicationOperator<T>" => new MultiplicationEnumerateMember(semanticModel, typeSymbol, config),
            "AtCoder.Operators.IDivisionOperator<T>" => new DivisionEnumerateMember(semanticModel, typeSymbol, config),
            "AtCoder.Operators.IUnaryNumOperator<T>" => new UnaryNumEnumerateMember(semanticModel, typeSymbol, config),
            "AtCoder.Operators.ICompareOperator<T>" => new CompareOperatorEnumerateMember(semanticModel, typeSymbol, config),
            "AtCoder.Operators.IMinMaxValue<T>" => new MinMaxValueEnumerateMember(semanticModel, typeSymbol, config),
            "AtCoder.Operators.IShiftOperator<T>" => new ShiftEnumerateMember(semanticModel, typeSymbol, config),
            "AtCoder.Operators.ICastOperator<TFrom, TTo>" => new CastEnumerateMember(semanticModel, typeSymbol, config),
            "System.Collections.Generic.IComparer<T>" => new ComparerEnumerateMember(semanticModel, typeSymbol, config),
            _ => new EnumerateMember(semanticModel, typeSymbol, config),
        };

        public IEnumerable<(MemberDeclarationSyntax Syntax, bool IsMethod)> EnumerateMemberSyntax()
        {
            foreach (var member in TypeSymbol.GetMembers())
            {
                if (!member.IsAbstract)
                    continue;
                if (member is IPropertySymbol property)
                    yield return (CreatePropertySyntax(property, member.IsStatic), false);
                else if (member is IMethodSymbol method && method.MethodKind == MethodKind.Ordinary)
                    yield return (CreateMethodSyntax(method, member.IsStatic), true);
            }
        }

        private static SyntaxTokenList PublicModifiers(bool isStatic)
            => isStatic ? TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)) : TokenList(Token(SyntaxKind.PublicKeyword));

        protected virtual PropertyDeclarationSyntax CreatePropertySyntax(IPropertySymbol symbol, bool isStatic)
        {
            var dec = PropertyDeclaration(symbol.Type.ToTypeSyntax(SemanticModel, SemanticModel.SyntaxTree.Length - 1), symbol.Name)
                    .WithModifiers(PublicModifiers(isStatic));

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
        protected virtual MethodDeclarationSyntax CreateMethodSyntax(IMethodSymbol symbol, bool isStatic)
        {
            if (symbol.ReturnsVoid)
                return CreateMethodSyntax(SemanticModel, SemanticModel.SyntaxTree.Length - 1, symbol, isStatic, Block());
            else
                return CreateMethodSyntax(SemanticModel, SemanticModel.SyntaxTree.Length - 1, symbol, isStatic, SyntaxHelpers.ArrowDefault);
        }

        private MethodDeclarationSyntax CommonMethodDeclaration(IMethodSymbol symbol, SemanticModel semanticModel, int position, bool isStatic)
            => MethodDeclaration(symbol.ReturnType.ToTypeSyntax(semanticModel, semanticModel.SyntaxTree.Length), symbol.Name)
                    .WithModifiers(PublicModifiers(isStatic))
                    .WithAttributeLists(SingletonList(
                        AttributeList(
                                SingletonSeparatedList(
                                    SyntaxHelpers.AggressiveInliningAttribute(
                                        methodImplAttribute.ToMinimalDisplayString(semanticModel, position),
                                        methodImplOptions.ToMinimalDisplayString(semanticModel, position),
                                        config.UseMethodImplNumeric)))))
                    .WithParameterList(symbol.ToParameterListSyntax(semanticModel, semanticModel.SyntaxTree.Length));
        protected MethodDeclarationSyntax CreateMethodSyntax(SemanticModel semanticModel, int position, IMethodSymbol symbol, bool isStatic, BlockSyntax block)
        {
            return CommonMethodDeclaration(symbol, semanticModel, position, isStatic)
                .WithBody(block);
        }
        protected MethodDeclarationSyntax CreateMethodSyntax(SemanticModel semanticModel, int position, IMethodSymbol symbol, bool isStatic, ArrowExpressionClauseSyntax arrowExpressionClause)
        {
            return CommonMethodDeclaration(symbol, semanticModel, position, isStatic)
                .WithExpressionBody(arrowExpressionClause)
                .WithSemicolonToken(SyntaxHelpers.SemicolonToken);
        }
    }
}
