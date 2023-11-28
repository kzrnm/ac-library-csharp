using System;
using System.Collections.Generic;
using System.Linq;
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
                    pq.Enqueue(x);
                    list.Add(x);
                    pq.Count.Should().Be(list.Count).And.Be(i + 1);
                }
                pq.Unorderd().ToArray().Should().HaveCount(n);
                list.Sort();
                foreach (var lx in list)
                {
                    pq.Dequeue().Should().Be(lx);
                }
                pq.Count.Should().Be(0);

                list.Reverse();
                foreach (var lx in list)
                    pq.Enqueue(-lx);
                foreach (var lx in list)
                {
                    pq.TryDequeue(out var res).Should().BeTrue();
                    res.Should().Be(-lx);
                }
                pq.TryDequeue(out _).Should().BeFalse();
                pq.Count.Should().Be(0);
            }
        }
        [Fact]
        public void SimpleKV()
        {
            const int size = Global.IsCi ? 500 : 80;
            var mt = MTRandom.Create();

            for (int n = 0; n < size; n++)
            {
                var list = new List<int>();
                var pq = new PriorityQueueDictionary<long, int>();
                for (int i = 0; i < n; i++)
                {
                    var x = mt.Next(0, int.MaxValue);
                    pq.Enqueue(x, -x);
                    list.Add(x);
                    pq.Count.Should().Be(list.Count).And.Be(i + 1);
                }
                pq.UnorderdKeys().ToArray().Should().HaveCount(n);
                pq.UnorderdValues().ToArray().Should().HaveCount(n);
                list.Sort();
                foreach (var lx in list)
                {
                    pq.Dequeue().Should().Be(KeyValuePair.Create((long)lx, -lx));
                }
                pq.Count.Should().Be(0);


                list.Reverse();
                foreach (var lx in list)
                    pq.Enqueue(-lx, lx);
                foreach (var lx in list)
                {
                    pq.TryDequeue(out var res).Should().BeTrue();
                    res.Should().Be(KeyValuePair.Create(-(long)lx, lx));
                }
                pq.TryDequeue(out _).Should().BeFalse();
                pq.Count.Should().Be(0);

                foreach (var lx in list)
                    pq.Enqueue(-lx, lx);
                foreach (var lx in list)
                {
                    pq.TryDequeue(out var key, out var val).Should().BeTrue();
                    key.Should().Be(-lx);
                    val.Should().Be(lx);
                }
                pq.TryDequeue(out _, out _).Should().BeFalse();
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
                    pq.Enqueue(x);
                    list.Add(x);
                    pq.Count.Should().Be(list.Count).And.Be(i + 1);
                }
                list.Sort(ComparerUtil.ReverseComparerInt);
                foreach (var lx in list)
                {
                    pq.Dequeue().Should().Be(lx);
                }
                pq.Count.Should().Be(0);

                list.Reverse();
                foreach (var lx in list)
                    pq.Enqueue(-lx);
                foreach (var lx in list)
                {
                    pq.TryDequeue(out var res).Should().BeTrue();
                    res.Should().Be(-lx);
                }
                pq.TryDequeue(out _).Should().BeFalse();
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
                var pq = new PriorityQueueDictionary<long, int>(ComparerUtil.ReverseComparerLong);
                for (int i = 0; i < n; i++)
                {
                    var x = mt.Next(0, int.MaxValue);
                    pq.Enqueue(x, -x);
                    list.Add(x);
                    pq.Count.Should().Be(list.Count).And.Be(i + 1);
                }
                list.Sort(ComparerUtil.ReverseComparerInt);
                foreach (var lx in list)
                {
                    pq.Dequeue().Should().Be(KeyValuePair.Create((long)lx, -lx));
                }
                pq.Count.Should().Be(0);

                list.Reverse();
                foreach (var lx in list)
                    pq.Enqueue(-lx, lx);
                foreach (var lx in list)
                {
                    pq.TryDequeue(out var res).Should().BeTrue();
                    res.Should().Be(KeyValuePair.Create(-(long)lx, lx));
                }
                pq.TryDequeue(out _).Should().BeFalse();
                pq.Count.Should().Be(0);

                foreach (var lx in list)
                    pq.Enqueue(-lx, lx);
                foreach (var lx in list)
                {
                    pq.TryDequeue(out var key, out var val).Should().BeTrue();
                    key.Should().Be(-lx);
                    val.Should().Be(lx);
                }
                pq.TryDequeue(out _, out _).Should().BeFalse();
                pq.Count.Should().Be(0);
            }
        }

        [Fact]
        public void EnqueueDequeue()
        {
            var pq1 = new PriorityQueue<int>();
            var pq2 = new PriorityQueue<int>();

            pq1.EnqueueDequeue(1).Should().Be(1);

            for (int i = 10; i > 0; i--)
            {
                pq1.Enqueue(i);
                pq2.Enqueue(i);
            }
            var mt = MTRandom.Create();

            for (int i = -1; i < 20; i++)
            {
                EnqueueDequeue(i);
                EnqueueDequeue(mt.Next());
            }

            void EnqueueDequeue(int value)
            {
                pq2.Enqueue(value);
                pq1.EnqueueDequeue(value).Should().Be(pq2.Dequeue());
                pq1.Unorderd().ToArray().Should().BeEquivalentTo(pq2.Unorderd().ToArray());
            }
        }

        [Fact]
        public void EnqueueDequeueKV()
        {
            var pq1 = new PriorityQueueDictionary<int, string>();
            var pq2 = new PriorityQueueDictionary<int, string>();

            pq1.EnqueueDequeue(1, "ab").Should().Be(KeyValuePair.Create(1, "ab"));

            for (int i = 10; i > 0; i--)
            {
                pq1.Enqueue(i, i.ToString());
                pq2.Enqueue(i, i.ToString());
            }
            var mt = MTRandom.Create();

            for (int i = -1; i < 20; i++)
            {
                EnqueueDequeue(i);
                EnqueueDequeue(mt.Next());
            }

            void EnqueueDequeue(int value)
            {
                pq2.Enqueue(value, value.ToString());
                pq1.EnqueueDequeue(value, value.ToString()).Should().Be(pq2.Dequeue());
                pq1.UnorderdKeys().ToArray().Should().BeEquivalentTo(pq2.UnorderdKeys().ToArray());
                pq1.UnorderdValues().ToArray().Should().BeEquivalentTo(pq2.UnorderdValues().ToArray());
            }
        }


        [Fact]
        public void DequeueEnqueue()
        {
            var pq1 = new PriorityQueue<int>();
            var pq2 = new PriorityQueue<int>();
            for (int i = 10; i > 0; i--)
            {
                pq1.Enqueue(i);
                pq2.Enqueue(i);
            }
            var mt = MTRandom.Create();

            for (int i = -1; i < 20; i++)
            {
                DequeueEnqueue();
            }

            void DequeueEnqueue()
            {
                var value = mt.Next();
                pq1.DequeueEnqueue(value).Should().Be(pq2.Dequeue());
                pq2.Enqueue(value);
                pq1.Unorderd().ToArray().Should().BeEquivalentTo(pq2.Unorderd().ToArray());
            }
        }

        [Fact]
        public void DequeueEnqueueKV()
        {
            var pq1 = new PriorityQueueDictionary<int, string>();
            var pq2 = new PriorityQueueDictionary<int, string>();
            for (int i = 10; i > 0; i--)
            {
                pq1.Enqueue(i, i.ToString());
                pq2.Enqueue(i, i.ToString());
            }
            var mt = MTRandom.Create();

            for (int i = -1; i < 20; i++)
            {
                DequeueEnqueue();
            }

            void DequeueEnqueue()
            {
                var key = mt.Next();
                var value = mt.Next().ToString();
                pq1.DequeueEnqueue(key, value).Should().Be(pq2.Dequeue());
                pq2.Enqueue(key, value);
                pq1.UnorderdKeys().ToArray().Should().BeEquivalentTo(pq2.UnorderdKeys().ToArray());
                pq1.UnorderdValues().ToArray().Should().BeEquivalentTo(pq2.UnorderdValues().ToArray());
            }
        }
        [Fact]
        public void DequeueEnqueueFunc()
        {
            var pq1 = new PriorityQueue<int>();
            var pq2 = new PriorityQueue<int>();
            int func(int k) => k * 2;
            for (int i = 10; i > 0; i--)
            {
                pq1.Enqueue(i);
                pq2.Enqueue(i);
            }
            var mt = MTRandom.Create();

            for (int i = -1; i < 20; i++)
            {
                DequeueEnqueue();
            }

            void DequeueEnqueue()
            {
                pq2.Enqueue(func(pq2.Dequeue()));
                pq1.DequeueEnqueue(func);
                pq1.Unorderd().ToArray().Should().BeEquivalentTo(pq2.Unorderd().ToArray());
            }
        }

        [Fact]
        public void DequeueEnqueueFuncKV()
        {
            var pq1 = new PriorityQueueDictionary<int, string>();
            var pq2 = new PriorityQueueDictionary<int, string>();
            (int, string) func(int k, string v) => (k * 2, (2 * k).ToString());
            for (int i = 10; i > 0; i--)
            {
                pq1.Enqueue(i, i.ToString());
                pq2.Enqueue(i, i.ToString());
            }
            var mt = MTRandom.Create();

            for (int i = -1; i < 20; i++)
            {
                DequeueEnqueue();
            }

            void DequeueEnqueue()
            {
                var (k, v) = pq2.Dequeue();
                var (ek, ev) = func(k, v);
                pq2.Enqueue(KeyValuePair.Create(ek, ev));
                pq1.DequeueEnqueue((Func<int, string, (int, string)>)func);
                pq1.UnorderdKeys().ToArray().Should().BeEquivalentTo(pq2.UnorderdKeys().ToArray());
                pq1.UnorderdValues().ToArray().Should().BeEquivalentTo(pq2.UnorderdValues().ToArray());
            }
        }
    }
}
