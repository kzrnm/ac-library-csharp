using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtCoderAnalyzer.Helpers
{
    public static class SymbolHelpers
    {
        public static ITypeSymbol ReplaceGenericType(ITypeSymbol symbol,
            ImmutableDictionary<ITypeParameterSymbol, ITypeSymbol> convDic)
        {
            if (symbol is ITypeParameterSymbol typeParam)
                return convDic.TryGetValue(typeParam, out var newType) ? newType : typeParam;
            if (symbol is not INamedTypeSymbol named)
                return symbol;

            var typeArguments = named.TypeArguments;
            if (typeArguments.Length == 0)
                return symbol;
            var newTypeArguments = new ITypeSymbol[typeArguments.Length];
            for (int i = 0; i < newTypeArguments.Length; i++)
            {
                newTypeArguments[i] = ReplaceGenericType(typeArguments[i], convDic);
            }

            return named.ConstructedFrom.Construct(newTypeArguments);
        }

        public static TypeSyntax ToTypeSyntax(this ITypeSymbol symbol)
            => SyntaxFactory.ParseTypeName(symbol.ToDisplayString());

        public static ParameterSyntax ToParameterSyntax(this IParameterSymbol symbol)
        {
            var modifiers = symbol switch
            {
                { IsParams: true } => SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.ParamsKeyword)),
                { RefKind: RefKind.Out } => SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.OutKeyword)),
                { RefKind: RefKind.Ref } => SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.RefKeyword)),
                { RefKind: RefKind.In } => SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.InKeyword)),
                _ => SyntaxFactory.TokenList(),
            };
            return SyntaxFactory.Parameter(SyntaxFactory.Identifier(symbol.Name))
                .WithModifiers(modifiers)
                .WithType(symbol.Type.ToTypeSyntax());
        }

        public static ParameterListSyntax ToParameterListSyntax(this IMethodSymbol symbol)
        {
            if (symbol.Parameters.Length == 0)
                return SyntaxFactory.ParameterList();

            return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(
                symbol.Parameters.Select(ToParameterSyntax)));
        }
    }
}
