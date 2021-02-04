# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.2.4] - 2021-02-04
### Changed
- FenwickTree, Segtree and LazySegtree display value in debug view.

## [1.2.3] - 2021-02-04
### Changed
- Parameter with Modifiers in CreateOperatorCodeFixProvider

## [1.2.2] - 2021-02-04
### Changed
Rename ILazySegtreeOperator.Composition args

## [1.2.1] - 2021-02-02
### Changed
- Create void method with empty block in CreateOperatorCodeFixProvider

## [1.2.0] - 2021-01-18
### Added
- Add SimpleList<T>.Sort(), SimpleList<T>.Reverse()
- Add ValueTuple name to CRT

### Changed
- Avoid recursive in scc
- Avoid recursive in MFGraph
- Replace DebugUtil.Assert to Contract.Assert

## [1.1.0] - 2021-01-10
### Added
- Add SimpleList<T>

### Changed
- Optimize MaxFlow

## [1.0.8] - 2021-01-05
### Added
- Add IShiftOperator interface

### Changed
- Split operator interfaces

## [1.0.7] - 2021-01-03
### Added
- Regard System.Collections.Generic.IComparer<T> as operator type

## [1.0.6] - 2020-12-26
### Added

- GenerateDocumentationFile
- Split files
- Use EditorBrowsable(EditorBrowsableState.Never) instead of private or internal
- Use Generic Comparer in BinarySearch, PriorityQueue

### Changed
- Raise AC0008 on Method
- Follow ac-library v1.3
- [Bug fix]Create all members in CreateOperatorCodeFixProvider
- BinarySearch targets only IComparable<T>
- Remove struct constraint from operator

## [1.0.5] - 2020-12-18
### Added
- SourceLink

### Changed
- fix no AC0008 on OmittedTypeArgument
- Minify embedded source code

## [1.0.4] - 2020-12-09
### Added

- Add Analyzer for operator type
