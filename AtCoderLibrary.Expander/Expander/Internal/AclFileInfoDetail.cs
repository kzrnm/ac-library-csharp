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
        public string CodeBody { get; }
        public SyntaxTree SyntaxTree { get; }
        public AclFileInfoDetail(string fileName, IEnumerable<string> typeNames, IEnumerable<string> usings, string code)
        {
            FileName = fileName;
            TypeNames = typeNames.ToArray();
            Usings = usings.ToArray();
            CodeBody = code;
            SyntaxTree = CSharpSyntaxTree.ParseText(string.Join("\n", Usings) + "\n" + CodeBody);
        }
        public AclFileInfoDetail(AclFileInfo orig) : this(orig.FileName, orig.TypeNames, orig.Usings, orig.CodeBody) { }
    }
}
