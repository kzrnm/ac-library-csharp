using System.Collections.Generic;
using System.Runtime.InteropServices;
using FluentAssertions;
using Xunit;

namespace AtCoder.Internal
{
    public class SimpleListTest
    {
        [Fact]
        public void ConstructorCollection()
        {
            var list = new SimpleList<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            list.Should().HaveCount(6);
            list.Should().Equal(1, 2, 3, 4, 5, 6);
        }

        [Fact]
        public void ConstructorEnumerable()
        {
            IEnumerable<int> Nums()
            {
                yield return 1;
                yield return 2;
                yield return 3;
                yield return 4;
                yield return 5;
                yield return 6;
            }
            var list = new SimpleList<int>(Nums());
            list.Should().HaveCount(6);
            list.Should().Equal(1, 2, 3, 4, 5, 6);
        }

        [Fact]
        public void AddAndRemove()
        {
            var list = new SimpleList<int>
            {
                1,2,3,4,5,6
            };
            list.Should().Equal(1, 2, 3, 4, 5, 6);
            list.RemoveLast();
            list.Should().Equal(1, 2, 3, 4, 5);
            list.Add(7);
            list.Should().Equal(1, 2, 3, 4, 5, 7);
        }

        [Fact]
        public void RemoveLastSize()
        {
            var list = new SimpleList<int>
            {
                1,2,3,4,5,6
            };
            list.Should().Equal(1, 2, 3, 4, 5, 6);
            list.RemoveLast(2);
            list.Should().Equal(1, 2, 3, 4);
            list.RemoveLast(4);
            list.Should().BeEmpty();
        }

        [Fact]
        public void Reverse()
        {
            var list = new SimpleList<int>
            {
                1,2,3,4,5,6
            };
            list.Reverse();
            list.Should().Equal(6, 5, 4, 3, 2, 1);
            list.Reverse(1, 5);
            list.Should().Equal(6, 1, 2, 3, 4, 5);
            list.RemoveLast();
            list.Reverse();
            list.Should().Equal(4, 3, 2, 1, 6);
        }


        [Fact]
        public void Sort()
        {
            var list = new SimpleList<int>
            {
                2,3,4,1,5,6
            };
            list.Sort();
            list.Should().Equal(1, 2, 3, 4, 5, 6);

            list = new SimpleList<int>
            {
                2,3,4,1,5,6
            };
            list.Sort(Comparer<int>.Create((a, b) => b.CompareTo(a)));
            list.Should().Equal(6, 5, 4, 3, 2, 1);

            list = new SimpleList<int>
            {
                2,3,4,1,5,6
            };
            list.Sort(1, 3, Comparer<int>.Create((a, b) => b.CompareTo(a)));
            list.Should().Equal(2, 4, 3, 1, 5, 6);
        }

        [Fact]
        public void MemoryAndSpan()
        {
            var list = new SimpleList<int>
            {
                2,3,4,1,5,6
            };
            list.AsSpan().ToArray().Should().Equal(2, 3, 4, 1, 5, 6);
            list.AsMemory().ToArray().Should().Equal(2, 3, 4, 1, 5, 6);
            MemoryMarshal.TryGetArray<int>(list.AsMemory(), out var arraySegment)
                .Should().BeTrue();
            arraySegment.Array.Should().StartWith(new[] { 2, 3, 4, 1, 5, 6 });
        }
    }
}
