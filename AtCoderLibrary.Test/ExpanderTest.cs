using System.Linq;
using FluentAssertions;
using Xunit;
namespace AtCoder
{
    public class ExpanderTest
    {
        [Fact]
        public void FenwickTree()
        {
            var fileinfos = SourceExpander.EmbeddedGenerator.ModuleInitializer.sourceFileInfos;
            var fwSource = fileinfos.Should()
                .ContainSingle(f => f.FileName.Replace('\\', '/') == @"AtCoderLibrary>DataStructure/FenwickTree.cs")
                .Which;
            fwSource.Dependencies.Select(s => s.Replace('\\', '/')).Should().BeEquivalentTo(new string[] {
                @"AtCoderLibrary>Algebra/NumOperator.cs",
                @"AtCoderLibrary>Util/DebugUtil.cs",
                @"AtCoderLibrary>Internal/InternalBit.cs",
            });
            fwSource.TypeNames.Should().Contain("AtCoder.FenwickTree<TValue, TOp>");
        }
    }
}
