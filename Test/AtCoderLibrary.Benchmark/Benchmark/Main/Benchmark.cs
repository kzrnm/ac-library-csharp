using System;
using System.IO;
using AtCoder;
using AtCoder.Internal;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;

class BenchmarkProgram
{
    static void Main()
    {
        BenchmarkRunner.Run(typeof(BenchmarkProgram).Assembly);
    }
}

public class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        //AddDiagnoser(MemoryDiagnoser.Default);
        AddExporter(BenchmarkDotNet.Exporters.MarkdownExporter.GitHub);
        AddJob(Job.ShortRun.WithToolchain(CsProjCoreToolchain.NetCoreApp31));
    }
}

namespace Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class Benchmark
    {
        int n = 1 << 24;
        
        [Benchmark] public long Convolution() => global::Convolution.Calc(n);
        [Benchmark] public long Convolution377487361() => global::Convolution377487361.Calc(n);
        [Benchmark] public long FenwickTree() => global::FenwickTree.Calc(n);
        [Benchmark] public long LazySegtree() => global::LazySegtree.Calc(n);
        [Benchmark] public long LazySegtreeMaxRight() => global::LazySegtreeMaxRight.Calc(n);
        [Benchmark] public long McfGraph() => global::McfGraph.Calc(n);
        [Benchmark] public long MfGraph() => global::MfGraph.Calc(n);
        [Benchmark] public long MfGraph1() => global::MfGraph1.Calc(n);
        [Benchmark] public long SCC() => global::SCC.Calc(n);
        [Benchmark] public long SatisfiableTwoSat() => global::SatisfiableTwoSat.Calc(n);
        [Benchmark] public long Segtree() => global::Segtree.Calc(n);
        [Benchmark] public long SegtreeMaxRight() => global::SegtreeMaxRight.Calc(n);
        [Benchmark] public long SuffixArrayLong() => global::SuffixArrayLong.Calc(n);
        [Benchmark] public long SuffixArrayString() => global::SuffixArrayString.Calc(n);
        [Benchmark] public long ZAlgorithm() => global::ZAlgorithm.Calc(n);
    }
}
