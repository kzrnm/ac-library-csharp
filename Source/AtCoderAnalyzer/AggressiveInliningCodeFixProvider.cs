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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static AtCoderAnalyzer.Helpers.Constants;

namespace AtCoderAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AggressiveInliningCodeFixProvider)), Shared]
    public class AggressiveInliningCodeFixProvider : CodeFixProvider
    {
        private const string title = "Add AggressiveInlining attribute";
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
                is not MethodDeclarationSyntax methodDeclaration)
                return;

            var action = CodeAction.Create(title: title,
               createChangedDocument: c => AddAggressiveInlining(context.Document, root, methodDeclaration, c),
               equivalenceKey: title);
            context.RegisterCodeFix(action, diagnostic);
        }

#pragma warning disable IDE0060
        private async Task<Document> AddAggressiveInlining(
            Document document, CompilationUnitSyntax root,
            MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
        {
            var hasSystem_Runtime_CompilerServices =
                root.Usings.Any(sy => sy.Name.ToString() == System_Runtime_CompilerServices);
            if (!hasSystem_Runtime_CompilerServices)
                root = SyntaxHelpers.AddSystem_Runtime_CompilerServicesSyntax(root);

            return document.WithSyntaxRoot(root.ReplaceNode(methodDeclaration,
                methodDeclaration.AddAttributeLists(SyntaxHelpers.AggressiveInliningAttributeList)));
        }
#pragma warning restore IDE0060
    }
}
