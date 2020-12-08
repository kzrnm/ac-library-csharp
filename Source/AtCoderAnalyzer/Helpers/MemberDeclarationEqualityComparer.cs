using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtCoderAnalyzer.Helpers
{
    internal class MemberDeclarationEqualityComparer : IEqualityComparer<MemberDeclarationSyntax>
    {
        public static MemberDeclarationEqualityComparer Default { get; } = new MemberDeclarationEqualityComparer();
        public bool Equals(MemberDeclarationSyntax x, MemberDeclarationSyntax y) => x.IsEquivalentTo(y, true);
        public int GetHashCode(MemberDeclarationSyntax obj) => obj.RawKind;
    }
}
