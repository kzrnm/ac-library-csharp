using System;
using System.Collections.Generic;
using AtCoder.Internal;
using FluentAssertions;
using Xunit;

namespace AtCoder.Test.Internal
{
    public class InternalBitTest
    {

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(4, 2)]
        [InlineData(5, 3)]
        [InlineData(6, 3)]
        [InlineData(7, 3)]
        [InlineData(8, 3)]
        [InlineData(9, 4)]
        [InlineData(1 << 30, 30)]
        [InlineData((1 << 30) + 1, 31)]
        [InlineData(int.MaxValue, 31)]
        public void CeilPow2Test(int input, int expected)
        {
            InternalMath.CeilPow2(input).Should().Be(expected);
        }

        [Theory]
        [InlineData(1U, 0)]
        [InlineData(2U, 1)]
        [InlineData(3U, 0)]
        [InlineData(4U, 2)]
        [InlineData(5U, 0)]
        [InlineData(6U, 1)]
        [InlineData(7U, 0)]
        [InlineData(8U, 3)]
        [InlineData(9U, 0)]
        [InlineData(1U << 30, 30)]
        [InlineData((1U << 31) - 1, 0)]
        [InlineData(1U << 31, 31)]
        [InlineData(uint.MaxValue, 0)]
        public void BSFTest(uint input, int expected)
        {
            InternalBit.BSF(input).Should().Be(expected);
        }
    }
}
