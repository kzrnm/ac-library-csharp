using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using FluentAssertions;
using Kzrnm.Convert.Base32768;
using Xunit;

namespace AtCoder.Embedding
{
    public class SourceExpanderTest
    {
        private readonly Dictionary<string, string> assemblyMetadata
            = typeof(IntFenwickTree).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .ToDictionary(attr => attr.Key, attr => attr.Value);

        [Fact]
        public void LanguageVersion()
            => assemblyMetadata.Should().ContainKey("SourceExpander.EmbeddedLanguageVersion")
                .WhichValue.Should().Be("8.0");

        [Fact]
        public async void EmbeddedSource()
        {
            var embeddedSourceGZipJson = Base32768.Decode(assemblyMetadata.Should().ContainKey("SourceExpander.EmbeddedSourceCode.GZipBase32768").WhichValue);
            using var msIn = new MemoryStream(embeddedSourceGZipJson);
            using var gzipStream = new GZipStream(msIn, CompressionMode.Decompress);
            var embeddedSource = await JsonSerializer.DeserializeAsync<SourceFileInfo[]>(gzipStream);
            embeddedSource.Select(s => s.FileName)
                .Should()
                .OnlyContain(name => name.StartsWith("AtCoderLibrary>"));
            embeddedSource.SelectMany(s => s.TypeNames)
                .Should()
                .Contain(new[] {
                    "AtCoder.IArithmeticOperator<T>",
                    "AtCoder.FenwickTree<TValue, TOp>",
                });
        }

        private class SourceFileInfo
        {
            public string FileName { get; set; }
            public IEnumerable<string> TypeNames { get; set; }
            //public IEnumerable<string> Usings { get; set; }
            //public IEnumerable<string> Dependencies { get; set; }
            //public string CodeBody { get; set; }
        }
    }
}
