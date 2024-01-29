using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MersenneTwister;
using Xunit;

namespace AtCoder
{
    public static class ComparerUtil
    {
        public static IComparer<int> ReverseComparerInt { get; } = Comparer<int>.Create((a, b) => b.CompareTo(a));
        public static IComparer<long> ReverseComparerLong { get; } = Comparer<long>.Create((a, b) => b.CompareTo(a));
    }
    public class ComparerUtilTest
    {
        [Fact]
        public void UtilReverseTest()
        {
            var mt = MTRandom.Create();
            for (int n = 0; n < 200; n++)
            {
                var arr = new int[n];
                for (int i = 0; i < n; i++)
                {
                    arr[i] = mt.Next();
                }
                var cpp = arr.ToArray();
                Array.Sort(arr);
                Array.Reverse(arr);
                Array.Sort(cpp, ComparerUtil.ReverseComparerInt);
                cpp.Should().Equal(cpp);
            }
            for (int n = 0; n < 200; n++)
            {
                var arr = new long[n];
                for (int i = 0; i < n; i++)
                {
                    arr[i] = mt.Next();
                }
                var cpp = arr.ToArray();
                Array.Sort(arr);
                Array.Reverse(arr);
                Array.Sort(cpp, ComparerUtil.ReverseComparerLong);
                cpp.Should().Equal(cpp);
            }
        }
    }
}
