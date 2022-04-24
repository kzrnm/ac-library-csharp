using System;
using System.Collections.Generic;
using System.Linq;
using AtCoder.Internal;
using FluentAssertions;
using Xunit;

namespace AtCoder
{
    public class DequeTest
    {
        [Fact]
        public void Simple()
        {
            var deque = new Deque<int>();

            deque.data.Should().HaveCount(8);
            deque.Should().HaveCount(0);

            deque.Invoking(deque => deque.PopLast()).Should().Throw<InvalidOperationException>();
            deque.Invoking(deque => deque.PopFirst()).Should().Throw<InvalidOperationException>();

            for (int i = 1; i < 8; i++)
            {
                deque.AddLast(i);
                deque.Last.Should().Be(i);
                deque.data.Should().HaveCount(8);
                deque.Should().HaveCount(i);
            }
            deque.Add(-1);
            deque.data.Should().HaveCount(16);
            deque.Should().HaveCount(8);
            for (int i = deque.Count + 1; i <= 10000; i++)
            {
                deque.AddFirst(i);
                deque.First.Should().Be(i);
                deque.Last.Should().Be(-1);
                deque.data.Should().HaveCount(1 << InternalBit.CeilPow2(i + 1));
                deque.Should().HaveCount(i);
            }

            var cap = 1 << InternalBit.CeilPow2(10000);
            for (int i = deque.Count - 1; i >= 8; i--)
            {
                deque.PopFirst().Should().Be(i + 1);
                deque.data.Should().HaveCount(cap);
                deque.Should().HaveCount(i);
            }

            deque.PopLast().Should().Be(-1);

            for (int i = deque.Count - 1; i >= 0; i--)
            {
                deque.PopLast().Should().Be(i + 1);
                deque.data.Should().HaveCount(cap);
                deque.Should().HaveCount(i);
            }

            deque.Invoking(deque => deque.PopLast()).Should().Throw<InvalidOperationException>();
            deque.Invoking(deque => deque.PopFirst()).Should().Throw<InvalidOperationException>();
        }

        static int NativeSize(int capacity)
        {
            if (capacity <= 8)
                return 8;
            ++capacity;
            var v = 1;
            while (v < capacity) v <<= 1;
            return v;
        }

        public static IEnumerable<object[]> Capacity_Data = Enumerable.Range(0, 40).Select(i => new object[] { i });
        [Theory]
        [MemberData(nameof(Capacity_Data))]
        public void Capacity(int size)
        {
            var deque = new Deque<int>(size);
            deque.data.Should().HaveCount(NativeSize(size));
        }

        [Fact]
        public void Contains()
        {
            var deque = new Deque<int>();
            ((ICollection<int>)deque).Contains(0).Should().BeFalse();
            deque.AddLast(1);
            ((ICollection<int>)deque).Contains(1).Should().BeTrue();
            deque.AddLast(1);
            ((ICollection<int>)deque).Contains(1).Should().BeTrue();
            deque.PopFirst();
            ((ICollection<int>)deque).Contains(1).Should().BeTrue();
            deque.PopFirst();
            ((ICollection<int>)deque).Contains(1).Should().BeFalse();
            deque.AddFirst(2);
            ((ICollection<int>)deque).Contains(2).Should().BeTrue();
            ((ICollection<int>)deque).Contains(1).Should().BeFalse();

            for (int i = 0; i < 4; i++)
                deque.AddFirst(i + 4);

            ((ICollection<int>)deque).Contains(2).Should().BeTrue();
            ((ICollection<int>)deque).Contains(7).Should().BeTrue();
        }

        [Fact]
        public void CopyTo()
        {
            var dist = new int[8];
            dist.AsSpan().Fill(-1);
            var deque = new Deque<int>();

            deque.CopyTo(dist, 0);
            dist.Should().Equal(new[] { -1, -1, -1, -1, -1, -1, -1, -1 });

            deque.AddLast(1);
            deque.AddLast(2);
            deque.CopyTo(dist, 0);
            dist.Should().Equal(new[] { 1, 2, -1, -1, -1, -1, -1, -1 });

            deque.AddFirst(3);
            deque.AddFirst(4);
            deque.CopyTo(dist, 0);
            dist.Should().Equal(new[] { 4, 3, 1, 2, -1, -1, -1, -1 });
        }

        [Fact]
        public void Enumerate()
        {
            var deque = new Deque<int> { 1, 2, 3, 4, 5 };
            deque.Should().Equal(new[] { 1, 2, 3, 4, 5 });
            deque.Reversed().Should().Equal(new[] { 5, 4, 3, 2, 1 });
        }
    }
}
