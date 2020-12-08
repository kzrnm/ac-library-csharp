using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtCoderAnalyzer.Helpers
{
    internal static class SyntaxHelpers
    {
        public static ImmutableHashSet<string> ToNamespaceHashSet(this SyntaxList<UsingDirectiveSyntax> usings)
            => usings
                .Where(sy => sy.Alias == null && sy.StaticKeyword.IsKind(SyntaxKind.None))
                .Select(sy => sy.Name.ToString().Trim())
                .ToImmutableHashSet();

        public static CompilationUnitSyntax AddSystem_Runtime_CompilerServicesSyntax(CompilationUnitSyntax root)
        {
            return root.AddUsings(
                SyntaxFactory.UsingDirective(
                    SyntaxFactory.QualifiedName(
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.IdentifierName("System"),
                            SyntaxFactory.IdentifierName("Runtime")
                        ),
                        SyntaxFactory.IdentifierName("CompilerServices")
                    )));
        }

        public static readonly AttributeSyntax AggressiveInliningAttribute
            = SyntaxFactory.Attribute(SyntaxFactory.ParseName("MethodImpl"))
                    .AddArgumentListArguments(
                        SyntaxFactory.AttributeArgument(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("MethodImplOptions"),
                                SyntaxFactory.IdentifierName("AggressiveInlining")
                        )));

        public static readonly AttributeListSyntax AggressiveInliningAttributeList
            = SyntaxFactory.AttributeList(
                SyntaxFactory.SeparatedList(new[] { AggressiveInliningAttribute }));

        public static readonly ArrowExpressionClauseSyntax ArrowDefault
            = SyntaxFactory.ArrowExpressionClause(
                SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression));

        public static readonly SyntaxToken SemicolonToken
            = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
    }
}
