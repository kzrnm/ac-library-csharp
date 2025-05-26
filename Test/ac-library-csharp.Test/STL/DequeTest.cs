using System;
using System.Collections.Generic;
using System.Linq;
using AtCoder.Internal;
using MersenneTwister;
using Shouldly;
using Xunit;

namespace AtCoder
{
    public class DequeTest
    {
        [Fact]
        public void Empty()
        {
            Impl([]);
            for (int capacity = 0; capacity < 10; capacity++)
            {
                Impl(new Deque<int>(capacity));

                var deque = new Deque<int>(capacity);
                deque.AddFirst(0);
                deque.PopLast();
                Impl(new Deque<int>(capacity));
            }

            static void Impl(Deque<int> deque)
            {
                deque.GetEnumerator().MoveNext().ShouldBeFalse();
                deque.Count.ShouldBe(0);
                deque.ShouldBeEmpty();
                deque.ShouldBe([]);
            }
        }

        [Fact]
        public void Lengths()
        {
            for (int size = 1; size < 10; size++)
            {
                var orig = Enumerable.Range(1, size).ToArray();
                var deque = new Deque<int>();
                foreach (var num in orig)
                    deque.AddLast(num);
                deque.Count.ShouldBe(size);
                deque.ShouldBe(orig);
            }
        }

        [Fact]
        public void Random()
        {
            var mt = MTRandom.Create();
            var deque = new Deque<int>();
            var list = new LinkedList<int>();

            void AddFirst(int num)
            {
                deque.AddFirst(num);
                list.AddFirst(num);
                deque.ShouldBe(list);
            }

            void AddLast(int num)
            {
                deque.AddLast(num);
                list.AddLast(num);
                deque.ShouldBe(list);
            }
            void PopFirst()
            {
                deque.PopFirst();
                list.RemoveFirst();
                deque.ShouldBe(list);
            }

            void PopLast()
            {
                deque.PopLast();
                list.RemoveLast();
                deque.ShouldBe(list);
            }


            for (int q = 0; q < 10000; q++)
            {
                var type = mt.Next(4);
                if (deque.Count == 0) type %= 2;

                switch (type)
                {
                    case 0: AddFirst(mt.Next()); break;
                    case 1: AddLast(mt.Next()); break;
                    case 2: PopFirst(); break;
                    case 3: PopLast(); break;
                }
            }

            for (int q = 0; q < 10000; q++)
            {
                var type = mt.Next(6);
                if (deque.Count == 0) type %= 2;
                switch (type)
                {
                    case 0: AddFirst(mt.Next()); break;
                    case 1: AddLast(mt.Next()); break;
                    case 2: case 4: PopFirst(); break;
                    case 3: case 5: PopLast(); break;
                }
            }
        }

        [Fact]
        public void Simple()
        {
            var deque = new Deque<int>();

            deque.data.Length.ShouldBe(2);
            deque.Count.ShouldBe(0);

            new Action(() => deque.PopLast()).ShouldThrow<InvalidOperationException>();
            new Action(() => deque.PopFirst()).ShouldThrow<InvalidOperationException>();

            for (int i = 1; i < 8; i++)
            {
                deque.AddLast(i);
                deque.Last.ShouldBe(i);
                deque.data.Length.ShouldBe(NativeSize(i));
                deque.Count.ShouldBe(i);
            }
            deque.Add(-1);
            deque.data.Length.ShouldBe(16);
            deque.Count.ShouldBe(8);
            for (int i = deque.Count + 1; i <= 10000; i++)
            {
                deque.AddFirst(i);
                deque.First.ShouldBe(i);
                deque.Last.ShouldBe(-1);
                deque.data.Length.ShouldBe(NativeSize(i));
                deque.Count.ShouldBe(i);
            }

            var cap = 1 << InternalBit.CeilPow2(10000);
            for (int i = deque.Count - 1; i >= 8; i--)
            {
                deque.PopFirst().ShouldBe(i + 1);
                deque.data.Length.ShouldBe(cap);
                deque.Count.ShouldBe(i);
            }

            deque.PopLast().ShouldBe(-1);

            for (int i = deque.Count - 1; i >= 0; i--)
            {
                deque.PopLast().ShouldBe(i + 1);
                deque.data.Length.ShouldBe(cap);
                deque.Count.ShouldBe(i);
            }

            new Action(() => deque.PopLast()).ShouldThrow<InvalidOperationException>();
            new Action(() => deque.PopFirst()).ShouldThrow<InvalidOperationException>();
        }

