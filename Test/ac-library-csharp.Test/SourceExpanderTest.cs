using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Shouldly;
using SourceExpander;
using Xunit;

namespace AtCoder.Embedding;

#if DEBUG
public partial class SourceExpanderTest
{
    class EmbeddingFact : FactAttribute
    {
        public override string Skip => "SourceExpander.Embedder is disabled.";
    }

#if NET9_0
    const string languageVersion = "13.0";
#elif NET10_0
    const string languageVersion = "14.0";
#endif

    [Fact]
    public async Task Embedding()
    {
        var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
        embedded.AssemblyMetadatas.Keys.ShouldContain(k => Regex.IsMatch(k, "SourceExpander.*"));
    }

    [EmbeddingFact]
    public void AssemblyName()
    {
        typeof(Segtree<,>).Assembly.GetName().Name.ShouldBe("ac-library-csharp");
    }

    [EmbeddingFact]
    public async Task Symbol()
    {
        var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
        embedded.AssemblyMetadatas.ShouldContainKey("SourceExpander.EmbeddedSourceCode.GZipBase32768");
        embedded.SourceFiles.Select(s => s.FileName).ToArray().ShouldSatisfyAllConditions([
            s => s.Length.ShouldBeGreaterThan(2),
            s => s.ShouldAllBe(name => name.StartsWith("ac-library-csharp>")),
        ]);
        embedded.SourceFiles.SelectMany(s => s.Usings)
            .Contains("using System.Runtime.Intrinsics.X86;")
            .ShouldBe(true);
    }

    [EmbeddingFact]
    public async Task LanguageVersion()
    {
        var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
        embedded.AssemblyMetadatas.ShouldContainKeyAndValue("SourceExpander.EmbeddedLanguageVersion", languageVersion);
    }

    [EmbeddingFact]
    public async Task EmbeddedSource()
    {
        var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
        embedded.AssemblyMetadatas.ShouldContainKey("SourceExpander.EmbeddedSourceCode.GZipBase32768");
        embedded.SourceFiles.Select(s => s.FileName).ToArray().ShouldSatisfyAllConditions([
            s => s.Length.ShouldBeGreaterThan(2),
            s => s.ShouldAllBe(name => name.StartsWith("ac-library-csharp>")),
        ]);
        embedded.SourceFiles.SelectMany(s => s.TypeNames).ToArray()
            .ShouldSatisfyAllConditions([
            t => t.ShouldContain("AtCoder.Operators.IArithmeticOperator<T>"),
            t => t.ShouldContain("AtCoder.Segtree<TValue, TOp>"),
        ]);
    }

    [EmbeddingFact]
    public async Task EmbeddedNamespaces()
    {
        var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
        embedded.AssemblyMetadatas.ShouldContainKey("SourceExpander.EmbeddedNamespaces");
        embedded.EmbeddedNamespaces.ShouldBe([
            "AtCoder",
            "AtCoder.Extension",
            "AtCoder.Internal",
            "AtCoder.Operators",
        ]);
    }

    [EmbeddingFact]
    public async Task RemoveContract()
    {
        var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
        var code = string.Join(' ', embedded.SourceFiles.Select(s => s.CodeBody));
        code.ShouldNotContain("Contract.Assert");
    }

    [EmbeddingFact]
    public async Task RemoveDebugView()
    {
        var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
        var code = string.Join(' ', embedded.SourceFiles.Select(s => s.CodeBody));
        code.ShouldContain("CollectionDebugView");
        code.Replace("CollectionDebugView", "CoDe").ShouldNotContain("Debug");
    }

    [EmbeddingFact]
    public async Task FenwickTreeGenericMath()
    {
        var embedded = await EmbeddedData.LoadFromAssembly(typeof(Segtree<,>));
        var source = embedded.SourceFiles.Single(s => s.FileName.Split('\\', '/').Last() == "FenwickTree.GenericMath.cs");
        source.CodeBody.ShouldContain("FenwickTree<T>");
        source.CodeBody.ShouldNotContain("Debug");
    }

#if DEBUG
    [Fact]
    public void DebugExpanded()
    {
        _ = typeof(SourceExpander.Embedded.Expand.AtCoder.Segtree<,>);
        _ = typeof(SourceExpander.Embedded.Expand.AtCoder.Mod1000000007);
    }
#endif
}
#endif
