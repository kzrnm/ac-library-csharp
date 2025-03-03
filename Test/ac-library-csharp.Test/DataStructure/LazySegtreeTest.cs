﻿using System;
using System.IO;
using System.Linq;
using AtCoder.Internal;
using Shouldly;
using Xunit;

namespace AtCoder
{
    public class LazySegtreeTest
    {
        private readonly struct Starry : ILazySegtreeOperator<int, int>
        {
            public int Identity => -1_000_000_000;
            public int FIdentity => 0;

            public int Composition(int f, int g) => f + g;

            public int Mapping(int f, int x) => f + x;

            public int Operate(int x, int y) => Math.Max(x, y);
        }

        [Fact]
        public void Zero()
        {
            new LazySegtree<int, int, Starry>(0).AllProd.ShouldBe(-1_000_000_000);
            new LazySegtree<int, int, Starry>(10).AllProd.ShouldBe(-1_000_000_000);
        }

        [Fact]
        public void Invalid()
        {
            var s = new LazySegtree<int, int, Starry>(10);
            new Action(() => _ = s[-1]).ShouldThrow<ContractAssertException>();
            new Action(() => _ = s[10]).ShouldThrow<ContractAssertException>();
            new Action(() => _ = s[0]).ShouldNotThrow();
            new Action(() => _ = s[9]).ShouldNotThrow();

            new Action(() => _ = s.Prod(-1, -1)).ShouldThrow<ContractAssertException>();
            new Action(() => _ = s.Prod(3, 2)).ShouldThrow<ContractAssertException>();
            new Action(() => _ = s.Prod(0, 11)).ShouldThrow<ContractAssertException>();
            new Action(() => _ = s.Prod(-1, 11)).ShouldThrow<ContractAssertException>();
            new Action(() => _ = s.Prod(0, 0)).ShouldNotThrow();
            new Action(() => _ = s.Prod(10, 10)).ShouldNotThrow();
            new Action(() => _ = s.Prod(0, 10)).ShouldNotThrow();

            new Action(() => _ = s.MaxRight(11, s => true)).ShouldThrow<ContractAssertException>();
            new Action(() => _ = s.MaxRight(-1, s => true)).ShouldThrow<ContractAssertException>();
            new Action(() => _ = s.MaxRight(0, s => false)).ShouldThrow<ContractAssertException>();
            new Action(() => _ = s.MaxRight(0, s => true)).ShouldNotThrow();
            new Action(() => _ = s.MaxRight(10, s => true)).ShouldNotThrow();

            new Action(() => _ = s.MinLeft(11, s => true)).ShouldThrow<ContractAssertException>();
            new Action(() => _ = s.MinLeft(-1, s => true)).ShouldThrow<ContractAssertException>();
            new Action(() => _ = s.MinLeft(0, s => false)).ShouldThrow<ContractAssertException>();
            new Action(() => _ = s.MinLeft(0, s => true)).ShouldNotThrow();
            new Action(() => _ = s.MinLeft(10, s => true)).ShouldNotThrow();
        }

        [Fact]
        public void NaiveProd()
        {
            for (int n = 0; n <= 50; n++)
            {
                var seg = new LazySegtree<int, int, Starry>(n);
                var p = new int[n];
                for (int i = 0; i < n; i++)
                {
                    p[i] = (i * i + 100) % 31;
                    seg[i] = p[i];
                }
                for (int l = 0; l <= n; l++)
                {
                    for (int r = l; r <= n; r++)
                    {
                        int e = -1_000_000_000;
                        for (int i = l; i < r; i++)
                        {
                            e = Math.Max(e, p[i]);
                        }
                        seg.Prod(l, r).ShouldBe(e);
                        seg[l..r].ShouldBe(e);
                    }
                }
            }
        }

        [Fact]
        public void Usage()
        {
            var seg = new LazySegtree<int, int, Starry>(new int[10]);
            seg.AllProd.ShouldBe(0);
            seg.Apply(0, 3, 5);
            seg.AllProd.ShouldBe(5);
            seg.Apply(2, -10);
            seg.Prod(2, 3).ShouldBe(-5);
            seg[2..3].ShouldBe(-5);
            seg.Prod(2, 4).ShouldBe(0);
            seg[2..4].ShouldBe(0);
        }

