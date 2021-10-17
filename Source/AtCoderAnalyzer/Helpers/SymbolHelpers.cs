using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

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
            => ParseTypeName(symbol.ToDisplayString());

        public static ParameterSyntax ToParameterSyntax(this IParameterSymbol symbol)
        {
            var modifiers = symbol switch
            {
                { IsParams: true } => TokenList(Token(SyntaxKind.ParamsKeyword)),
                { RefKind: RefKind.Out } => TokenList(Token(SyntaxKind.OutKeyword)),
                { RefKind: RefKind.Ref } => TokenList(Token(SyntaxKind.RefKeyword)),
                { RefKind: RefKind.In } => TokenList(Token(SyntaxKind.InKeyword)),
                _ => TokenList(),
            };
            return Parameter(Identifier(symbol.Name))
                .WithModifiers(modifiers)
                .WithType(symbol.Type.ToTypeSyntax());
        }

        public static ParameterListSyntax ToParameterListSyntax(this IMethodSymbol symbol)
        {
            if (symbol.Parameters.Length == 0)
                return ParameterList();

            return ParameterList(SeparatedList(
                symbol.Parameters.Select(ToParameterSyntax)));
        }
    }
}
