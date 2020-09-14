using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AtCoder.Internal.CodeExpander
{
    internal class AllCodeExpander : CodeExpander
    {
        public AllCodeExpander(string code, IEnumerable<AclFileInfoDetail> aclFileInfos) : base(code, aclFileInfos) { }
        public override IEnumerable<string> ExpandedLines()
        {
            var usings = AclFiles.Values.SelectMany(acl => acl.Usings).ToHashSet();
            using var sr = new StringReader(OrigCode);

            var line = sr.ReadLine();
            while (line != null)
            {
                if (string.IsNullOrWhiteSpace(line)) { }
                else if (line.StartsWith("using"))
                {
                    usings.Add(line);
                }
                else break;
                line = sr.ReadLine();
            }

            foreach (var u in SortedUsings(usings))
                yield return u;

            while (line != null)
            {
                yield return line;
                line = sr.ReadLine();
            }

            yield return "#region AtCoderLibrary";
            foreach (var body in AclFiles.Values.Select(acl => acl.CodeBody))
                yield return body;
            yield return "#endregion AtCoderLibrary";
        }
    }
}
