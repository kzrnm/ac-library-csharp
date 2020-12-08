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

namespace AtCoderAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AggressiveInliningCodeFixProvider)), Shared]
    public class AggressiveInliningCodeFixProvider : CodeFixProvider
    {
        private const string title = "Add AggressiveInlining";
        public override ImmutableArray<string> FixableDiagnosticIds
            => ImmutableArray.Create(
                DiagnosticDescriptors.AC0007_AgressiveInlining.Id);

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
               createChangedDocument: c => AddAggressiveInlining(context.Document, root, typeDeclarationSyntax, c),
               equivalenceKey: title);
            context.RegisterCodeFix(action, diagnostic);
        }

        private async Task<Document> AddAggressiveInlining(
            Document document, CompilationUnitSyntax root,
            TypeDeclarationSyntax typeDeclarationSyntax, CancellationToken cancellationToken)
        {
            root = root.ReplaceNode(typeDeclarationSyntax,
                            new AddAggressiveInliningRewriter(await document.GetSemanticModelAsync(cancellationToken)).Visit(typeDeclarationSyntax));

            if (!root.Usings.ToNamespaceHashSet().Contains(System_Runtime_CompilerServices))
                root = SyntaxHelpers.AddSystem_Runtime_CompilerServicesSyntax(root);

            return document.WithSyntaxRoot(root);
        }

        private class AddAggressiveInliningRewriter : CSharpSyntaxRewriter
        {
            private readonly SemanticModel semanticModel;
            private readonly INamedTypeSymbol methodImpl;
            public AddAggressiveInliningRewriter(SemanticModel semanticModel) : base(false)
            {
                this.semanticModel = semanticModel;
                methodImpl = semanticModel.Compilation.GetTypeByMetadataName(
                    System_Runtime_CompilerServices_MethodImplAttribute);
            }
            public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                if (methodImpl is null)
                    return node;
                if (semanticModel.GetDeclaredSymbol(node) is not IMethodSymbol m)
                    return node;

                if (m.MethodKind == MethodKind.Ordinary
                    || m.MethodKind == MethodKind.ExplicitInterfaceImplementation)
                {
                    if (m.GetAttributes()
                        .Select(at => at.AttributeClass)
                        .Contains(methodImpl, SymbolEqualityComparer.Default))
                        return node;

                    return node.AddAttributeLists(SyntaxHelpers.AggressiveInliningAttributeList);
                }
                return node;
            }
        }
    }
}
