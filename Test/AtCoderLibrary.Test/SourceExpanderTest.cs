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
        public async Task Symbol()
        {
            bool expected =
#if NETCOREAPP3_0
                          false
#else
                          true
#endif
                ;
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Mod1000000007));
            embedded.AssemblyMetadatas.Should().ContainKey("SourceExpander.EmbeddedSourceCode.GZipBase32768");
            embedded.SourceFiles.Select(s => s.FileName)
                .Should()
                .HaveCountGreaterThan(2)
                .And
                .OnlyContain(name => name.StartsWith("AtCoderLibrary>"));
            embedded.SourceFiles.SelectMany(s => s.Usings)
                .Contains("using System.Runtime.Intrinsics.X86;")
                .Should()
                .Be(expected);
        }

        [Fact]
        public async Task LanguageVersion()
        {
            const string VERSION =
#if NETCOREAPP3_0
                          "7.3"
#else
                          "8.0"
#endif
                ;
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Mod1000000007));
            embedded.AssemblyMetadatas
                .Should().ContainKey("SourceExpander.EmbeddedLanguageVersion")
                .WhoseValue.Should().Be(VERSION);
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
                "AtCoder.Operators.IArithmeticOperator<T>",
                "AtCoder.Segtree<TValue, TOp>");
        }


        [Fact]
        public async Task RemoveContract()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Mod1000000007));
            var codes = embedded.SourceFiles.Select(s => s.CodeBody);
            codes.Should().NotContain(code => code.Contains("Contract.Assert"));
        }
    }
}
