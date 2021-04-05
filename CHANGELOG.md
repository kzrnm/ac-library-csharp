# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Added
- Add unsigned constructor to modint https://github.com/naminodarie/ac-library-csharp/pull/45
- Add span overloads to convolution https://github.com/naminodarie/ac-library-csharp/pull/45
### Changed
- AtCoderAnalyzer can run parallel
- Improve FloorSum https://github.com/naminodarie/ac-library-csharp/pull/42
- Optimize SuffixArray(string)

## [1.4.4] - 2021-03-05
### Changed
- Rename AtCoder.Internal.SCCGraph to AtCoder.Internal.InternalSCCGraph

## [1.4.3] - 2021-03-05
### Changed
- Deque<T>.operator[int] returns reference
### Removed
- PriorityQueue<TKey, TValue> is obsolete in .NET 6 or newer

## [1.4.2] - 2021-02-16
### Changed
- Avoid using List<T>
- Improve SimpleList<T>

## [1.4.1] - 2021-02-15
### Added
- Rename AtCoder.Internal.BitOperations

## [1.4.0] - 2021-02-15
### Added
- Support .NET Standard 2.1 and C# 7.3

## [1.3.0] - 2021-02-11
### Changed
- Update CreateOperatorCodeFixProvider
- Use Span<T>.Fill
- CeilPow2 AggressiveInlining

## [1.2.7] - 2021-02-09
### Changed
- Optimize PriorityQueue<TKey, TValue>

## [1.2.6] - 2021-02-09
### Added
- Add PriorityQueue.TryDequeue
- Add New StlFunction.NextPermutation method like std::next_permutation

### Changed
- Avoid Recursive call in PriorityQueue
- McfGraph use PriorityQueueOp
- Rename StlFunction.NextPermutation to StlFunction.Permutations

## [1.2.5] - 2021-02-06
### Added
- Add DebugView to SimpleList<T>

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
