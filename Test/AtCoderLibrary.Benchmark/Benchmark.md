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
|          Convolution | 1,235.5 ms |  47.25 ms |  2.59 ms |
| Convolution377487361 | 1,233.8 ms | 303.75 ms | 16.65 ms |
|          FenwickTree |   886.0 ms | 100.76 ms |  5.52 ms |
|          LazySegtree | 1,377.0 ms | 231.94 ms | 12.71 ms |
|  LazySegtreeMaxRight | 1,242.4 ms | 251.01 ms | 13.76 ms |
|             McfGraph |   939.8 ms | 197.49 ms | 10.82 ms |
|              MfGraph |   289.7 ms | 380.55 ms | 20.86 ms |
|             MfGraph1 |   357.1 ms | 370.16 ms | 20.29 ms |
|                  SCC |   922.9 ms | 150.73 ms |  8.26 ms |
|    SatisfiableTwoSat |   240.8 ms | 134.69 ms |  7.38 ms |
|              Segtree |   741.2 ms |  90.06 ms |  4.94 ms |
|      SegtreeMaxRight |   587.2 ms | 268.77 ms | 14.73 ms |
|      SuffixArrayLong |   244.7 ms |  36.09 ms |  1.98 ms |
|    SuffixArrayString |   503.9 ms | 118.66 ms |  6.50 ms |
|           ZAlgorithm |   159.5 ms | 119.88 ms |  6.57 ms |
