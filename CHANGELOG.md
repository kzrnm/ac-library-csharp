# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [3.9.2] - 2024-02-08
### Added
- Add DebuggerDisplay to ModInt.

## [3.9.1] - 2024-12-28
### Changed
- Shorten a name of `Dsu.parentOrSize`
- Refactor `ConvolutionImpl`
- Refactor `ModPrimitiveRoot`

## [3.9.0]
### Changed
- `ILazySegtreeOperator<T, F>` implements `ISegtreeOperator<T>`

## [3.8.0] - 2023-12-03
### Added
- Create `Segtree<T>` from Span

## [3.7.0] - 2023-11-29
### Added
- Add PriorityQueue.TryPeek(out T)
- Add PriorityQueue.DequeueEnqueue(T)

## [3.6.2] - 2023-11-26
### Changed
- Fix EnqueueDequeue

## [3.6.1] - 2023-11-25
### Changed
- Fix doc of FloorSum

## [3.6.0] - 2023-11-23
### Added
- Add `Parse`/`TryParse` to `ModInt`

## [3.5.0] - 2023-10-29
### Added
- Add MfGraph.Count
### Changed
- Rename generic parameter TValue to T

## [3.4.1] - 2023-10-26
### Changed
- Rename `AtCoder.Internal.BigMul` into `AtCoder.Internal.Mul128`

## [3.4.0] - 2023-10-26
### Changed
- Separate `MathLib` and `InternalMath` implementations in separate classes

## [3.3.0] - 2023-10-25
### Added
- Add `IModInt<T>.Value` and `IModInt<T>.Mod`
### Changed
- Move `IStaticMod` to IModInterface.cs
### Removed
- Remove DebugView from expanded code

## [3.2.0] - 2023-09-25
### Added
- Add `Deque<T>.Grow(int capacity)`

### Changed
- `AtCoder.Internal.Barrett` for `2^31` < m < `2^32`
- Fix empty `Deque<T>.GetEnumerator()`

## [3.1.0] - 2023-09-24
### Added
- Add IModInt interface

## [3.0.2] - 2023-09-21
### Changed
- AtCoderAnalyzer: Create operator as readonly struct

## [3.0.1] - 2023-09-12
### Change
- Update DebugView
- Rename IMinMaxValue<T> to IMinMaxValueOperator<T>

## [3.0.0-pre8] - 2023-01-27
### Added
- Add SimpleList<T>.RemoveLastSize

## [3.0.0-pre7] - 2023-01-26
### Changed
- Optimize PowMod"

## [3.0.0-pre6] - 2023-01-26
### Added
- Change accessibility of some Internal* class members public

## [3.0.0-pre5] - 2023-01-26
### Changed
- Fix LCPArray → LcpArray

## [3.0.0-pre4] - 2023-01-26
### Added
- Change accessibility of some members to `[EditorBrowsable(Never)] public`

## [3.0.0-pre3] - 2023-01-26
### Added
- Multiply 128Bit in barrett reduction

## [3.0.0-pre2] - 2023-01-25
### Added
- AtCoderAnalyzer: Create operator for static abstract
### Changed
- Rename DynamicModID to DynamicModIntId

## [3.0.0-pre1] - 2023-01-24
### Changed
- Rename assembly from AtCoderLibrary to ac-library-csharp
- Rename some methods/classes to PascalCase

## [3.0.0-atcoder1] - 2023-01-24
### Added
- Make SourceExpander.Embedder switchable

### Removed
- Disable some Contract.Assert

## [2.1.1] - 2023-01-17
### Changed
- Optimize PrimitiveRoot

## [2.1.0] - 2023-01-07
### Changed
- Fix capacity of Deque<T>

## [2.0.1] - 2022-12-24
### Added
- Add DebuggerDisplay

## [2.0.0] - 2022-12-07
### Added
- Generic Math
 
## [1.20.0] - 2022-06-18
### Added
- StaticModInt<T>, DynamicModInt<T>
  - add Zero and One to StaticModInt<T>, DynamicModInt<T>
  - implements IFormattable
  - implicit cast to ulong

## [1.19.0] - 2022-05-31
### Added
- AtCoderAnalyzer_UseMethodImplNumeric

