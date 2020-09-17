using System.Collections.Generic;
using System.Linq;
using AtCoder.Internal;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtCoder.Expand
{
    internal class CompilationCodeExpander : RoslynCodeExpander
    {
        public CompilationCodeExpander(string code, IEnumerable<AclFileInfoDetail> aclFileInfos) : base(code, aclFileInfos) { }

        private static readonly MetadataReference[] s_metadataReferences = new[] {
                    MetadataReference.CreateFromFile(typeof(RoslynCodeExpander).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(INumOperator<>).Assembly.Location),
                };
        private static SemanticModel GetSemanticModel(SyntaxTree tree) =>
            CSharpCompilation.Create("SemanticModel",
                new[] { tree }, s_metadataReferences).GetSemanticModel(tree, true);

        protected override IEnumerable<AclFileInfoDetail> GetRequiredAcl()
        {
            var classNames = FoundTypeNames(OrigTree);
            return AclFiles.Values.Where(acl => acl.TypeNames.Intersect(classNames).Any()).ToArray();
        }
        private string[] FoundTypeNames(SyntaxTree tree)
        {
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var semanticModel = GetSemanticModel(tree);
            return root.DescendantNodes()
                .Select(s => GetTypeNameFromSymbol(semanticModel.GetSymbolInfo(s).Symbol))
                .OfType<string>()
                .Distinct()
                .ToArray();
        }
        private static string? GetTypeNameFromSymbol(ISymbol? symbol)
        {
            if (symbol == null) return null;
            if (symbol is INamedTypeSymbol named)
            {
                return named.ConstructedFrom.ToDisplayString();
            }
            return symbol.ContainingType?.ConstructedFrom?.ToDisplayString() ?? symbol.ToDisplayString();
        }
    }
}
