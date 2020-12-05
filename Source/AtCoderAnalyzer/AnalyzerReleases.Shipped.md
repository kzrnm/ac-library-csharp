; Shipped analyzer releases
; https://github.com/dotnet/roslyn-analyzers/blob/master/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md


### Version 1.0.2

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
AC0001 | Overflow | Warning | int multiply expression is assigned to long
AC0002 | Overflow | Warning | int left shift expression is assigned to long
AC0003 | Type Define | Error | Not defined IStaticMod
AC0004 | Type Define | Error | Not defined IDynamicModID
AC0005 | Type Define | Error | Not defined ISegtreeOperator<T>
AC0006 | Type Define | Error | Not defined ILazySegtreeOperator<T, F>
AC0007 | Type Define | Info | Operator method  doesn't have [MethodImpl(MethodImplOptions.AggressiveInlining)] attribute