using System.Collections.Immutable;
using System.Linq;
using AtCoderAnalyzer.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AtCoderAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AggressiveInliningAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(DiagnosticDescriptors.AC0007_AgressiveInlining);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterCompilationStartAction(compilationStartContext =>
            {
                if (ContainingOperatorTypes.TryParseTypes(compilationStartContext.Compilation, out var types))
                {
                    compilationStartContext.RegisterSyntaxNodeAction(
                        c => AnalyzeMethodSymbol(c, types), SyntaxKind.MethodDeclaration);
                }
            });
        }
        private class ContainingOperatorTypes
        {
            public ImmutableHashSet<INamedTypeSymbol> Types { get; }
            public INamedTypeSymbol MethodImplAttribute { get; }

            public ContainingOperatorTypes(INamedTypeSymbol methodImpl, params INamedTypeSymbol[] operators)
            {
                MethodImplAttribute = methodImpl;
                Types = ImmutableHashSet.Create<INamedTypeSymbol>(SymbolEqualityComparer.Default,
                    operators);
            }
            public static bool TryParseTypes(Compilation compilation, out ContainingOperatorTypes types)
            {
                types = null;
                var methodImpl = compilation.GetTypeByMetadataName("System.Runtime.CompilerServices.MethodImplAttribute");
                if (methodImpl is null)
                    return false;
                var arithmeticOperator = compilation.GetTypeByMetadataName("AtCoder.IArithmeticOperator`1");
                if (arithmeticOperator is null)
                    return false;
                var compareOperator = compilation.GetTypeByMetadataName("AtCoder.ICompareOperator`1");
                if (compareOperator is null)
                    return false;
                var segtreeOperator = compilation.GetTypeByMetadataName("AtCoder.ISegtreeOperator`1");
                if (segtreeOperator is null)
                    return false;
                var lazySegtreeOperator = compilation.GetTypeByMetadataName("AtCoder.ILazySegtreeOperator`2");
                if (lazySegtreeOperator is null)
                    return false;
                types = new ContainingOperatorTypes(
                    methodImpl,
                    arithmeticOperator,
                    compareOperator,
                    segtreeOperator,
                    lazySegtreeOperator);
                return true;
            }
        }

        private void AnalyzeMethodSymbol(SyntaxNodeAnalysisContext context, ContainingOperatorTypes types)
        {
            if (context.Node is not MethodDeclarationSyntax node)
                return;
            if (context.SemanticModel.GetDeclaredSymbol(node, context.CancellationToken) is not IMethodSymbol symbol)
                return;
            if (symbol.MethodKind != MethodKind.Ordinary
                && symbol.MethodKind != MethodKind.ExplicitInterfaceImplementation)
                return;
            if (!types.Types.Overlaps(symbol.ContainingType.AllInterfaces.Select(n => n.ConstructedFrom)))
                return;
            if (symbol.GetAttributes().Any(at => SymbolEqualityComparer.Default.Equals(at.AttributeClass, types.MethodImplAttribute)))
                return;
            if (node.DescendantNodes().Any(
                n => n.IsKind(SyntaxKind.ThrowExpression) || n.IsKind(SyntaxKind.ThrowStatement)))
                return;

            var diagnostic = Diagnostic.Create(
                DiagnosticDescriptors.AC0007_AgressiveInlining, 
                symbol.Locations[0], symbol.Name);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
