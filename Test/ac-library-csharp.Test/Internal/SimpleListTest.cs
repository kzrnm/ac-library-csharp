using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Shouldly;
using Xunit;

namespace AtCoder.Internal
{
    public class SimpleListTest
    {
        [Fact]
        public void ConstructorCollection()
        {
            SimpleList<int> list = new([1, 2, 3, 4, 5, 6]);
            list.ShouldBe([1, 2, 3, 4, 5, 6]);
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
            SimpleList<int> list = new(Nums());
            list.ShouldBe([1, 2, 3, 4, 5, 6]);
        }

        [Fact]
        public void AddAndRemove()
        {
            SimpleList<int> list = [1, 2, 3, 4, 5, 6];
            list.ShouldBe([1, 2, 3, 4, 5, 6]);
            list.RemoveLast();
            list.ShouldBe([1, 2, 3, 4, 5]);
            list.Add(7);
            list.ShouldBe([1, 2, 3, 4, 5, 7]);
        }

        [Fact]
        public void RemoveLastSize()
        {
            SimpleList<int> list = [1, 2, 3, 4, 5, 6];
            list.ShouldBe([1, 2, 3, 4, 5, 6]);
            list.RemoveLast(2);
            list.ShouldBe([1, 2, 3, 4]);
            list.RemoveLast(4);
            list.ShouldBeEmpty();
        }

        [Fact]
        public void Reverse()
        {
            SimpleList<int> list = [1, 2, 3, 4, 5, 6];
            list.Reverse();
            list.ShouldBe([6, 5, 4, 3, 2, 1]);
            list.Reverse(1, 5);
            list.ShouldBe([6, 1, 2, 3, 4, 5]);
            list.RemoveLast();
            list.Reverse();
            list.ShouldBe([4, 3, 2, 1, 6]);
        }


        [Fact]
        public void Sort()
        {
            SimpleList<int> list = [2, 3, 4, 1, 5, 6];
            list.Sort();
            list.ShouldBe([1, 2, 3, 4, 5, 6]);

            list = [2, 3, 4, 1, 5, 6];
            list.Sort(Comparer<int>.Create((a, b) => b.CompareTo(a)));
            list.ShouldBe([6, 5, 4, 3, 2, 1]);

            list = [2, 3, 4, 1, 5, 6];
            list.Sort(1, 3, Comparer<int>.Create((a, b) => b.CompareTo(a)));
            list.ShouldBe([2, 4, 3, 1, 5, 6]);
        }

        [Fact]
        public void MemoryAndSpan()
        {
            SimpleList<int> list = [2, 3, 4, 1, 5, 6];
            list.AsSpan().ToArray().ShouldBe([2, 3, 4, 1, 5, 6]);
            list.AsMemory().ToArray().ShouldBe([2, 3, 4, 1, 5, 6]);
            MemoryMarshal.TryGetArray<int>(list.AsMemory(), out var arraySegment)
                .ShouldBeTrue();
            arraySegment.Array.Take(6).ShouldBe([2, 3, 4, 1, 5, 6]);
        }
    }
}
