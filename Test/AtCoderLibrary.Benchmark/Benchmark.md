``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.201
  [Host]   : .NET Core 3.1.13 (CoreCLR 4.700.21.11102, CoreFX 4.700.21.11602), X64 RyuJIT
  ShortRun : .NET Core 3.1.13 (CoreCLR 4.700.21.11102, CoreFX 4.700.21.11602), X64 RyuJIT

Job=ShortRun  Toolchain=.NET Core 3.1  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|               Method |       Mean |     Error |   StdDev |
|--------------------- |-----------:|----------:|---------:|
|          Convolution | 1,297.3 ms | 312.41 ms | 17.12 ms |
| Convolution377487361 | 1,225.0 ms |  28.15 ms |  1.54 ms |
|          FenwickTree |   880.7 ms | 372.55 ms | 20.42 ms |
|          LazySegtree | 1,449.4 ms | 301.91 ms | 16.55 ms |
|  LazySegtreeMaxRight | 1,255.6 ms | 281.89 ms | 15.45 ms |
|             McfGraph |   902.1 ms |  43.29 ms |  2.37 ms |
|              MfGraph |   281.0 ms | 190.26 ms | 10.43 ms |
|             MfGraph1 |   352.7 ms | 281.72 ms | 15.44 ms |
|                  SCC |   922.7 ms | 291.31 ms | 15.97 ms |
|    SatisfiableTwoSat |   307.5 ms | 710.52 ms | 38.95 ms |
|              Segtree |   896.9 ms | 772.02 ms | 42.32 ms |
|      SegtreeMaxRight |   606.4 ms | 165.41 ms |  9.07 ms |
|      SuffixArrayLong |   273.9 ms | 532.51 ms | 29.19 ms |
|    SuffixArrayString |   530.3 ms | 148.93 ms |  8.16 ms |
|           ZAlgorithm |   169.1 ms | 148.91 ms |  8.16 ms |
