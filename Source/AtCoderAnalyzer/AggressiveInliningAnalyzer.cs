using System;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using AtCoderAnalyzer.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using static AtCoderAnalyzer.Helpers.Constants;
using MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions;

namespace AtCoderAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AggressiveInliningAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(DiagnosticDescriptors.AC0007_AgressiveInlining_Descriptor);

        private class ContainingOperatorTypes
        {
            public INamedTypeSymbol MethodImplAttribute { get; }
            public INamedTypeSymbol CompilerGeneratedAttribute { get; }
            public INamedTypeSymbol IsOperatorAttribute { get; }

            public ContainingOperatorTypes(INamedTypeSymbol methodImpl, INamedTypeSymbol compilerGenerated, INamedTypeSymbol isOperator)
            {
                MethodImplAttribute = methodImpl;
                CompilerGeneratedAttribute = compilerGenerated;
                IsOperatorAttribute = isOperator;
            }
            public static bool TryParseTypes(Compilation compilation, out ContainingOperatorTypes types)
            {
                types = null;
                var methodImpl = compilation.GetTypeByMetadataName(System_Runtime_CompilerServices_MethodImplAttribute);
                if (methodImpl is null)
                    return false;
                var compilerGenerated = compilation.GetTypeByMetadataName(System_Runtime_CompilerServices_CompilerGeneratedAttribute);
                if (compilerGenerated is null)
                    return false;
                var isOperator = compilation.GetTypeByMetadataName(AtCoder_IsOperatorAttribute);
                if (isOperator is null)
                    return false;
                types = new ContainingOperatorTypes(methodImpl, compilerGenerated, isOperator);
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
                        SyntaxKind.StructDeclaration
                        , SyntaxKind.ClassDeclaration
                        , /* RecordDeclaration */ (SyntaxKind)9063
                        , /* RecordStructDeclaration */ (SyntaxKind)9068);
                }
            });
        }

        private void AnalyzeTypeDecra(SyntaxNodeAnalysisContext context, ContainingOperatorTypes types)
        {
            if (context.SemanticModel.GetDeclaredSymbol(context.Node, context.CancellationToken)
                is not INamedTypeSymbol symbol)
                return;
            var concurrentBuild = context.Compilation.Options.ConcurrentBuild;

            bool HasIsOperatorAttribute(INamedTypeSymbol symbol)
            {
                foreach (var at in symbol.ConstructedFrom.GetAttributes())
                    if (SymbolEqualityComparer.Default.Equals(at.AttributeClass, types.IsOperatorAttribute))
                        return true;
                return false;
            }

            if (concurrentBuild)
            {
                if (symbol.AllInterfaces
                    .AsParallel(context.CancellationToken)
                    .Any(HasIsOperatorAttribute))
                    goto HasIsOperator;
            }
            else
            {
                if (symbol.AllInterfaces
                    .Do(_ => context.CancellationToken.ThrowIfCancellationRequested())
                    .Any(HasIsOperatorAttribute))
                    goto HasIsOperator;
            }
            return;
        HasIsOperator:

            bool DoesNotHaveMethodImplInlining(IMethodSymbol m)
            {
                if (m.MethodKind is
                     not (MethodKind.ExplicitInterfaceImplementation or MethodKind.Ordinary))
                    return false;

                var result = true;

                foreach (var attr in m.GetAttributes())
                {
                    if (SymbolEqualityComparer.Default.Equals(attr.AttributeClass, types.CompilerGeneratedAttribute))
                    {
                        return false;
                    }
                    else if (SymbolEqualityComparer.Default.Equals(attr.AttributeClass, types.MethodImplAttribute))
                    {
                        if (attr.ConstructorArguments is { Length: 0 })
                            result = true;
                        else
                        {
                            var arg = attr.ConstructorArguments[0];
                            if (arg.Kind is TypedConstantKind.Primitive or TypedConstantKind.Enum)
                                try
                                {
                                    result = !((MethodImplOptions)Convert.ToInt32(arg.Value)).HasFlag(MethodImplOptions.AggressiveInlining);
                                }
                                catch
                                {
                                    result = true;
                                }
                        }
                    }
                }
                return result;
            }

            string[] notMethodImplInliningMethods;
            if (concurrentBuild)
                notMethodImplInliningMethods = symbol.GetMembers()
                    .AsParallel(context.CancellationToken)
                    .OfType<IMethodSymbol>()
                    .Where(DoesNotHaveMethodImplInlining)
                    .Select(m => m.Name)
                    .ToArray();
            else
                notMethodImplInliningMethods = symbol.GetMembers()
                    .Do(_ => context.CancellationToken.ThrowIfCancellationRequested())
                    .OfType<IMethodSymbol>()
                    .Where(DoesNotHaveMethodImplInlining)
                    .Select(m => m.Name)
                    .ToArray();
            if (notMethodImplInliningMethods.Length == 0)
                return;

            var diagnostic = DiagnosticDescriptors.AC0007_AgressiveInlining(
                context.Node.GetLocation(), notMethodImplInliningMethods);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
