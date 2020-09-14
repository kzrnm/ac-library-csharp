using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AtCoder.Internal.CodeExpander
{
    internal abstract class CodeExpander : ICodeExpander
    {
        public static CodeExpander Create(ExpandMethod expandMethod, string code, IEnumerable<AclFileInfoDetail> aclFileInfos)
            => expandMethod switch
            {
                ExpandMethod.All => new AllCodeExpander(code, aclFileInfos),
                ExpandMethod.NameSyntax => new SimpleMatchCodeExpander(code, aclFileInfos),
                ExpandMethod.Strict => new CompilationCodeExpander(code, aclFileInfos),
                _ => throw new InvalidEnumArgumentException(nameof(expandMethod), (int)expandMethod, expandMethod.GetType()),
            };


        protected Dictionary<string, AclFileInfoDetail> AclFiles { set; get; }
        protected string OrigCode { get; }
        public CodeExpander(string code, IEnumerable<AclFileInfoDetail> aclFileInfos)
        {
            AclFiles = aclFileInfos.ToDictionary(acl => acl.FileName);
            OrigCode = code;
        }
        public abstract IEnumerable<string> ExpandedLines();

        internal static string[] SortedUsings(IEnumerable<string> usings)
        {
            var arr = usings.ToArray();
            Array.Sort(arr, (a, b) => StringComparer.Ordinal.Compare(a.TrimEnd(';'), b.TrimEnd(';')));
            return arr;
        }
    }
    internal interface ICodeExpander
    {
        IEnumerable<string> ExpandedLines();
    }
}