## [1.18.0] - 2022-05-18
### Added
- PriorityQueue.DequeueEnqueue

## [1.17.0] - 2022-04-25
### Changed
- Bug fix: new Deque<T>(capacity)

## [1.16.0] - 2022-04-13
### Changed
- Bug fix: -ModInt(0) should be 0

## [1.15.0] - 2022-04-11
### Added
- Add McfGraph.Slope2

## [1.14.0] - 2022-03-28
### Added
- Analyzer: Optimize CodeFix

## [1.13.0] - 2022-03-25
### Added
- Allow using `IComparable<T>` as `BinarySearch` argument

## [1.12.0] - 2022-03-25
### Changed
- Optimize BinarySearch

## [1.11.2] - 2022-03-20
### Changed
- SourceExpander 5.0.0

## [1.11.1] - 2022-03-08
- immaterial changes

## [1.11.0] - 2022-03-05
- Add AggressiveInlining

## [1.10.0] - 2022-02-27
- Fix analyzer AggressiveInlining
- Update libraries
  - SourceExpander 4.1.1

## [1.9.1] - 2022-01-25
### Changed
- Update libraries
- Move ISegtreeOperator and ILazySegtreeOperator

## [1.9.0] - 2022-01-19
### Added
- Add PriorityQueue.EnqueueDequeue

## [1.8.0] - 2021-10-18
- **Breaking** Move operator interfaces to AtCoder.Operators namespace
- **Breaking** Separate ISubtractOperator from IAdditionOperator
- Update CreateOperatorCodeFixProvider

## [1.7.0] - 2021-09-14
- Raname PriorityQueue<TKey, TValue> to PriorityQueueDictionary<TKey, TValue>
- Optimize PriorityQueue

## [1.6.5] - 2021-06-28
- Optimize MaxFlow
- **Breaking** Rename some classes to camel case

## [1.6.4] - 2021-06-21
- **Breaking** Rename namespace of STL classes to AtCoder.Stl to AtCoder
- **Breaking** Rename namespace of binary search classes to AtCoder.Extension

## [1.6.3] - 2021-06-21
### Changed
- Fix typo in xml doc

## [1.6.2] - 2021-06-21
### Changed
- Split IMinMaxValue<T>

## [1.6.1] - 2021-06-19
### Removed
- **Breaking** Remove IEnumerable implementation from PriorityQueue

## [1.6.0] - 2021-06-19
### Changed
- Remove EditorBrowsableState.Never from Contract.
- Hide Deque<T>.Add
- **Breaking** Rename namespace of STL classes to AtCoder.Stl
- **Breaking** Raname PriorityQueue<T>.Add to PriorityQueue<T>.Enqueue  (thx @fairy-lettuce) https://github.com/kzrnm/ac-library-csharp/issues/53

## [1.5.6] - 2021-06-04
### Added
- Add AggressiveInlining

## [1.5.5] - 2021-06-03
### Added
- Add AggressiveInlining
- Enumerate CSR edge

## [1.5.4]
### Added
- Add AsMemory() to SimpleList<T>

## [1.5.3]
### Changed
- Relax type constraint of DynamicModInt<T> https://github.com/kzrnm/ac-library-csharp/pull/51
- Rename ModID* to DynamicModID* https://github.com/kzrnm/ac-library-csharp/pull/51

## [1.5.2] - 2021-04-11
### Added
- Add debug method to PriorityQueue https://github.com/kzrnm/ac-library-csharp/pull/50

## [1.5.1] - 2021-04-08
### Added
- Add SuffixArray that takes Span parameter https://github.com/kzrnm/ac-library-csharp/pull/48

## [1.5.0] - 2021-04-08
### Added
- Add unsigned constructor to modint https://github.com/kzrnm/ac-library-csharp/pull/45
- Add span overloads to convolution https://github.com/kzrnm/ac-library-csharp/pull/45
### Changed
- AtCoderAnalyzer can run parallel
- Improve FloorSum https://github.com/kzrnm/ac-library-csharp/pull/42
- Optimize FenwickTree https://github.com/kzrnm/ac-library-csharp/pull/47
- Optimize Convolution https://github.com/kzrnm/ac-library-csharp/pull/47
- Optimize SuffixArray https://github.com/kzrnm/ac-library-csharp/pull/47

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
