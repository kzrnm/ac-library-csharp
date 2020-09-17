using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtCoder.Internal;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtCoder.Expand
{
    internal abstract class RoslynCodeExpander : CodeExpanderCore
    {
        private List<string>? _expanded;
        private SyntaxTree? _origTree;
        protected SyntaxTree OrigTree => _origTree ??= CSharpSyntaxTree.ParseText(OrigCode);
        public RoslynCodeExpander(string code, IEnumerable<AclFileInfoDetail> aclFileInfos) : base(code, aclFileInfos) { }
        public override IEnumerable<string> ExpandedLines()
        {
            if (_expanded != null) return _expanded;

            var origRoot = (CompilationUnitSyntax)OrigTree.GetRoot();
            var usings = new HashSet<string>(origRoot.Usings.Select(u => u.ToString().Trim()));

            var remover = new UsingRemover();
            var newRoot = (CompilationUnitSyntax)remover.Visit(origRoot);

            var (requiedUsings, requiedBody) = GetRequiredCodes();
            usings.UnionWith(requiedUsings);
            var sortedUsings = SortedUsings(usings);

            _expanded = new List<string>();

            foreach (var line in sortedUsings)
                _expanded.Add(line);
            using (var sr = new StringReader(newRoot.ToString()))
            {
                var line = sr.ReadLine();
                while (line != null)
                {
                    _expanded.Add(line);
                    line = sr.ReadLine();
                }
            }
            _expanded.Add("#region AtCoderLibrary");

            foreach (var body in requiedBody)
            {
                using (var sr = new StringReader(body))
                {
                    var line = sr.ReadLine();
                    while (line != null)
                    {
                        _expanded.Add(line);
                        line = sr.ReadLine();
                    }
                }
            }
            _expanded.Add("#endregion AtCoderLibrary");

            return _expanded;
        }

        protected abstract IEnumerable<AclFileInfoDetail> GetRequiredAcl();
        private (IEnumerable<string> usings, IEnumerable<string> body) GetRequiredCodes()
        {
            var bodies = new List<string>();
            var fileNameQueue = new Queue<string>();
            var usedFileName = new HashSet<string>();

            var usings = new HashSet<string>();

            var acls = GetRequiredAcl();
            foreach (var acl in acls)
            {
                usedFileName.Add(acl.FileName);
                usings.UnionWith(acl.Usings);
                bodies.Add(acl.CodeBody);
            }
            foreach (var d in acls.SelectMany(acl => acl.Dependencies))
                if (usedFileName.Add(d))
                    fileNameQueue.Enqueue(d);

            while (fileNameQueue.Count > 0)
            {
                var dep = fileNameQueue.Dequeue();
                if (AclFiles.TryGetValue(dep, out var acl))
                {
                    usings.UnionWith(acl.Usings);
                    bodies.Add(acl.CodeBody);
                    foreach (var d in acl.Dependencies)
                        if (usedFileName.Add(d))
                            fileNameQueue.Enqueue(d);
                }
            }
            bodies.Sort();
            return (usings, bodies);
        }
    }
}
