using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AtCoderAnalyzer.Diagnostics;
using AtCoderAnalyzer.Helpers;
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
                DiagnosticDescriptors.AC0008_DefineOperatorType_Descriptor);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterCompilationStartAction(compilationStartContext =>
            {
                if (OperatorTypesMatcher.TryParseTypes(compilationStartContext.Compilation, out var types))
                {
                    compilationStartContext.RegisterSyntaxNodeAction(
                        c => AnalyzeGenericNode(c, types), SyntaxKind.GenericName);
                }
            });
        }
        private void AnalyzeGenericNode(SyntaxNodeAnalysisContext context, OperatorTypesMatcher types)
        {
            var semanticModel = context.SemanticModel;
            if (context.Node is not GenericNameSyntax genericNode)
                return;

            if (genericNode.TypeArgumentList.Arguments.Any(sy => sy.IsKind(SyntaxKind.OmittedTypeArgument)))
                return;

            ImmutableArray<ITypeParameterSymbol> originalTypes;
            ImmutableArray<ITypeSymbol> writtenTypes;
            switch (semanticModel.GetSymbolInfo(genericNode, context.CancellationToken).Symbol)
            {
                case INamedTypeSymbol symbol:
                    originalTypes = symbol.TypeParameters;
                    writtenTypes = symbol.TypeArguments;
                    break;
                case IMethodSymbol symbol:
                    originalTypes = symbol.TypeParameters;
                    writtenTypes = symbol.TypeArguments;
                    break;
                default:
                    return;
            }

            if (originalTypes.Length == 0 || originalTypes.Length != writtenTypes.Length)
                return;

            var notDefinedTypes = new List<string>();
            for (int i = 0; i < originalTypes.Length; i++)
            {
                var originalType = originalTypes[i];
                var writtenType = writtenTypes[i];



                bool ok = false;
                foreach (var ty in originalType.ConstraintTypes)
                {
                    if (ty is INamedTypeSymbol named)
                        if (ok = types.IsMatch(named.ConstructedFrom))
                            break;
                }
                if (!ok)
                    continue;

                var k = originalType.TypeKind;
                if (writtenType.TypeKind == TypeKind.Error)
                {
                    notDefinedTypes.Add(writtenType.Name.ToString());
                }
            }
            if (notDefinedTypes.Count == 0)
                return;

            var diagnostic = DiagnosticDescriptors.AC0008_DefineOperatorType(
                genericNode.GetLocation(), notDefinedTypes);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
