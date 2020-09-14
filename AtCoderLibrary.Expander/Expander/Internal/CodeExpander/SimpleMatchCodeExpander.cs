using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtCoder.Internal.CodeExpander
{
    internal class SimpleMatchCodeExpander : RoslynCodeExpander
    {
        public SimpleMatchCodeExpander(string code, IEnumerable<AclFileInfoDetail> aclFileInfos) : base(code, aclFileInfos) { }
        private static string ToSimpleClassName(string className)
        {
            var span = className.AsSpan();

            // AtCoder.INumOperator<T> → INumOperator<T>
            for (int l = span.Length - 1; l >= 0; l--)
                if (span[l] == '.')
                {
                    span = span.Slice(l + 1);
                    break;
                }


            // INumOperator<T> → INumOperator
            for (int r = 0; r < span.Length; r++)
                if (span[r] == '<')
                {
                    span = span.Slice(0, r);
                    break;
                }
            return span.ToString();
        }
        protected override IEnumerable<AclFileInfoDetail> GetRequiredAcl()
        {
            var root = OrigTree.GetRoot();
            var simpleNames = root.DescendantNodes()
                .OfType<SimpleNameSyntax>()
                .Select(s => s.Identifier.ToString())
                .Distinct()
                .ToArray();
            return AclFiles.Values.Where(acl => acl.TypeNames.Select(ToSimpleClassName).Intersect(simpleNames).Any()).ToArray();
        }
    }
}
