# AtCoderAnalyzers

https://www.nuget.org/packages/AtCoderAnalyzer

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
[AC0001](AC0001.md) | Overflow | Warning | int multiply expression is assigned to long
[AC0002](AC0002.md) | Overflow | Warning | int left shift expression is assigned to long
[AC0003](AC0003.md) | Type Define | Error | Not defined `IStaticMod`
[AC0004](AC0004.md) | Type Define | Error | Not defined `IDynamicModID`
[AC0005](AC0005.md) | Type Define | Error | Not defined `ISegtreeOperator<T>`
[AC0006](AC0006.md) | Type Define | Error | Not defined `ILazySegtreeOperator<T, F>`
[AC0007](AC0007.md) | Type Define | Info | Operator method  doesn't have `[MethodImpl(MethodImplOptions.AggressiveInlining)]` attribute
