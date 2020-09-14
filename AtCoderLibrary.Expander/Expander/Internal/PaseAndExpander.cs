using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AtCoder.Embedded;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtCoder.Internal
{
    internal class PaseAndExpander
    {
        private string OrigCode { get; }
        private SyntaxTree OrigTree { get; }
        private List<AclFileInfoDetail> AclFiles { set; get; }
        private HashSet<string> Usings { get; } = new HashSet<string>();
        public PaseAndExpander(string origCode, IEnumerable<AclFileInfoDetail> aclFileInfos)
        {
            AclFiles = new List<AclFileInfoDetail>(aclFileInfos);
            OrigCode = origCode;
            OrigTree = CSharpSyntaxTree.ParseText(origCode);
        }

        private static MetadataReference[] metadataReferences = new[] {
                    MetadataReference.CreateFromFile(typeof(PaseAndExpander).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(INumOperator<>).Assembly.Location),
                };
        private static SemanticModel GetSemanticModel(SyntaxTree tree) =>
            CSharpCompilation.Create("SemanticModel",
                new[] { tree }, metadataReferences).GetSemanticModel(tree, true);

        public string Expand()
        {
            var origRoot = (CompilationUnitSyntax)OrigTree.GetRoot();
            Usings.UnionWith(origRoot.Usings.Select(u => u.ToString().Trim()));

            var remover = new UsingRemover();
            var newRoot = (CompilationUnitSyntax)remover.Visit(origRoot);

            var bodies = new List<string>();
            var queue = new Queue<AclFileInfoDetail>();


            var classNames = FoundTypeNames(OrigTree);
            for (int i = AclFiles.Count - 1; i >= 0; i--)
            {
                var target = AclFiles[i];
                if (target.TypeNames.Intersect(classNames).Any())
                {
                    bodies.Add(target.CodeBody);
                    Usings.UnionWith(target.Usings);
                    queue.Enqueue(target);
                    AclFiles.RemoveAt(i);
                }
            }

            while (queue.Count > 0)
            {
                var acl = queue.Dequeue();
                classNames = FoundTypeNames(acl.SyntaxTree);
                for (int i = AclFiles.Count - 1; i >= 0; i--)
                {
                    var target = AclFiles[i];
                    if (target.TypeNames.Intersect(classNames).Any())
                    {
                        bodies.Add(target.CodeBody);
                        Usings.UnionWith(target.Usings);
                        queue.Enqueue(target);
                        AclFiles.RemoveAt(i);
                    }
                }
            }
            var sortedUsings = SortedUsings(Usings);
            bodies.Sort();
            return @$"{string.Join("\n", sortedUsings)}
{newRoot}
#region AtCoderLibrary
{string.Join("\n", bodies)}
#endregion AtCoderLibrary
";
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
        private static string[] SortedUsings(IEnumerable<string> usings)
        {
            var arr = usings.ToArray();
            Array.Sort(arr, (a, b) => StringComparer.Ordinal.Compare(a.TrimEnd(';'), b.TrimEnd(';')));
            return arr;
        }
    }
}