        [Theory]
        [Trait("Category", "Practice")]
        [InlineData(
// Based on Library Checker https://github.com/yosupo06/library-checker-problems
// Range Affine Range Sum
// Apache License 2.0
@"5 7
1 2 3 4 5
1 0 5
0 2 4 100 101
1 0 3
0 1 3 102 103
1 2 5
0 2 5 104 105
1 0 5
",

@"15
404
41511
4317767
"
)]
        public void PracticeK(string input, string expected)
            => new SolverRunner(SolverK).Solve(input).ShouldEqualLines(expected);
        static void SolverK(TextReader reader, TextWriter writer)
        {
            int[] nq = reader.ReadLine().Split().Select(int.Parse).ToArray();
            (int n, int q) = (nq[0], nq[1]);

            Sk[] a = reader.ReadLine().Split().Select(x => new Sk() { A = int.Parse(x), Size = 1 }).ToArray();

            LazySegtree<Sk, Fk, Operatork> seg = new(a);

            for (int i = 0; i < q; i++)
            {
                int[] query = reader.ReadLine().Split().Select(int.Parse).ToArray();
                int t = query[0];
                if (t == 0)
                {
                    (int l, int r, int c, int d) = (query[1], query[2], query[3], query[4]);
                    seg.Apply(l, r, new() { A = c, B = d });
                }
                else
                {
                    (int l, int r) = (query[1], query[2]);
                    writer.WriteLine(seg.Prod(l, r).A);
                }
            }
        }
        struct Sk
        {
            public StaticModInt<Mod998244353> A;
            public int Size;
        }

        struct Fk
        {
            public StaticModInt<Mod998244353> A, B;
        }

        readonly struct Operatork : ILazySegtreeOperator<Sk, Fk>
        {
            public Sk Identity => default;
            public Fk FIdentity => new() { A = 1 };

            public Fk Composition(Fk l, Fk r)
                => new() { A = l.A * r.A, B = l.A * r.B + l.B };

            public Sk Mapping(Fk l, Sk r)
                => new() { A = r.A * l.A + r.Size * l.B, Size = r.Size };

            public Sk Operate(Sk x, Sk y)
                => new() { A = x.A + y.A, Size = x.Size + y.Size };
        }

        [Theory]
        [Trait("Category", "Practice")]
        [InlineData(
// Based on https://atcoder.jp/contests/practice2/tasks/practice2_l
@"5 5
0 1 0 0 1
2 1 5
1 3 4
2 2 5
1 1 3
2 1 2
",

@"2
0
1
"
)]
        public void PracticeL(string input, string expected)
            => new SolverRunner(SolverL).Solve(input).ShouldEqualLines(expected);
        static void SolverL(TextReader reader, TextWriter writer)
        {
            int[] nq = reader.ReadLine().Split().Select(int.Parse).ToArray();
            (int n, int q) = (nq[0], nq[1]);

            Sl[] a = reader.ReadLine().Split().Select(int.Parse)
                .Select(x => new Sl() { Zero = x ^ 1, One = x, Inversion = 0 }).ToArray();

            LazySegtree<Sl, bool, Operatorl> seg = new(a);

            for (int i = 0; i < q; i++)
            {
                int[] query = reader.ReadLine().Split().Select(int.Parse).ToArray();
                int t = query[0];
                int l = query[1];
                int r = query[2];
                l--;
                if (t == 1)
                {
                    seg.Apply(l, r, true);
                }
                else
                {
                    writer.WriteLine(seg.Prod(l, r).Inversion);
                }
            }
        }
        struct Sl
        {
            public long Zero;
            public long One;
            public long Inversion;
        }

        readonly struct Operatorl : ILazySegtreeOperator<Sl, bool>
        {
            public Sl Identity => default;
            public bool FIdentity => false;

            public bool Composition(bool l, bool r) => (l && !r) || (!l && r);

            public Sl Mapping(bool l, Sl r)
            {
                if (!l) return r;
                // swap
                return new() { Zero = r.One, One = r.Zero, Inversion = r.One * r.Zero - r.Inversion };
            }

            public Sl Operate(Sl l, Sl r) => new()
            {
                Zero = l.Zero + r.Zero,
                One = l.One + r.One,
                Inversion = l.Inversion + r.Inversion + l.One * r.Zero,
            };
        }
    }
}
