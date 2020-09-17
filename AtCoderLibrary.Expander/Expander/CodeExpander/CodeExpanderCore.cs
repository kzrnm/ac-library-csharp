using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using AtCoder.Internal;

namespace AtCoder.Expand
{
    public static class CodeExpander
    {
        public static string Expand(string code, ExpandMethod expandMethod)
        {
            var expander = Create(code, expandMethod);
            var sb = new StringBuilder();
            foreach (var line in expander.ExpandedLines())
                sb.AppendLine(line);
            return sb.ToString();
        }
        public static ICodeExpander Create(string code, ExpandMethod expandMethod)
            => Create(code, expandMethod, Expander.s_aclFileInfoDetails);
        internal static ICodeExpander Create(string code, ExpandMethod expandMethod, IEnumerable<AclFileInfoDetail> aclFileInfos)
            => expandMethod switch
            {
                ExpandMethod.All => new AllCodeExpander(code, aclFileInfos),
                ExpandMethod.NameSyntax => new SimpleMatchCodeExpander(code, aclFileInfos),
                ExpandMethod.Strict => new CompilationCodeExpander(code, aclFileInfos),
                _ => throw new InvalidEnumArgumentException(nameof(expandMethod), (int)expandMethod, expandMethod.GetType()),
            };
    }

    internal abstract class CodeExpanderCore : ICodeExpander
    {
        protected Dictionary<string, AclFileInfoDetail> AclFiles { set; get; }
        protected string OrigCode { get; }
        public CodeExpanderCore(string code, IEnumerable<AclFileInfoDetail> aclFileInfos)
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
    public interface ICodeExpander
    {
        IEnumerable<string> ExpandedLines();
    }
}
