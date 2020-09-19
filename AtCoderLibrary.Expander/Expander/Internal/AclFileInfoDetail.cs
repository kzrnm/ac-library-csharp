using System.Collections.Generic;
using System.Linq;
using AtCoder.Embedded;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AtCoder.Internal
{
    internal class AclFileInfoDetail
    {
        public string FileName { get; }
        public IList<string> TypeNames { get; }
        public IList<string> Usings { get; }
        public IList<string> Dependencies { get; }
        public string CodeBody { get; }
        private SyntaxTree? _syntaxTree;
        public SyntaxTree SyntaxTree => _syntaxTree ??= CSharpSyntaxTree.ParseText(string.Join("\n", Usings) + "\n" + CodeBody);
        public AclFileInfoDetail(string fileName, IEnumerable<string> typeNames, IEnumerable<string> usings, IEnumerable<string> dependencies, string code)
        {
            FileName = fileName;
            TypeNames = typeNames.ToArray();
            Usings = usings.ToArray();
            Dependencies = dependencies.ToArray();
            CodeBody = code;
        }
        public AclFileInfoDetail(AclFileInfo orig) : this(orig.FileName, orig.TypeNames, orig.Usings, orig.Dependencies, orig.CodeBody) { }
    }
}
