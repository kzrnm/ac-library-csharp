using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AtCoderAnalyzer.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static AtCoderAnalyzer.Helpers.Constants;

namespace AtCoderAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CreateOperatorAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(
                DiagnosticDescriptors.AC0008_DefineOperatorType);

        private class ContainingOperatorTypes
        {
            public INamedTypeSymbol IsOperatorAttribute { get; }

            public ContainingOperatorTypes(INamedTypeSymbol isOperator)
            {
                IsOperatorAttribute = isOperator;
            }
            public static bool TryParseTypes(Compilation compilation, out ContainingOperatorTypes types)
            {
                types = null;
                var isOperator = compilation.GetTypeByMetadataName(AtCoder_IsOperatorAttribute);
                if (isOperator is null)
                    return false;
                types = new ContainingOperatorTypes(isOperator);
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
                        c => AnalyzeGenericNode(c, types), SyntaxKind.GenericName);
                }
            });
        }
        private void AnalyzeGenericNode(SyntaxNodeAnalysisContext context, ContainingOperatorTypes types)
        {
            var semanticModel = context.SemanticModel;
            if (context.Node is not GenericNameSyntax genericNode)
                return;

            if (semanticModel.GetSymbolInfo(genericNode, context.CancellationToken).Symbol
                is not INamedTypeSymbol symbol)
                return;

            var originalTypes = symbol.TypeParameters;
            var writtenTypes = symbol.TypeArguments;

            if (originalTypes.Length != writtenTypes.Length)
                return;

            var notDefinedTypes = new List<string>();
            for (int i = 0; i < originalTypes.Length; i++)
            {
                var originalType = originalTypes[i];
                var writtenType = writtenTypes[i];

                if (!originalType.ConstraintTypes.SelectMany(ty => ty.GetAttributes())
                    .Select(at => at.AttributeClass)
                    .Contains(types.IsOperatorAttribute, SymbolEqualityComparer.Default))
                    continue;

                var k = originalType.TypeKind;
                if (writtenType.TypeKind == TypeKind.Error)
                {
                    notDefinedTypes.Add(writtenType.Name.ToString());
                }
            }
            if (notDefinedTypes.Count == 0)
                return;

            var diagnostic = Diagnostic.Create(DiagnosticDescriptors.AC0008_DefineOperatorType,
                genericNode.GetLocation(), string.Join(", ", notDefinedTypes));
            context.ReportDiagnostic(diagnostic);
        }
    }
}
