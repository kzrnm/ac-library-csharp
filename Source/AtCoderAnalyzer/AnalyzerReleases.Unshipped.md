; Unshipped analyzer release
; https://github.com/dotnet/roslyn-analyzers/blob/master/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md


### Removed Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
AC0003 | Type Define | Error | Not defined IStaticMod
AC0004 | Type Define | Error | Not defined IDynamicModID
AC0005 | Type Define | Error | Not defined ISegtreeOperator<T>
AC0006 | Type Define | Error | Not defined ILazySegtreeOperator<T, F>

### Changed Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
AC0007 | Type Define | Info | Some operator methods don't have `MethodImpl` attribute