using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SourceExpander;
using Xunit;

namespace AtCoder.Embedding
{
    public class SourceExpanderTest
    {
        class EmbeddingFact : FactAttribute
        {
#if !EMBEDDING
            public override string Skip => "SourceExpander.Embedder is disabled.";
#endif
        }
        class EmbeddingGenericMathFact : EmbeddingFact
        {
            public override string Skip => genericMath ? null : "GenericMath is disabled.";
        }

#if NETCOREAPP3_0
        const bool useIntrinsics = false;
        const bool genericMath = false;
        const string languageVersion = "7.3";
#elif NETCOREAPP3_1
        const bool useIntrinsics = true;
        const bool genericMath = false;
        const string languageVersion = "8.0";
#else
        const bool useIntrinsics = true;
        const bool genericMath = true;
        const string languageVersion = "11.0";
#endif

        [Fact]
        public async Task Embedding()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
            embedded.AssemblyMetadatas.Keys.Should()
#if EMBEDDING
                .ContainMatch(@"SourceExpander.*");
#else
                .NotContainMatch(@"SourceExpander.*");
#endif
        }

        [EmbeddingFact]
        public void AssemblyName()
        {
            typeof(Segtree<,>).Assembly.GetName().Name.Should().Be("ac-library-csharp");
        }

        [EmbeddingFact]
        public async Task Symbol()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
            embedded.AssemblyMetadatas.Should().ContainKey("SourceExpander.EmbeddedSourceCode.GZipBase32768");
            embedded.SourceFiles.Select(s => s.FileName)
                .Should()
                .HaveCountGreaterThan(2)
                .And
                .OnlyContain(name => name.StartsWith("ac-library-csharp>"));
            embedded.SourceFiles.SelectMany(s => s.Usings)
                .Contains("using System.Runtime.Intrinsics.X86;")
                .Should()
                .Be(useIntrinsics);
        }

        [EmbeddingFact]
        public async Task LanguageVersion()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
            embedded.AssemblyMetadatas
                .Should().ContainKey("SourceExpander.EmbeddedLanguageVersion")
                .WhoseValue.Should().Be(languageVersion);
        }

        [EmbeddingFact]
        public async Task EmbeddedSource()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
            embedded.AssemblyMetadatas.Should().ContainKey("SourceExpander.EmbeddedSourceCode.GZipBase32768");
            embedded.SourceFiles.Select(s => s.FileName)
                .Should()
                .HaveCountGreaterThan(2)
                .And
                .OnlyContain(name => name.StartsWith("ac-library-csharp>"));
            embedded.SourceFiles.SelectMany(s => s.TypeNames)
                .Should()
                .Contain(
                "AtCoder.Operators.IArithmeticOperator<T>",
                "AtCoder.Segtree<TValue, TOp>");
        }

        [EmbeddingFact]
        public async Task EmbeddedNamespaces()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
            embedded.AssemblyMetadatas.Should().ContainKey("SourceExpander.EmbeddedNamespaces");
            embedded.EmbeddedNamespaces.Should()
                .Equal(
                "AtCoder",
                "AtCoder.Extension",
                "AtCoder.Internal",
                "AtCoder.Operators");
        }


        [EmbeddingFact]
        public async Task RemoveContract()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
            var codes = embedded.SourceFiles.Select(s => s.CodeBody);
            codes.Should().NotContain(code => code.Contains("Contract.Assert"));
        }

        [EmbeddingFact]
        public async Task SegtreeDebugView()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
            var source = embedded.SourceFiles.Single(s => s.FileName.Split('\\', '/').Last() == "Segtree.cs");
            source.CodeBody.Should()
                .EndWith("private class DebugView{private readonly Segtree<TValue,TOp>s;public DebugView(Segtree<TValue,TOp>segtree){s=segtree;}}}}")
                .And
                .NotContain("DebugItem");
        }

        [EmbeddingFact]
        public async Task LazySegtreeDebugView()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
            var source = embedded.SourceFiles.Single(s => s.FileName.Split('\\', '/').Last() == "LazySegtree.cs");
            source.CodeBody.Should()
                .EndWith("private class DebugView{private readonly LazySegtree<TValue,F,TOp>s;public DebugView(LazySegtree<TValue,F,TOp>segtree){s=segtree;}}}}")
                .And
                .NotContain("DebugItem");
        }

        [EmbeddingGenericMathFact]
        public async Task FenwickTreeGenericMathDebugView()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
            var source = embedded.SourceFiles.Single(s => s.FileName.Split('\\', '/').Last() == "FenwickTree.GenericMath.cs");
            source.CodeBody.Should()
                .EndWith("private class DebugView{private readonly FenwickTree<TValue>fw;public DebugView(FenwickTree<TValue>fenwickTree){fw=fenwickTree;}}}}")
                .And
                .NotContain("DebugItem");
        }

        [EmbeddingFact]
        public async Task FenwickTreeDebugView()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
            var source = embedded.SourceFiles.Single(s => s.FileName.Split('\\', '/').Last() == "FenwickTree.cs");
            source.CodeBody.Should()
                .EndWith("private class DebugView{private readonly FenwickTree<TValue,TOp>fw;public DebugView(FenwickTree<TValue,TOp>fenwickTree){fw=fenwickTree;}}}}")
                .And
                .NotContain("DebugItem");
        }

#if DEBUG
        [EmbeddingFact]
        public void DebugExpanded()
        {
            _ = typeof(SourceExpander.Embedded.Expand.AtCoder.Segtree<,>);
            _ = typeof(SourceExpander.Embedded.Expand.AtCoder.Mod1000000007);
        }
#endif
    }
}
