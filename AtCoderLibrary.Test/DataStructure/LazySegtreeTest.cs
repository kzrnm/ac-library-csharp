using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using AtCoder.Test.Utils;
using FluentAssertions;
using Xunit;

namespace AtCoder.Test.DataStructure
{
    public class LazySegtreeTest : TestWithDebugAssert
    {


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
            => new SolverRunner(SolverK).Solve(input).Should().EqualLines(expected);
        static void SolverK(TextReader reader, TextWriter writer)
        {
            int[] nq = reader.ReadLine().Split().Select(int.Parse).ToArray();
            (int n, int q) = (nq[0], nq[1]);

            Sk[] a = reader.ReadLine().Split().Select(x => new Sk() { A = int.Parse(x), Size = 1 }).ToArray();

            LazySegtree<Sk, Fk, Operatork> seg = new LazySegtree<Sk, Fk, Operatork>(a);

            for (int i = 0; i < q; i++)
            {
                int[] query = reader.ReadLine().Split().Select(int.Parse).ToArray();
                int t = query[0];
                if (t == 0)
                {
                    (int l, int r, int c, int d) = (query[1], query[2], query[3], query[4]);
                    seg.Apply(l, r, new Fk() { A = c, B = d });
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

        struct Operatork : ILazySegtreeOperator<Sk, Fk>
        {
            public Sk Identity => default;
            public Fk FIdentity => new Fk() { A = 1 };

            public Fk Composition(Fk l, Fk r)
                => new Fk() { A = l.A * r.A, B = l.A * r.B + l.B };

            public Sk Mapping(Fk l, Sk r)
                => new Sk() { A = r.A * l.A + r.Size * l.B, Size = r.Size };

            public Sk Operate(Sk x, Sk y)
                => new Sk { A = x.A + y.A, Size = x.Size + y.Size };
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
            => new SolverRunner(SolverL).Solve(input).Should().EqualLines(expected);
        static void SolverL(TextReader reader, TextWriter writer)
        {
            int[] nq = reader.ReadLine().Split().Select(int.Parse).ToArray();
            (int n, int q) = (nq[0], nq[1]);

            Sl[] a = reader.ReadLine().Split().Select(int.Parse)
                .Select(x => new Sl() { Zero = x ^ 1, One = x, Inversion = 0 }).ToArray();

            LazySegtree<Sl, bool, Operatorl> seg = new LazySegtree<Sl, bool, Operatorl>(a);

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

        struct Operatorl : ILazySegtreeOperator<Sl, bool>
        {
            public Sl Identity => default;
            public bool FIdentity => false;

            public bool Composition(bool l, bool r) => (l && !r) || (!l && r);

            public Sl Mapping(bool l, Sl r)
            {
                if (!l) return r;
                // swap
                return new Sl { Zero = r.One, One = r.Zero, Inversion = r.One * r.Zero - r.Inversion };
            }

            public Sl Operate(Sl l, Sl r) => new Sl
            {
                Zero = l.Zero + r.Zero,
                One = l.One + r.One,
                Inversion = l.Inversion + r.Inversion + l.One * r.Zero,
            };
        }
    }
}
