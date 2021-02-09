using System;
using System.Collections.Generic;
using System.Linq;
using AtCoder.Utils;
using FluentAssertions;
using MersenneTwister;
using Xunit;

namespace AtCoder
{
    public class PriorityQueueTest
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


        [Fact]
        public void Simple()
        {
            var mt = MTRandom.Create();

            for (int n = 0; n < 200; n++)
            {
                var list = new List<int>();
                var pq = new PriorityQueue<int>();
                for (int i = 0; i < n; i++)
                {
                    var x = mt.Next();
                    pq.Add(x);
                    list.Add(x);
                    pq.Count.Should().Be(list.Count).And.Be(i + 1);
                }
                list.Sort();
                int count = list.Count;
                foreach (var lx in list)
                {
                    pq.Count.Should().Be(count--);
                    pq.Peek.Should().Be(lx);
                    pq.Dequeue().Should().Be(lx);
                }
                pq.Count.Should().Be(0);
            }
        }
        [Fact]
        public void SimpleKV()
        {
            var mt = MTRandom.Create();

            for (int n = 0; n < 200; n++)
            {
                var list = new List<int>();
                var pq = new PriorityQueue<long, int>();
                for (int i = 0; i < n; i++)
                {
                    var x = mt.Next(0, int.MaxValue);
                    pq.Add(x, -x);
                    list.Add(x);
                    pq.Count.Should().Be(list.Count).And.Be(i + 1);
                }
                list.Sort();
                int count = list.Count;
                foreach (var lx in list)
                {
                    pq.Count.Should().Be(count--);
                    pq.Peek.Should().Be(KeyValuePair.Create((long)lx, -lx));
                    pq.Dequeue().Should().Be(KeyValuePair.Create((long)lx, -lx));
                }
                pq.Count.Should().Be(0);
            }
        }

        [Fact]
        public void Comparer()
        {
            var mt = MTRandom.Create();

            for (int n = 0; n < 200; n++)
            {
                var list = new List<int>();
                var pq = new PriorityQueue<int>(ComparerUtil.ReverseComparerInt);
                for (int i = 0; i < n; i++)
                {
                    var x = mt.Next();
                    pq.Add(x);
                    list.Add(x);
                    pq.Count.Should().Be(list.Count).And.Be(i + 1);
                }
                list.Sort(ComparerUtil.ReverseComparerInt);
                int count = list.Count;
                foreach (var lx in list)
                {
                    pq.Count.Should().Be(count--);
                    pq.Peek.Should().Be(lx);
                    pq.Dequeue().Should().Be(lx);
                }
                pq.Count.Should().Be(0);
            }
        }
        [Fact]
        public void ComparerKV()
        {
            var mt = MTRandom.Create();

            for (int n = 0; n < 200; n++)
            {
                var list = new List<int>();
                var pq = new PriorityQueue<long, int>(ComparerUtil.ReverseComparerLong);
                for (int i = 0; i < n; i++)
                {
                    var x = mt.Next(0, int.MaxValue);
                    pq.Add(x, -x);
                    list.Add(x);
                    pq.Count.Should().Be(list.Count).And.Be(i + 1);
                }
                list.Sort(ComparerUtil.ReverseComparerInt);
                int count = list.Count;
                foreach (var lx in list)
                {
                    pq.Count.Should().Be(count--);
                    pq.Peek.Should().Be(KeyValuePair.Create((long)lx, -lx));
                    pq.Dequeue().Should().Be(KeyValuePair.Create((long)lx, -lx));
                }
                pq.Count.Should().Be(0);
            }
        }
    }
}
