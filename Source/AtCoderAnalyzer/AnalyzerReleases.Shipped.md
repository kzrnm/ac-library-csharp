﻿; Shipped analyzer releases
; https://github.com/dotnet/roslyn-analyzers/blob/master/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

## Release 1.0.2

### New Rules
Rule ID | Category | Severity | Notes
--------|----------|----------|-------
AC0001 | Overflow | Warning | int multiply expression is assigned to long
AC0002 | Overflow | Warning | int left shift expression is assigned to long
AC0003 | Type Define | Error | Not defined IStaticMod
AC0004 | Type Define | Error | Not defined IDynamicModID
AC0005 | Type Define | Error | Not defined ISegtreeOperator<T>
AC0006 | Type Define | Error | Not defined ILazySegtreeOperator<T, F>
AC0007 | Type Define | Info | Some operator methods don't have `MethodImpl` attribute

## Release 1.0.4

### New Rules
Rule ID | Category | Severity | Notes
--------|----------|----------|-------
AC0008 | Type Define | Error | Not defined operator type

### Removed Rules
Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
AC0003 | Type Define | Error | Not defined IStaticMod
AC0004 | Type Define | Error | Not defined IDynamicModID
AC0005 | Type Define | Error | Not defined ISegtreeOperator<T>
AC0006 | Type Define | Error | Not defined ILazySegtreeOperator<T, F>
