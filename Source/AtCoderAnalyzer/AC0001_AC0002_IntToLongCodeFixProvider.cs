using System.Collections.Immutable;
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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AC0001_AC0002_IntToLongCodeFixProvider)), Shared]
    public class AC0001_AC0002_IntToLongCodeFixProvider : CodeFixProvider
    {
        private const string title = "Cast int to long";
        public override ImmutableArray<string> FixableDiagnosticIds
            => ImmutableArray.Create(
                DiagnosticDescriptors.AC0001_MultiplyOverflowInt32_Descriptor.Id,
                DiagnosticDescriptors.AC0002_LeftShiftOverflowInt32_Descriptor.Id);

        public sealed override FixAllProvider GetFixAllProvider()
            => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            if (await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false)
                is not CompilationUnitSyntax root)
                return;
            var diagnostic = context.Diagnostics[0];
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var node = root.FindNode(diagnosticSpan);
            foreach (var nn in node.ChildNodes().Prepend(node))
            {
                if (nn is BinaryExpressionSyntax b)
                {
                    switch (b.Kind())
                    {
                        case SyntaxKind.LeftShiftExpression:
                        case SyntaxKind.MultiplyExpression:
                            var action = CodeAction.Create(title: title,
                               createChangedDocument: c => CastLong(context.Document, b, c),
                               equivalenceKey: title);
                            context.RegisterCodeFix(action, diagnostic);
                            return;
                    }
                }
            }
        }
        private async Task<Document> CastLong(Document document, BinaryExpressionSyntax syntax, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            while (syntax.Left is BinaryExpressionSyntax nx)
                syntax = nx;

            if (syntax.Left is LiteralExpressionSyntax lx && lx.Token.Value is int num)
            {
                var longEx = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal((long)num));
                return document.WithSyntaxRoot(root.ReplaceNode(syntax.Left, longEx));
            }
            var castEx = SyntaxFactory.CastExpression(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.LongKeyword)), syntax.Left);
            return document.WithSyntaxRoot(root.ReplaceNode(syntax.Left, castEx));
        }
    }
}
