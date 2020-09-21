using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceCodeEmbedder
{
    public class AclFileInfoWithDependency
    {
        private AclFileInfoRaw[] Infos { get; }
        public AclFileInfoWithDependency(IEnumerable<AclFileInfoRaw> infos)
        {
            Infos = infos.ToArray();
            Array.Sort(Infos, (info1, info2) => StringComparer.OrdinalIgnoreCase.Compare(info1.FilePath, info2.FilePath));
        }

        private static readonly MetadataReference[] metadataReferences = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };
        private IEnumerable<string> GetDependencies(AclFileInfoRaw raw)
        {
            var tree = raw.SyntaxTree;
            var root = (CompilationUnitSyntax)tree.GetRoot();

            var semanticModel = CSharpCompilation.Create("SemanticModel"
                , Infos.Select(info => info.SyntaxTree), metadataReferences)
                .GetSemanticModel(tree, true);
            var typeQueue = new Queue<string>(root.DescendantNodes()
                .Select(s => GetTypeNameFromSymbol(semanticModel.GetSymbolInfo(s).Symbol?.OriginalDefinition))
                .OfType<string>()
                .Distinct());

            var added = new HashSet<string>(raw.TypeNames);
            var dependencies = new HashSet<string>();
            while (typeQueue.Count > 0)
            {
                var typeName = typeQueue.Dequeue();
                if (!added.Add(typeName)) continue;

                dependencies.UnionWith(Infos.Where(acl => acl.TypeNames.Contains(typeName)).Select(acl => acl.FilePath));
            }

            return dependencies;
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

        public IEnumerable<string> ToEmbededStringLines() => Infos.Select(ToEmbededString);
        private string ToEmbededString(AclFileInfoRaw raw)
        {
            var args = string.Join(", ",
                Quote(raw.FilePath),
                QuoteArray(raw.TypeNames),
                QuoteArray(raw.Usings),
                QuoteArray(GetDependencies(raw)),
                Quote(raw.CodeBody));
            return @$"new AclFileInfo({args}),";
        }
        private static string Quote(string s)
            => $"@\"{s.Replace("\"", "\"\"")}\"";
        private static string QuoteArray(IEnumerable<string> ss)
            => "new string[] { " + string.Join(", ", ss.Select(Quote)) + " }";

    }
}
