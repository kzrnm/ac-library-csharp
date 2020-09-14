using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceCodeEmbeder
{
    [DebuggerDisplay("AclFileInfoRaw: {" + nameof(FilePath) + "}")]
    public class AclFileInfoRaw
    {
        public string FilePath { get; }
        public ReadOnlyCollection<string> TypeNames { get; }
        public string OrigCode { get; }
        public SyntaxTree SyntaxTree { get; }
        public ReadOnlyCollection<string> Usings { get; }
        public string CodeBody { get; }

        private AclFileInfoRaw(string filePath, string[] typeNames, string origCode, string[] usings, string code)
        {
            FilePath = filePath;
            TypeNames = new ReadOnlyCollection<string>(typeNames);
            OrigCode = origCode;
            SyntaxTree = CSharpSyntaxTree.ParseText(OrigCode);
            Usings = new ReadOnlyCollection<string>(usings);
            CodeBody = code;
        }

        public static AclFileInfoRaw ParseFile(string path) => ParseFile(path, string.Empty);
        public static AclFileInfoRaw ParseFile(string path, string pathHead)
        {
            var code = File.ReadAllText(path);
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var usings = root.Usings.Select(u => u.ToString().Trim()).ToArray();

            var remover = new MinifyRewriter();
            var newRoot = (CompilationUnitSyntax)remover.Visit(root)!;
            var relativePath = path.Split(pathHead, StringSplitOptions.RemoveEmptyEntries).First();
            return new AclFileInfoRaw(relativePath, GetDeclaratedTypeName(CSharpSyntaxTree.Create(newRoot)), code, usings, MinifySpace(newRoot.ToString()));
        }


        private static string[] GetDeclaratedTypeName(SyntaxTree tree)
        {
            var semanticModel = CSharpCompilation.Create("SemanticModel", new[] { tree }).GetSemanticModel(tree);
            var root = tree.GetRoot();
            return root.DescendantNodes()
                .OfType<TypeDeclarationSyntax>()
                .Select(syntax => semanticModel.GetDeclaredSymbol(syntax)?.ToDisplayString())
                .OfType<string>()
                .Distinct()
                .ToArray();
        }

        private static string MinifySpace(string str) => Regex.Replace(str, " +", " ");
    }

}
