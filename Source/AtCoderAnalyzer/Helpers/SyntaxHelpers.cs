﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AtCoderAnalyzer.Helpers
{
    internal static class SyntaxHelpers
    {
        public static ImmutableHashSet<string> ToNamespaceHashSet(this SyntaxList<UsingDirectiveSyntax> usings)
            => usings
                .Where(sy => sy.Alias == null && sy.StaticKeyword.IsKind(SyntaxKind.None))
                .Select(sy => sy.Name.ToString().Trim())
                .ToImmutableHashSet();

        public static SeparatedSyntaxList<TNode> ToSeparatedSyntaxList<TNode>(this IEnumerable<TNode> nodes) where TNode : SyntaxNode
            => SeparatedList(nodes);
        public static CompilationUnitSyntax AddSystem_Runtime_CompilerServicesSyntax(CompilationUnitSyntax root)
        {
            return root.AddUsings(
                UsingDirective(
                    QualifiedName(
                        QualifiedName(
                            IdentifierName("System"),
                            IdentifierName("Runtime")
                        ),
                        IdentifierName("CompilerServices")
                    )));
        }

        public static MemberAccessExpressionSyntax AggressiveInliningMember(string methodImplOptions)
            => MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                ParseExpression(methodImplOptions), IdentifierName("AggressiveInlining"));
        public static AttributeSyntax AggressiveInliningAttribute(string methodImplAttribute, string methodImplOptions)
            => Attribute(
                ParseName(methodImplAttribute.EndsWith("Attribute")
                    ? methodImplAttribute.Substring(0, methodImplAttribute.Length - "Attribute".Length)
                    : methodImplAttribute)
            ).AddArgumentListArguments(AttributeArgument(AggressiveInliningMember(methodImplOptions)));
        public static AttributeListSyntax AggressiveInliningAttributeList(string methodImplAttribute, string methodImplOptions)
            => AttributeList(SingletonSeparatedList(AggressiveInliningAttribute(methodImplAttribute, methodImplOptions)));

        public static readonly ArrowExpressionClauseSyntax ArrowDefault
            = ArrowExpressionClause(
                LiteralExpression(SyntaxKind.DefaultLiteralExpression));

        public static readonly SyntaxToken SemicolonToken = Token(SyntaxKind.SemicolonToken);
    }
}
