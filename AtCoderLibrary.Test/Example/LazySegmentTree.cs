using System;
using System.IO;
using System.Linq;
using AtCoder.Test.Utils;
using Xunit;

namespace AtCoder.Test.Example
{
    using MInt = StaticModInt<Mod998244353>;
    public class LazySegmentTreeTest
    {
        [ProblemTestCase(@"https://judge.yosupo.jp/problem/range_affine_range_sum")]
        public void Practice(string input, string answer)
        {
            var reader = new StringReader(input);
            var writer = new StringWriter();

            Solver(reader, writer);

            static void Solver(TextReader reader, TextWriter writer)
            {
                int[] nq = reader.ReadLine().Split().Select(int.Parse).ToArray();
                (int n, int q) = (nq[0], nq[1]);

                S[] a = reader.ReadLine().Split().Select(x => new S() { A = int.Parse(x), Size = 1 }).ToArray();

                LazySegtree<S, F, Operator> seg = new LazySegtree<S, F, Operator>(a);

                for (int i = 0; i < q; i++)
                {
                    int[] query = reader.ReadLine().Split().Select(int.Parse).ToArray();
                    int t = query[0];
                    if (t == 0)
                    {
                        (int l, int r, int c, int d) = (query[1], query[2], query[3], query[4]);
                        seg.Apply(l, r, new F() { A = c, B = d });
                    }
                    else
                    {
                        (int l, int r) = (query[1], query[2]);
                        writer.WriteLine(seg.Prod(l, r).A);
                    }
                }
            }

            Assert.True(new TokenEqualityValidator().IsValid(input, answer, writer.ToString()));
        }
    }

    struct S
    {
        public MInt A;
        public int Size;
    }

    struct F
    {
        public MInt A, B;
    }

    struct Operator : IMonoidFuncOperator<S, F>
    {
        public S Identity => default;
        public F FIdentity => new F() { A = 1 };

        public F Composition(F l, F r)
            => new F() { A = l.A * r.A, B = l.A * r.B + l.B };

        public S Mapping(F l, S r)
            => new S() { A = r.A * l.A + r.Size * l.B, Size = r.Size };

        public S Operate(S x, S y)
            => new S { A = x.A + y.A, Size = x.Size + y.Size };
    }
}
