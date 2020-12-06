using System.Collections.Immutable;
using System.Linq;
using AtCoderAnalyzer.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using static AtCoderAnalyzer.Helpers.Constants;

namespace AtCoderAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AggressiveInliningAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(DiagnosticDescriptors.AC0007_AgressiveInlining);

        private class ContainingOperatorTypes
        {
            public INamedTypeSymbol MethodImplAttribute { get; }
            public INamedTypeSymbol IsOperatorAttribute { get; }

            public ContainingOperatorTypes(INamedTypeSymbol methodImpl, INamedTypeSymbol isOperator)
            {
                MethodImplAttribute = methodImpl;
                IsOperatorAttribute = isOperator;
            }
            public static bool TryParseTypes(Compilation compilation, out ContainingOperatorTypes types)
            {
                types = null;
                var methodImpl = compilation.GetTypeByMetadataName(System_Runtime_CompilerServices_MethodImplAttribute);
                if (methodImpl is null)
                    return false;
                var isOperator = compilation.GetTypeByMetadataName(AtCoder_IsOperatorAttribute);
                if (isOperator is null)
                    return false;
                types = new ContainingOperatorTypes(methodImpl, isOperator);
                return true;
            }
        }
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterCompilationStartAction(compilationStartContext =>
            {
                if (ContainingOperatorTypes.TryParseTypes(compilationStartContext.Compilation, out var types))
                {
                    compilationStartContext.RegisterSyntaxNodeAction(
                        c => AnalyzeTypeDecra(c, types),
                        SyntaxKind.StructDeclaration, SyntaxKind.ClassConstraint);
                }
            });
        }

        private void AnalyzeTypeDecra(SyntaxNodeAnalysisContext context, ContainingOperatorTypes types)
        {
            if (context.SemanticModel.GetDeclaredSymbol(context.Node, context.CancellationToken)
                is not INamedTypeSymbol symbol)
                return;

            if (!symbol.AllInterfaces
                .SelectMany(n => n.ConstructedFrom.GetAttributes())
                .Select(at => at.AttributeClass)
                .Contains(types.IsOperatorAttribute, SymbolEqualityComparer.Default))
                return;

            var notMethodImplMethods = symbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.MethodKind == MethodKind.Ordinary
                         || m.MethodKind == MethodKind.ExplicitInterfaceImplementation)
                .Where(m => !m.GetAttributes().Select(at => at.AttributeClass).Contains(
                        types.MethodImplAttribute, SymbolEqualityComparer.Default))
                .Select(m => m.Name)
                .ToArray();
            if (notMethodImplMethods.Length == 0)
                return;

            var diagnostic = Diagnostic.Create(
                DiagnosticDescriptors.AC0007_AgressiveInlining,
                context.Node.GetLocation(), string.Join(", ", notMethodImplMethods));
            context.ReportDiagnostic(diagnostic);
        }
    }
}
