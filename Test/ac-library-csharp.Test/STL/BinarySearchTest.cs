using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace AtCoder.Extension
{
    public class BinarySearchTest
    {
        [Fact]
        public void Simple()
        {
            var arr = new int[] { 0, 1, 2, 3, 3, 3, 6, 7, 8, 9, 100 };

            arr.LowerBound(0).ShouldBe(0);
            ((IList<int>)arr).LowerBound(0).ShouldBe(0);
            ((Span<int>)arr).LowerBound(0).ShouldBe(0);
            ((ReadOnlySpan<int>)arr).LowerBound(0).ShouldBe(0);

            arr.UpperBound(0).ShouldBe(1);
            ((IList<int>)arr).UpperBound(0).ShouldBe(1);
            ((Span<int>)arr).UpperBound(0).ShouldBe(1);
            ((ReadOnlySpan<int>)arr).UpperBound(0).ShouldBe(1);

            arr.LowerBound(3).ShouldBe(3);
            ((IList<int>)arr).LowerBound(3).ShouldBe(3);
            ((Span<int>)arr).LowerBound(3).ShouldBe(3);
            ((ReadOnlySpan<int>)arr).LowerBound(3).ShouldBe(3);

            arr.UpperBound(3).ShouldBe(6);
            ((IList<int>)arr).UpperBound(3).ShouldBe(6);
            ((Span<int>)arr).UpperBound(3).ShouldBe(6);
            ((ReadOnlySpan<int>)arr).UpperBound(3).ShouldBe(6);

            arr.LowerBound(10).ShouldBe(10);
            ((IList<int>)arr).LowerBound(10).ShouldBe(10);
            ((Span<int>)arr).LowerBound(10).ShouldBe(10);
            ((ReadOnlySpan<int>)arr).LowerBound(10).ShouldBe(10);

            arr.UpperBound(10).ShouldBe(10);
            ((IList<int>)arr).UpperBound(10).ShouldBe(10);
            ((Span<int>)arr).UpperBound(10).ShouldBe(10);
            ((ReadOnlySpan<int>)arr).UpperBound(10).ShouldBe(10);

            arr.LowerBound(100).ShouldBe(10);
            ((IList<int>)arr).LowerBound(100).ShouldBe(10);
            ((Span<int>)arr).LowerBound(100).ShouldBe(10);
            ((ReadOnlySpan<int>)arr).LowerBound(100).ShouldBe(10);

            arr.UpperBound(100).ShouldBe(11);
            ((IList<int>)arr).UpperBound(100).ShouldBe(11);
            ((Span<int>)arr).UpperBound(100).ShouldBe(11);
            ((ReadOnlySpan<int>)arr).UpperBound(100).ShouldBe(11);

            arr.LowerBound(101).ShouldBe(11);
            ((IList<int>)arr).LowerBound(101).ShouldBe(11);
            ((Span<int>)arr).LowerBound(101).ShouldBe(11);
            ((ReadOnlySpan<int>)arr).LowerBound(101).ShouldBe(11);

            arr.UpperBound(101).ShouldBe(11);
            ((IList<int>)arr).UpperBound(101).ShouldBe(11);
            ((Span<int>)arr).UpperBound(101).ShouldBe(11);
            ((ReadOnlySpan<int>)arr).UpperBound(101).ShouldBe(11);
        }

        [Fact]
        public void Comparer()
        {
            var arr = new int[] { 100, 9, 8, 7, 6, 3, 3, 3, 2, 1, 0 };

            arr.LowerBound(0, ComparerUtil.ReverseComparerInt).ShouldBe(10);
            ((IList<int>)arr).LowerBound(0, ComparerUtil.ReverseComparerInt).ShouldBe(10);
            ((Span<int>)arr).LowerBound(0, ComparerUtil.ReverseComparerInt).ShouldBe(10);
            ((ReadOnlySpan<int>)arr).LowerBound(0, ComparerUtil.ReverseComparerInt).ShouldBe(10);

            arr.UpperBound(0, ComparerUtil.ReverseComparerInt).ShouldBe(11);
            ((IList<int>)arr).UpperBound(0, ComparerUtil.ReverseComparerInt).ShouldBe(11);
            ((Span<int>)arr).UpperBound(0, ComparerUtil.ReverseComparerInt).ShouldBe(11);
            ((ReadOnlySpan<int>)arr).UpperBound(0, ComparerUtil.ReverseComparerInt).ShouldBe(11);

            arr.LowerBound(3, ComparerUtil.ReverseComparerInt).ShouldBe(5);
            ((IList<int>)arr).LowerBound(3, ComparerUtil.ReverseComparerInt).ShouldBe(5);
            ((Span<int>)arr).LowerBound(3, ComparerUtil.ReverseComparerInt).ShouldBe(5);
            ((ReadOnlySpan<int>)arr).LowerBound(3, ComparerUtil.ReverseComparerInt).ShouldBe(5);

            arr.UpperBound(3, ComparerUtil.ReverseComparerInt).ShouldBe(8);
            ((IList<int>)arr).UpperBound(3, ComparerUtil.ReverseComparerInt).ShouldBe(8);
            ((Span<int>)arr).UpperBound(3, ComparerUtil.ReverseComparerInt).ShouldBe(8);
            ((ReadOnlySpan<int>)arr).UpperBound(3, ComparerUtil.ReverseComparerInt).ShouldBe(8);

            arr.LowerBound(10, ComparerUtil.ReverseComparerInt).ShouldBe(1);
            ((IList<int>)arr).LowerBound(10, ComparerUtil.ReverseComparerInt).ShouldBe(1);
            ((Span<int>)arr).LowerBound(10, ComparerUtil.ReverseComparerInt).ShouldBe(1);
            ((ReadOnlySpan<int>)arr).LowerBound(10, ComparerUtil.ReverseComparerInt).ShouldBe(1);

            arr.UpperBound(10, ComparerUtil.ReverseComparerInt).ShouldBe(1);
            ((IList<int>)arr).UpperBound(10, ComparerUtil.ReverseComparerInt).ShouldBe(1);
            ((Span<int>)arr).UpperBound(10, ComparerUtil.ReverseComparerInt).ShouldBe(1);
            ((ReadOnlySpan<int>)arr).UpperBound(10, ComparerUtil.ReverseComparerInt).ShouldBe(1);

            arr.LowerBound(100, ComparerUtil.ReverseComparerInt).ShouldBe(0);
            ((IList<int>)arr).LowerBound(100, ComparerUtil.ReverseComparerInt).ShouldBe(0);
            ((Span<int>)arr).LowerBound(100, ComparerUtil.ReverseComparerInt).ShouldBe(0);
            ((ReadOnlySpan<int>)arr).LowerBound(100, ComparerUtil.ReverseComparerInt).ShouldBe(0);

            arr.UpperBound(100, ComparerUtil.ReverseComparerInt).ShouldBe(1);
            ((IList<int>)arr).UpperBound(100, ComparerUtil.ReverseComparerInt).ShouldBe(1);
            ((Span<int>)arr).UpperBound(100, ComparerUtil.ReverseComparerInt).ShouldBe(1);
            ((ReadOnlySpan<int>)arr).UpperBound(100, ComparerUtil.ReverseComparerInt).ShouldBe(1);

            arr.LowerBound(101, ComparerUtil.ReverseComparerInt).ShouldBe(0);
            ((IList<int>)arr).LowerBound(101, ComparerUtil.ReverseComparerInt).ShouldBe(0);
            ((Span<int>)arr).LowerBound(101, ComparerUtil.ReverseComparerInt).ShouldBe(0);
            ((ReadOnlySpan<int>)arr).LowerBound(101, ComparerUtil.ReverseComparerInt).ShouldBe(0);

            arr.UpperBound(101, ComparerUtil.ReverseComparerInt).ShouldBe(0);
            ((IList<int>)arr).UpperBound(101, ComparerUtil.ReverseComparerInt).ShouldBe(0);
            ((Span<int>)arr).UpperBound(101, ComparerUtil.ReverseComparerInt).ShouldBe(0);
            ((ReadOnlySpan<int>)arr).UpperBound(101, ComparerUtil.ReverseComparerInt).ShouldBe(0);

            arr.LowerBound(-1, ComparerUtil.ReverseComparerInt).ShouldBe(11);
            ((IList<int>)arr).LowerBound(-1, ComparerUtil.ReverseComparerInt).ShouldBe(11);
            ((Span<int>)arr).LowerBound(-1, ComparerUtil.ReverseComparerInt).ShouldBe(11);
            ((ReadOnlySpan<int>)arr).LowerBound(-1, ComparerUtil.ReverseComparerInt).ShouldBe(11);

            arr.UpperBound(-1, ComparerUtil.ReverseComparerInt).ShouldBe(11);
            ((IList<int>)arr).UpperBound(-1, ComparerUtil.ReverseComparerInt).ShouldBe(11);
            ((Span<int>)arr).UpperBound(-1, ComparerUtil.ReverseComparerInt).ShouldBe(11);
            ((ReadOnlySpan<int>)arr).UpperBound(-1, ComparerUtil.ReverseComparerInt).ShouldBe(11);
        }

        [Fact]
        public void Comparable()
        {
            var arr = new int[] { 0, 1, 2, 3, 3, 3, 6, 7, 8, 9, 100 };

            arr.LowerBound(new ArrayLengthComparable(0)).ShouldBe(0);
            ((IList<int>)arr).LowerBound(new ArrayLengthComparable(0)).ShouldBe(0);
            ((Span<int>)arr).LowerBound(new ArrayLengthComparable(0)).ShouldBe(0);
            ((ReadOnlySpan<int>)arr).LowerBound(new ArrayLengthComparable(0)).ShouldBe(0);

            arr.UpperBound(new ArrayLengthComparable(0)).ShouldBe(1);
            ((IList<int>)arr).UpperBound(new ArrayLengthComparable(0)).ShouldBe(1);
            ((Span<int>)arr).UpperBound(new ArrayLengthComparable(0)).ShouldBe(1);
            ((ReadOnlySpan<int>)arr).UpperBound(new ArrayLengthComparable(0)).ShouldBe(1);

            arr.LowerBound(new ArrayLengthComparable(3)).ShouldBe(3);
            ((IList<int>)arr).LowerBound(new ArrayLengthComparable(3)).ShouldBe(3);
            ((Span<int>)arr).LowerBound(new ArrayLengthComparable(3)).ShouldBe(3);
            ((ReadOnlySpan<int>)arr).LowerBound(new ArrayLengthComparable(3)).ShouldBe(3);

            arr.UpperBound(new ArrayLengthComparable(3)).ShouldBe(6);
            ((IList<int>)arr).UpperBound(new ArrayLengthComparable(3)).ShouldBe(6);
            ((Span<int>)arr).UpperBound(new ArrayLengthComparable(3)).ShouldBe(6);
            ((ReadOnlySpan<int>)arr).UpperBound(new ArrayLengthComparable(3)).ShouldBe(6);

            arr.LowerBound(new ArrayLengthComparable(10)).ShouldBe(10);
            ((IList<int>)arr).LowerBound(new ArrayLengthComparable(10)).ShouldBe(10);
            ((Span<int>)arr).LowerBound(new ArrayLengthComparable(10)).ShouldBe(10);
            ((ReadOnlySpan<int>)arr).LowerBound(new ArrayLengthComparable(10)).ShouldBe(10);

            arr.UpperBound(new ArrayLengthComparable(10)).ShouldBe(10);
            ((IList<int>)arr).UpperBound(new ArrayLengthComparable(10)).ShouldBe(10);
            ((Span<int>)arr).UpperBound(new ArrayLengthComparable(10)).ShouldBe(10);
            ((ReadOnlySpan<int>)arr).UpperBound(new ArrayLengthComparable(10)).ShouldBe(10);

            arr.LowerBound(new ArrayLengthComparable(100)).ShouldBe(10);
            ((IList<int>)arr).LowerBound(new ArrayLengthComparable(100)).ShouldBe(10);
            ((Span<int>)arr).LowerBound(new ArrayLengthComparable(100)).ShouldBe(10);
            ((ReadOnlySpan<int>)arr).LowerBound(new ArrayLengthComparable(100)).ShouldBe(10);

            arr.UpperBound(new ArrayLengthComparable(100)).ShouldBe(11);
            ((IList<int>)arr).UpperBound(new ArrayLengthComparable(100)).ShouldBe(11);
            ((Span<int>)arr).UpperBound(new ArrayLengthComparable(100)).ShouldBe(11);
            ((ReadOnlySpan<int>)arr).UpperBound(new ArrayLengthComparable(100)).ShouldBe(11);

            arr.LowerBound(new ArrayLengthComparable(101)).ShouldBe(11);
            ((IList<int>)arr).LowerBound(new ArrayLengthComparable(101)).ShouldBe(11);
            ((Span<int>)arr).LowerBound(new ArrayLengthComparable(101)).ShouldBe(11);
            ((ReadOnlySpan<int>)arr).LowerBound(new ArrayLengthComparable(101)).ShouldBe(11);

            arr.UpperBound(new ArrayLengthComparable(101)).ShouldBe(11);
            ((IList<int>)arr).UpperBound(new ArrayLengthComparable(101)).ShouldBe(11);
            ((Span<int>)arr).UpperBound(new ArrayLengthComparable(101)).ShouldBe(11);
            ((ReadOnlySpan<int>)arr).UpperBound(new ArrayLengthComparable(101)).ShouldBe(11);
        }

        struct ArrayLengthComparable : IComparable<int>
        {
            Array s;
            public ArrayLengthComparable(int length) { s = new byte[length]; }
            public ArrayLengthComparable(Array s) { this.s = s; }
            public readonly int CompareTo(int other) => s.Length.CompareTo(other);
        }

        [Fact]
        public void Func()
        {
            StlFunction.BinarySearch(-10, 20, n => n < 10).ShouldBe(9);
            StlFunction.BinarySearch(20, -10, n => n > 10).ShouldBe(11);

            StlFunction.BinarySearch(-10L, 20L, n => n < 10).ShouldBe(9L);
            StlFunction.BinarySearch(20L, -10L, n => n > 10).ShouldBe(11L);
        }
    }
}
