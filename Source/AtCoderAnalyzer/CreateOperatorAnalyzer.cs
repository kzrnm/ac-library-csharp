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
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterCompilationStartAction(compilationStartContext =>
            {
                if (ContainingOperatorTypes.TryParseTypes(compilationStartContext.Compilation, out var types))
                {
                    compilationStartContext.RegisterSyntaxNodeAction(
                        c => AnalyzeGenericNode(c, types), SyntaxKind.GenericName);
                }
            });
        }
        private class ContainingOperatorTypes
        {
            public ImmutableDictionary<INamedTypeSymbol, DiagnosticDescriptor> Types { get; }

            public ContainingOperatorTypes(
             INamedTypeSymbol staticModInt,
             INamedTypeSymbol dynamicModInt,
             INamedTypeSymbol segtree,
             INamedTypeSymbol lazySegtree)
            {
                var build = ImmutableDictionary.CreateBuilder<INamedTypeSymbol, DiagnosticDescriptor>(SymbolEqualityComparer.Default);
                build.Add(staticModInt, DiagnosticDescriptors.AC0003_StaticModInt);
                build.Add(dynamicModInt, DiagnosticDescriptors.AC0004_DynamicModInt);
                build.Add(segtree, DiagnosticDescriptors.AC0005_SegtreeOperator);
                build.Add(lazySegtree, DiagnosticDescriptors.AC0006_LazySegtreeOperator);
                Types = build.ToImmutable();
            }
            public static bool TryParseTypes(Compilation compilation, out ContainingOperatorTypes types)
            {
                types = null;
                var staticModInt = compilation.GetTypeByMetadataName("AtCoder.StaticModInt`1");
                if (staticModInt is null)
                    return false;
                var dynamicModInt = compilation.GetTypeByMetadataName("AtCoder.DynamicModInt`1");
                if (dynamicModInt is null)
                    return false;
                var segtree = compilation.GetTypeByMetadataName("AtCoder.Segtree`2");
                if (segtree is null)
                    return false;
                var lazySegtree = compilation.GetTypeByMetadataName("AtCoder.LazySegtree`3");
                if (lazySegtree is null)
                    return false;

                types = new ContainingOperatorTypes(
                    staticModInt,
                    dynamicModInt,
                    segtree,
                    lazySegtree);
                return true;
            }
        }
        private void AnalyzeGenericNode(SyntaxNodeAnalysisContext context, ContainingOperatorTypes types)
        {
            var semanticModel = context.SemanticModel;
            if (context.Node is not GenericNameSyntax genericNode)
                return;

            if (semanticModel.GetTypeInfo(genericNode, context.CancellationToken).Type.OriginalDefinition
                is not INamedTypeSymbol type
                || !types.Types.TryGetValue(type, out var descriptor))
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
