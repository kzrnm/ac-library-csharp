using System.Collections.Generic;
using System.Collections.Immutable;
using AtCoderAnalyzer.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AtCoderAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CreateOperatorAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(
                DiagnosticDescriptors.AC0003_StaticModInt,
                DiagnosticDescriptors.AC0004_DynamicModInt,
                DiagnosticDescriptors.AC0005_SegtreeOperator,
                DiagnosticDescriptors.AC0006_LazySegtreeOperator);
        private static readonly ImmutableDictionary<string, DiagnosticDescriptor> TypeDescriptorDic
            = new Dictionary<string, DiagnosticDescriptor>
            {
                { "StaticModInt`1", DiagnosticDescriptors.AC0003_StaticModInt },
                { "DynamicModInt`1", DiagnosticDescriptors.AC0004_DynamicModInt },
                { "Segtree`2", DiagnosticDescriptors.AC0005_SegtreeOperator },
                { "LazySegtree`3", DiagnosticDescriptors.AC0006_LazySegtreeOperator },
            }.ToImmutableDictionary();
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterCompilationStartAction(compilationStartContext =>
            {
                ITypeSymbol staticModIntTypeSymbol = compilationStartContext.Compilation.GetTypeByMetadataName("AtCoder.StaticModInt`1");
                if (staticModIntTypeSymbol is not null)
                {
                    compilationStartContext.RegisterSyntaxNodeAction(AnalyzeGenericNode, SyntaxKind.GenericName);
                }
            });
        }
        private void AnalyzeGenericNode(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;
            if (context.Node is not GenericNameSyntax genericNode)
                return;

            var type = semanticModel.GetTypeInfo(genericNode, context.CancellationToken).Type;
            if (type.ContainingNamespace.ToDisplayString() != "AtCoder")
                return;
            if (!TypeDescriptorDic.TryGetValue(type.MetadataName, out DiagnosticDescriptor descriptor))
                return;

            var operatorTypeSyntax = genericNode.TypeArgumentList.Arguments[genericNode.TypeArgumentList.Arguments.Count - 1];
            var operatorType = semanticModel.GetTypeInfo(operatorTypeSyntax, context.CancellationToken).Type;

            if (operatorType.TypeKind != TypeKind.Error)
                return;

            var diagnostic = Diagnostic.Create(descriptor,
                context.Node.GetLocation(),
                operatorTypeSyntax.ToString());
            context.ReportDiagnostic(diagnostic);
        }
    }
}
