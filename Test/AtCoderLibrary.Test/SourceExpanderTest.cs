using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SourceExpander;
using Xunit;

namespace AtCoder.Embedding
{
    public class SourceExpanderTest
    {
        [Fact]
        public async Task LanguageVersion()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Mod1000000007));
            embedded.AssemblyMetadatas
                .Should().ContainKey("SourceExpander.EmbeddedLanguageVersion")
                .WhichValue.Should().Be("8.0");
        }

        [Fact]
        public async Task EmbeddedSource()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Mod1000000007));
            embedded.AssemblyMetadatas.Should().ContainKey("SourceExpander.EmbeddedSourceCode.GZipBase32768");
            embedded.SourceFiles.Select(s => s.FileName)
                .Should()
                .HaveCountGreaterThan(2)
                .And
                .OnlyContain(name => name.StartsWith("AtCoderLibrary>"));
            embedded.SourceFiles.SelectMany(s => s.TypeNames)
                .Should()
                .Contain(
                    "AtCoder.IArithmeticOperator<T>",
                    "AtCoder.FenwickTree<TValue, TOp>");
        }
    }
}
