using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace AtCoder.Internal
{
    public class SimpleListTest
    {
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
    }
}
