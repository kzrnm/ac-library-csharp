using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AtCoder
{
    public class FenwickTreeTest
    {
        private readonly struct ModID0 : IDynamicModID { }
        private readonly struct ModID1 : IDynamicModID { }
        private readonly struct ModID2 : IDynamicModID { }
        private struct Mod11 : IStaticMod
        {
            public uint Mod => 11;
            public bool IsPrime => true;
        }

        [Fact]
        public void Zero()
        {
            var fwLong = new LongFenwickTree(0);
            fwLong.Sum(0, 0).Should().Be(0L);
            fwLong[0..0].Should().Be(0L);

            default(ModID0).SetMod(2);
            var fwMod = new DynamicModIntFenwickTree<ModID0>(0);
            fwMod.Sum(0, 0)
                .Should()
                .Be(fwMod[0..0])
                .And
                .Be(new DynamicModInt<ModID0>(0));
        }

        [Fact]
        public void OverFlowULong()
        {
            var fw = new ULongFenwickTree(10);

            for (int i = 0; i < 10; i++)
            {
                fw.Add(i, (1UL << 63) + (uint)i);
            }
            for (uint i = 0; i <= 10; i++)
            {
                for (uint j = i; j <= 10; j++)
                {
                    ulong sum = 0;
                    for (uint k = i; k < j; k++)
                    {
                        sum += k;
                    }
                    fw.Sum((int)i, (int)j)
                        .Should()
                        .Be(fw[(int)i..(int)j])
                        .And
                        .Be(((j - i) % 2) != 0 ? (1UL << 63) + sum : sum);
                }
            }
        }

        [Fact]
        public void NaiveTest()
        {
            for (int n = 0; n <= 50; n++)
            {
                var fw = new LongFenwickTree(n);
                for (int i = 0; i < n; i++)
                {
                    fw.Add(i, i * i);
                }
                for (int l = 0; l <= n; l++)
                {
                    for (int r = l; r <= n; r++)
                    {
                        long sum = 0;
                        for (int i = l; i < r; i++)
                        {
                            sum += i * i;
                        }
                        fw.Sum(l, r).Should().Be(fw[l..r]).And.Be(sum);
                    }
                }
            }
        }

        [Fact]
        public void SMintTest()
        {
            for (int n = 0; n <= 50; n++)
            {
                var fw = new StaticModIntFenwickTree<Mod11>(n);
                for (int i = 0; i < n; i++)
                {
                    fw.Add(i, i * i);
                }
                for (int l = 0; l <= n; l++)
                {
                    for (int r = l; r <= n; r++)
                    {
                        StaticModInt<Mod11> sum = 0;
                        for (int i = l; i < r; i++)
                        {
                            sum += i * i;
                        }
                        fw.Sum(l, r).Should().Be(fw[l..r]).And.Be(sum);
                    }
                }
            }
        }

        [Fact]
        public void MintTest()
        {
            default(ModID1).SetMod(11);
            for (int n = 0; n <= 50; n++)
            {
                var fw = new DynamicModIntFenwickTree<ModID1>(n);
                for (int i = 0; i < n; i++)
                {
                    fw.Add(i, i * i);
                }
                for (int l = 0; l <= n; l++)
                {
                    for (int r = l; r <= n; r++)
                    {
                        DynamicModInt<ModID1> sum = 0;
                        for (int i = l; i < r; i++)
                        {
                            sum += i * i;
                        }
                        fw.Sum(l, r).Should().Be(fw[l..r]).And.Be(sum);
                    }
                }
            }
        }

        [Fact]
        public void Invalid()
        {
            var s = new IntFenwickTree(10);
            s.Invoking(s => s.Add(-1, 0)).Should().ThrowContractAssert();
            s.Invoking(s => s.Add(10, 0)).Should().ThrowContractAssert();
            s.Invoking(s => s.Add(0, 0)).Should().NotThrow();
            s.Invoking(s => s.Add(9, 0)).Should().NotThrow();

            s.Invoking(s => s.Sum(-1, 3)).Should().ThrowContractAssert();
            s.Invoking(s => s.Sum(3, 11)).Should().ThrowContractAssert();
            s.Invoking(s => s.Sum(5, 3)).Should().ThrowContractAssert();
            s.Invoking(s => s.Sum(0, 10)).Should().NotThrow();
            s.Invoking(s => s.Sum(10, 10)).Should().NotThrow();

            s.Invoking(s => s[-1..3]).Should().ThrowContractAssert();
            s.Invoking(s => s[3..11]).Should().ThrowContractAssert();
            s.Invoking(s => s[5..3]).Should().ThrowContractAssert();
            s.Invoking(s => s[0..10]).Should().NotThrow();
            s.Invoking(s => s[10..10]).Should().NotThrow();
        }

        [Fact]
        public void Bound()
        {
            var fw = new IntFenwickTree(10);
            fw.Add(3, int.MaxValue);
            fw.Add(5, int.MinValue);

            fw.Sum(0, 10).Should().Be(fw[0..10]).And.Be(-1);
            fw.Sum(3, 6).Should().Be(fw[3..6]).And.Be(-1);
            fw.Sum(3, 4).Should().Be(fw[3..4]).And.Be(int.MaxValue);
            fw.Sum(4, 10).Should().Be(fw[4..10]).And.Be(int.MinValue);
        }

        [Fact]
        public void BoundLong()
        {
            var fw = new LongFenwickTree(10);
            fw.Add(3, long.MaxValue);
            fw.Add(5, long.MinValue);

            fw.Sum(0, 10).Should().Be(fw[0..10]).And.Be(-1);
            fw.Sum(3, 6).Should().Be(fw[3..6]).And.Be(-1);
            fw.Sum(3, 4).Should().Be(fw[3..4]).And.Be(long.MaxValue);
            fw.Sum(4, 10).Should().Be(fw[4..10]).And.Be(long.MinValue);
        }

        [Fact]
        public void OverFlow()
        {
            var fw = new IntFenwickTree(20);
            var a = new long[20];

            for (int i = 0; i < 10; i++)
            {
                int x = int.MaxValue;
                a[i] += x;
                fw.Add(i, x);
            }
            for (int i = 10; i < 20; i++)
            {
                int x = int.MinValue;
                a[i] += x;
                fw.Add(i, x);
            }
            a[5] += 11111;
            fw.Add(5, 11111);

            for (int l = 0; l <= 20; l++)
            {
                for (int r = l; r <= 20; r++)
                {
                    long sum = 0;
                    for (int i = l; i < r; i++)
                    {
                        sum += a[i];
                    }
                    long dif = sum - fw.Sum(l, r);
                    (dif % (1L << 32)).Should().Be(0);
                }
            }
        }

        [Theory]
        [Trait("Category", "Practice")]
        [InlineData(
        // Based on Library Checker https://github.com/yosupo06/library-checker-problems
        // Point Add Range Sum
        // Apache License 2.0
        @"5 5
1 2 3 4 5
1 0 5
1 2 4
0 3 10
1 0 5
1 0 3
",

        @"15
7
25
6
"
        )]
        public void Practice(string input, string expected)
            => new SolverRunner(Solver).Solve(input).Should().EqualLines(expected);
        static void Solver(TextReader reader, TextWriter writer)
        {
            int[] nq = reader.ReadLine().Split().Select(int.Parse).ToArray();
            (int n, int q) = (nq[0], nq[1]);

            var fw = new LongFenwickTree(n);
            int[] a = reader.ReadLine().Split().Select(int.Parse).ToArray();
            for (int i = 0; i < a.Length; i++)
            {
                fw.Add(i, a[i]);
            }
            for (int i = 0; i < q; i++)
            {
                int[] query = reader.ReadLine().Split().Select(int.Parse).ToArray();
                int t = query[0];
                if (t == 0)
                {
                    (int p, int x) = (query[1], query[2]);
                    fw.Add(p, x);
                }
                else
                {
                    (int l, int r) = (query[1], query[2]);
                    writer.WriteLine(fw.Sum(l, r));
                }
            }
        }
    }
}
