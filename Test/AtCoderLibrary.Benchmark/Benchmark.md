``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.201
  [Host]   : .NET Core 3.1.13 (CoreCLR 4.700.21.11102, CoreFX 4.700.21.11602), X64 RyuJIT
  ShortRun : .NET Core 3.1.13 (CoreCLR 4.700.21.11102, CoreFX 4.700.21.11602), X64 RyuJIT

Job=ShortRun  Toolchain=.NET Core 3.1  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|              Method |       Mean |     Error |   StdDev |
|-------------------- |-----------:|----------:|---------:|
|         Convolution | 2,484.0 ms | 566.46 ms | 31.05 ms |
|         FenwickTree | 2,047.4 ms | 942.11 ms | 51.64 ms |
|         LazySegtree | 1,362.7 ms | 201.68 ms | 11.05 ms |
| LazySegtreeMaxRight | 2,567.6 ms |  99.79 ms |  5.47 ms |
|            McfGraph |   900.3 ms | 159.50 ms |  8.74 ms |
|             MfGraph |   549.4 ms | 354.94 ms | 19.46 ms |
|            MfGraph1 |   726.2 ms | 220.67 ms | 12.10 ms |
|                 SCC | 1,893.8 ms |  26.47 ms |  1.45 ms |
|   SatisfiableTwoSat | 1,981.5 ms | 492.09 ms | 26.97 ms |
|             Segtree | 1,478.8 ms |  50.42 ms |  2.76 ms |
|     SegtreeMaxRight | 1,139.0 ms | 193.17 ms | 10.59 ms |
|     SuffixArrayLong | 2,242.8 ms | 139.99 ms |  7.67 ms |
|   SuffixArrayString | 2,214.2 ms | 450.91 ms | 24.72 ms |
|          ZAlgorithm | 1,189.4 ms | 663.66 ms | 36.38 ms |