        static int NativeSize(int capacity)
        {
            ++capacity;
            var v = 1;
            while (v < capacity) v <<= 1;
            return v;
        }

        public static IEnumerable<object[]> Capacity_Data => Enumerable.Range(0, 40).Select(i => new object[] { i });
        [Theory]
        [MemberData(nameof(Capacity_Data))]
        public void Capacity(int size)
        {
            var deque = new Deque<int>(size);
            deque.data.Length.ShouldBe(NativeSize(size));
        }

        [Fact]
        public void Contains()
        {
            var deque = new Deque<int>();
            ((ICollection<int>)deque).Contains(0).ShouldBeFalse();
            deque.AddLast(1);
            ((ICollection<int>)deque).Contains(1).ShouldBeTrue();
            deque.AddLast(1);
            ((ICollection<int>)deque).Contains(1).ShouldBeTrue();
            deque.PopFirst();
            ((ICollection<int>)deque).Contains(1).ShouldBeTrue();
            deque.PopFirst();
            ((ICollection<int>)deque).Contains(1).ShouldBeFalse();
            deque.AddFirst(2);
            ((ICollection<int>)deque).Contains(2).ShouldBeTrue();
            ((ICollection<int>)deque).Contains(1).ShouldBeFalse();

            for (int i = 0; i < 4; i++)
                deque.AddFirst(i + 4);

            ((ICollection<int>)deque).Contains(2).ShouldBeTrue();
            ((ICollection<int>)deque).Contains(7).ShouldBeTrue();
        }

        [Fact]
        public void CopyTo()
        {
            var dist = new int[8];
            dist.AsSpan().Fill(-1);
            var deque = new Deque<int>();

            deque.CopyTo(dist, 0);
            dist.ShouldBe([-1, -1, -1, -1, -1, -1, -1, -1]);

            deque.AddLast(1);
            deque.AddLast(2);
            deque.CopyTo(dist, 0);
            dist.ShouldBe([1, 2, -1, -1, -1, -1, -1, -1]);

            deque.AddFirst(3);
            deque.AddFirst(4);
            deque.CopyTo(dist, 0);
            dist.ShouldBe([4, 3, 1, 2, -1, -1, -1, -1]);
        }

        [Fact]
        public void Enumerate()
        {
            var deque = new Deque<int> { 1, 2, 3, 4, 5 };
            deque.ShouldBe([1, 2, 3, 4, 5]);
            deque.Reversed().ShouldBe([5, 4, 3, 2, 1]);
        }

        [Fact]
        public void Grow()
        {
            {
                var deque = new Deque<int> { 1, };
                deque.ShouldBe([1,]);
                deque.Grow(6);
                deque.data.Length.ShouldBe(8);
                deque.ShouldBe([1,]);
            }
            {
                var deque = new Deque<int> { 1, 2, 3, };
                deque.ShouldBe([1, 2, 3,]);
                deque.Grow(6);
                deque.data.Length.ShouldBe(8);
                deque.ShouldBe([1, 2, 3,]);
            }
            {
                var deque = new Deque<int> { 1, 2, 3, };
                deque.ShouldBe([1, 2, 3,]);
                deque.PopFirst();
                deque.Grow(6);
                deque.data.Length.ShouldBe(8);
                deque.ShouldBe([2, 3,]);
            }
            {
                var deque = new Deque<int> { 1, 2, 3, };
                deque.ShouldBe([1, 2, 3,]);
                deque.PopFirst();
                deque.PopFirst();
                deque.AddLast(-1);
                deque.AddLast(-2);
                deque.PopFirst();
                deque.Grow(6);
                deque.data.Length.ShouldBe(8);
                deque.ShouldBe([-1, -2,]);
            }
            {
                var deque = new Deque<int> { 1, 2, 3, };
                deque.ShouldBe([1, 2, 3,]);
                deque.PopFirst();
                deque.PopFirst();
                deque.PopFirst();
                deque.ShouldBeEmpty();
                deque.Grow(6);
                deque.data.Length.ShouldBe(8);
                deque.ShouldBeEmpty();
            }
            {
                var deque = new Deque<int> { 1, 2, 3, };
                deque.ShouldBe([1, 2, 3,]);
                deque.PopLast();
                deque.PopFirst();
                deque.PopFirst();
                deque.ShouldBeEmpty();
                deque.Grow(6);
                deque.data.Length.ShouldBe(8);
                deque.ShouldBeEmpty();
            }
        }
    }
}
