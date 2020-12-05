﻿using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AtCoderAnalyzer.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtCoderAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CreateOperatorCodeFixProvider)), Shared]
    public class CreateOperatorCodeFixProvider : CodeFixProvider
    {
        private const string title = "Create operator type";
        private const string System_Runtime_CompilerServices = "System.Runtime.CompilerServices";
        public override ImmutableArray<string> FixableDiagnosticIds
            => ImmutableArray.Create(
                DiagnosticDescriptors.AC0003_StaticModInt.Id,
                DiagnosticDescriptors.AC0004_DynamicModInt.Id,
                DiagnosticDescriptors.AC0005_SegtreeOperator.Id,
                DiagnosticDescriptors.AC0006_LazySegtreeOperator.Id);
        public sealed override FixAllProvider GetFixAllProvider()
            => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            if (await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false)
                is not CompilationUnitSyntax root)
                return;
            var diagnostic = context.Diagnostics[0];
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            if (root.FindNode(diagnosticSpan)
                is not GenericNameSyntax genericNode)
                return;
            var operatorTypeSyntax = genericNode.TypeArgumentList.Arguments[genericNode.TypeArgumentList.Arguments.Count - 1];
            var operatorTypeName = operatorTypeSyntax.ToString();

            var action = CodeAction.Create(title: title,
               createChangedDocument: c => CreateOperatorType(context.Document,
               diagnostic.Id, genericNode.TypeArgumentList.Arguments, c),
               equivalenceKey: title);
            context.RegisterCodeFix(action, diagnostic);
        }
        private async Task<Document> CreateOperatorType(
            Document document,
            string diagnosticId,
            SeparatedSyntaxList<TypeSyntax> genericArgs,
            CancellationToken cancellationToken)
        {
            if (await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false)
                is not CompilationUnitSyntax root)
                return document;

            return document.WithSyntaxRoot(diagnosticId switch
            {
                "AC0003" => AC0003_StaticModInt(root, genericArgs),
                "AC0004" => AC0004_DynamicModInt(root, genericArgs),
                "AC0005" => AC0005_SegtreeOperator(root, genericArgs),
                "AC0006" => AC0006_LazySegtreeOperator(root, genericArgs),
                _ => root,
            });
        }
        private static SimpleBaseTypeSyntax ParseAtCoder(bool hasUsing, string text, params TypeSyntax[] genericTypes)
        {
            SimpleNameSyntax type;
            if (genericTypes.Length == 0)
            {
                type = SyntaxFactory.ParseName(text) as SimpleNameSyntax;
            }
            else
            {
                type = SyntaxFactory.GenericName(text).AddTypeArgumentListArguments(genericTypes);
            }

            if (!hasUsing)
            {
                return SyntaxFactory.SimpleBaseType(
                    SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName("AtCoder"), type));
            }
            return SyntaxFactory.SimpleBaseType(type);
        }
        private SyntaxNode AC0003_StaticModInt(CompilationUnitSyntax root, SeparatedSyntaxList<TypeSyntax> genericArgs)
        {
            var operatorTypeName = genericArgs[0].ToString();
            var hasUsing = root.Usings.Select(sy => sy.Name.ToString()).Any(n => n == "AtCoder");
            var newMember =
                SyntaxFactory.StructDeclaration(operatorTypeName)
                .AddBaseListTypes(
                    ParseAtCoder(hasUsing, "IStaticMod")
                    )
                .AddMembers(
                    SyntaxFactory.PropertyDeclaration(
                        SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.UIntKeyword)),
                        "Mod"
                    ).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithExpressionBody(
                        SyntaxFactory.ArrowExpressionClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(1000000007)))
                        )
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.PropertyDeclaration(
                        SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
                        "IsPrime"
                    ).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithExpressionBody(
                        SyntaxFactory.ArrowExpressionClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression))
                        )
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                );
            return root.AddMembers(newMember);
        }
        private SyntaxNode AC0004_DynamicModInt(CompilationUnitSyntax root, SeparatedSyntaxList<TypeSyntax> genericArgs)
        {
            var operatorTypeName = genericArgs[0].ToString();
            var hasUsing = root.Usings.Select(sy => sy.Name.ToString()).Any(n => n == "AtCoder");
            return root.AddMembers(
                SyntaxFactory.StructDeclaration(operatorTypeName)
                .AddBaseListTypes(
                    ParseAtCoder(hasUsing, "IDynamicModID")
                    ));
        }
        private readonly AttributeListSyntax AggressiveInliningAttribute
            = SyntaxFactory.AttributeList(
                SyntaxFactory.SeparatedList(new[] {
                    SyntaxFactory.Attribute(
                        SyntaxFactory.ParseName("MethodImpl"))
                    .AddArgumentListArguments(
                        SyntaxFactory.AttributeArgument(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("MethodImplOptions"),
                                SyntaxFactory.IdentifierName("AggressiveInlining")
                        )))
                }));
        private CompilationUnitSyntax AddSystem_Runtime_CompilerServicesSyntax(CompilationUnitSyntax root)
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
        private SyntaxNode AC0005_SegtreeOperator(CompilationUnitSyntax root, SeparatedSyntaxList<TypeSyntax> genericArgs)
        {
            var operatorTypeName = genericArgs[1].ToString();
            bool hasUsing = false;
            bool hasSystem_Runtime_CompilerServices = false;

            foreach (var sy in root.Usings)
            {
                var name = sy.Name.ToString();
                if (name == "AtCoder")
                    hasUsing = true;
                if (name == System_Runtime_CompilerServices)
                    hasSystem_Runtime_CompilerServices = true;
            }
            if (!hasSystem_Runtime_CompilerServices)
                root = AddSystem_Runtime_CompilerServicesSyntax(root);

            var segType = genericArgs[0];
            var baseType = ParseAtCoder(hasUsing, "ISegtreeOperator", segType);
            var newMember =
                SyntaxFactory.StructDeclaration(operatorTypeName)
                .AddBaseListTypes(baseType)
                .AddMembers(
                    SyntaxFactory.PropertyDeclaration(segType, "Identity")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithExpressionBody(
                        SyntaxFactory.ArrowExpressionClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression))
                        )
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.MethodDeclaration(segType, "Operate")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithParameterList(
                        SyntaxFactory.ParameterList(
                            SyntaxFactory.SeparatedList(new[] {
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("x")).WithType(segType),
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("y")).WithType(segType),
                            })
                            ))
                    .AddAttributeLists(AggressiveInliningAttribute)
                    .WithBody(SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression))
                            )
                        )
                );
            return root.AddMembers(newMember);
        }
        private SyntaxNode AC0006_LazySegtreeOperator(CompilationUnitSyntax root, SeparatedSyntaxList<TypeSyntax> genericArgs)
        {
            var operatorTypeName = genericArgs[2].ToString();
            bool hasUsing = false;
            bool hasSystem_Runtime_CompilerServices = false;

            foreach (var sy in root.Usings)
            {
                var name = sy.Name.ToString();
                if (name == "AtCoder")
                    hasUsing = true;
                if (name == System_Runtime_CompilerServices)
                    hasSystem_Runtime_CompilerServices = true;
            }
            if (!hasSystem_Runtime_CompilerServices)
                root = AddSystem_Runtime_CompilerServicesSyntax(root);

            var segType = genericArgs[0];
            var funType = genericArgs[1];
            var baseType = ParseAtCoder(hasUsing, "ILazySegtreeOperator", segType, funType);

            var newMember =
                SyntaxFactory.StructDeclaration(operatorTypeName)
                .AddBaseListTypes(baseType)
                .AddMembers(
                    SyntaxFactory.PropertyDeclaration(segType, "Identity")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithExpressionBody(
                        SyntaxFactory.ArrowExpressionClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression))
                        )
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.PropertyDeclaration(funType, "FIdentity")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithExpressionBody(
                        SyntaxFactory.ArrowExpressionClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression))
                        )
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),

                    SyntaxFactory.MethodDeclaration(funType, "Composition")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithParameterList(
                        SyntaxFactory.ParameterList(
                            SyntaxFactory.SeparatedList(new[] {
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("f")).WithType(funType),
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("g")).WithType(funType),
                            })
                            ))
                    .AddAttributeLists(AggressiveInliningAttribute)
                    .WithBody(SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression))
                            )
                        ),

                    SyntaxFactory.MethodDeclaration(segType, "Mapping")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithParameterList(
                        SyntaxFactory.ParameterList(
                            SyntaxFactory.SeparatedList(new[] {
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("f")).WithType(funType),
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("x")).WithType(segType),
                            })
                            ))
                    .AddAttributeLists(AggressiveInliningAttribute)
                    .WithBody(SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression))
                            )
                        ),

                    SyntaxFactory.MethodDeclaration(segType, "Operate")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithParameterList(
                        SyntaxFactory.ParameterList(
                            SyntaxFactory.SeparatedList(new[] {
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("x")).WithType(segType),
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("y")).WithType(segType),
                            })
                            ))
                    .AddAttributeLists(AggressiveInliningAttribute)
                    .WithBody(SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression))
                            )
                        )
                );
            return root.AddMembers(newMember);
        }
    }
}
