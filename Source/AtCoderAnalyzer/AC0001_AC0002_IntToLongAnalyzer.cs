using System;
using System.Collections.Immutable;
using AtCoderAnalyzer.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AtCoderAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AC0001_AC0002_IntToLongAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(
                DiagnosticDescriptors.AC0001_MultiplyOverflowInt32_Descriptor,
                DiagnosticDescriptors.AC0002_LeftShiftOverflowInt32_Descriptor);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(
                GeneratedCodeAnalysisFlags.Analyze |
                GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeIntToLongSyntaxNode,
                SyntaxKind.LeftShiftExpression, SyntaxKind.MultiplyExpression);
        }

        private void AnalyzeIntToLongSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;
            var node = context.Node;

            var typeInfo = semanticModel.GetTypeInfo(node, cancellationToken: context.CancellationToken);
            if (typeInfo.Type.SpecialType != SpecialType.System_Int32)
                return;

            Diagnostic diagnostic = node.Kind() switch
            {
                SyntaxKind.MultiplyExpression => DiagnosticDescriptors.AC0001_MultiplyOverflowInt32(context.Node),
                SyntaxKind.LeftShiftExpression => DiagnosticDescriptors.AC0002_LeftShiftOverflowInt32(context.Node),
                _ => throw new InvalidOperationException(),
            };

            for (; node is not null; node = GetParent(node))
            {
                if (semanticModel.GetTypeInfo(node, cancellationToken: context.CancellationToken)
                    .ConvertedType.SpecialType == SpecialType.System_Int64)
                {
                    context.ReportDiagnostic(diagnostic);
                    return;
                }
            }

            static SyntaxNode GetParent(SyntaxNode node)
            {
                var parent = node.Parent;
                if (parent is BinaryExpressionSyntax or ParenthesizedExpressionSyntax)
                    return parent;
                return null;
            }
        }
    }
}
