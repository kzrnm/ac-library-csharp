using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AtCoderAnalyzer.Diagnostics;
using AtCoderAnalyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static AtCoderAnalyzer.Helpers.Constants;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions;

namespace AtCoderAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AggressiveInliningCodeFixProvider)), Shared]
    public class AggressiveInliningCodeFixProvider : CodeFixProvider
    {
        private const string title = "Add AggressiveInlining";
        public override ImmutableArray<string> FixableDiagnosticIds
            => ImmutableArray.Create(
                DiagnosticDescriptors.AC0007_AgressiveInlining_Descriptor.Id);

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
                is not TypeDeclarationSyntax typeDeclarationSyntax)
                return;

            var action = CodeAction.Create(title: title,
               createChangedDocument: c => AddAggressiveInlining(context.Document, root, diagnosticSpan.Start, typeDeclarationSyntax, c),
               equivalenceKey: title);
            context.RegisterCodeFix(action, diagnostic);
        }

        private async Task<Document> AddAggressiveInlining(
            Document document, CompilationUnitSyntax root,
            int postion,
            TypeDeclarationSyntax typeDeclarationSyntax, CancellationToken cancellationToken)
        {
            root = root.ReplaceNode(typeDeclarationSyntax,
                            new AddAggressiveInliningRewriter(await document.GetSemanticModelAsync(cancellationToken), postion)
                            .Visit(typeDeclarationSyntax));

            return document.WithSyntaxRoot(root);
        }

        private class AddAggressiveInliningRewriter : CSharpSyntaxRewriter
        {
            private readonly SemanticModel semanticModel;
            private readonly int position;
            private readonly INamedTypeSymbol methodImplAttribute;
            private readonly INamedTypeSymbol methodImplOptions;
            private readonly AttributeSyntax aggressiveInliningAttribute;
            public AddAggressiveInliningRewriter(SemanticModel semanticModel, int position) : base(false)
            {
                this.semanticModel = semanticModel;
                this.position = position;
                methodImplAttribute = semanticModel.Compilation.GetTypeByMetadataName(
                    System_Runtime_CompilerServices_MethodImplAttribute);
                methodImplOptions = semanticModel.Compilation.GetTypeByMetadataName(
                    System_Runtime_CompilerServices_MethodImplOptions);
                aggressiveInliningAttribute = SyntaxHelpers.AggressiveInliningAttribute(methodImplAttribute.ToMinimalDisplayString(semanticModel, position), methodImplOptions.ToMinimalDisplayString(semanticModel, position));
            }

            public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                if (methodImplAttribute is null)
                    return node;
                if (methodImplOptions is null)
                    return node;
                if (semanticModel.GetDeclaredSymbol(node) is not IMethodSymbol m)
                    return node;

                if (m.MethodKind is
                     not (MethodKind.ExplicitInterfaceImplementation or MethodKind.Ordinary))
                    return node;

                if (m.GetAttributes()
                       .FirstOrDefault(at => SymbolEqualityComparer.Default.Equals(at.AttributeClass, methodImplAttribute)) is not { } attr
                    || attr.ApplicationSyntaxReference?.GetSyntax() is not AttributeSyntax syntax)
                    return node.AddAttributeLists(AttributeList(SingletonSeparatedList(aggressiveInliningAttribute)));

                if (attr.ConstructorArguments.Length > 0)
                {
                    var arg = attr.ConstructorArguments[0];
                    if (arg.Kind is TypedConstantKind.Primitive or TypedConstantKind.Enum)
                        try
                        {
                            if (((MethodImplOptions)Convert.ToInt32(arg.Value)).HasFlag(MethodImplOptions.AggressiveInlining))
                                return node;
                        }
                        catch
                        {
                        }
                }

                var list = new List<AttributeListSyntax>(node.AttributeLists.Count);
                foreach (var attributeList in node.AttributeLists)
                {
                    if (attributeList.Attributes.Contains(syntax))
                    {
                        var replaced = attributeList.Attributes.Replace(syntax, AddAggressiveInlining(syntax, attr));
                        list.Add(AttributeList(replaced));
                    }
                    else
                        list.Add(attributeList);
                }
                return node.WithAttributeLists(new SyntaxList<AttributeListSyntax>(list));
            }


            private AttributeSyntax AddAggressiveInlining(AttributeSyntax syntax, AttributeData attributeData)
            {
                if (attributeData.ConstructorArguments.Length == 0)
                {
                    return aggressiveInliningAttribute;
                }
                else
                {
                    var argConst = attributeData.ConstructorArguments[0];
                    var argSyntax = syntax.ArgumentList.Arguments[0];

                    if (argSyntax.NameEquals == null)
                    {
                        AttributeArgumentSyntax arg;
                        if (argConst.Type.SpecialType is SpecialType.System_Int16)
                            arg = argSyntax.WithExpression(
                                BinaryExpression(
                                    SyntaxKind.BitwiseOrExpression, argSyntax.Expression, LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(256))));
                        else if (SymbolEqualityComparer.Default.Equals(argConst.Type, methodImplOptions))
                            arg = argSyntax.WithExpression(
                                BinaryExpression(
                                    SyntaxKind.BitwiseOrExpression, argSyntax.Expression, SyntaxHelpers.AggressiveInliningMember(methodImplOptions.ToMinimalDisplayString(semanticModel, position))));
                        else
                            throw new InvalidProgramException("invalid MethodImplAttribute argument");

                        return syntax.WithArgumentList(
                            AttributeArgumentList(
                                syntax.ArgumentList.Arguments.Replace(syntax.ArgumentList.Arguments[0], arg)));
                    }
                    else
                    {
                        return syntax.WithArgumentList(
                            AttributeArgumentList(
                                syntax.ArgumentList.Arguments.Insert(0, AttributeArgument(SyntaxHelpers.AggressiveInliningMember(methodImplOptions.ToMinimalDisplayString(semanticModel, position))))));
                    }
                }
            }
        }
    }
}
