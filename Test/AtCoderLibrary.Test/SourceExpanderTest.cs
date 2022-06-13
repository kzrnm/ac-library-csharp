using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SourceExpander;
using Xunit;

namespace AtCoder.Embedding
{
    public class SourceExpanderTest
    {
#if NETCOREAPP3_0
        const bool useIntrinsics = false;
        const string languageVersion = "7.3";
#elif NETCOREAPP3_1
        const bool useIntrinsics = true;
        const string languageVersion = "8.0";
#else
        const bool useIntrinsics = true;
        const string languageVersion = "preview";
#endif


        [Fact]
        public async Task Symbol()
        {
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
                .Be(useIntrinsics);
        }

        [Fact]
        public async Task LanguageVersion()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Mod1000000007));
            embedded.AssemblyMetadatas
                .Should().ContainKey("SourceExpander.EmbeddedLanguageVersion")
                .WhoseValue.Should().Be(languageVersion);
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
        public async Task EmbeddedNamespaces()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Mod1000000007));
            embedded.AssemblyMetadatas.Should().ContainKey("SourceExpander.EmbeddedNamespaces");
            embedded.EmbeddedNamespaces.Should()
                .Equal(
                "AtCoder",
                "AtCoder.Extension",
                "AtCoder.Internal",
                "AtCoder.Operators");
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
