using System.IO;
using System.Linq;
using FluentAssertions;
using MersenneTwister;
using Xunit;

namespace AtCoder
{
    public class MaxFlowTest
    {
        [Fact]
        public void Simple()
        {
            var g = new MFGraphInt(4);
            g.AddEdge(0, 1, 1).Should().Be(0);
            g.AddEdge(0, 2, 1).Should().Be(1);
            g.AddEdge(1, 3, 1).Should().Be(2);
            g.AddEdge(2, 3, 1).Should().Be(3);
            g.AddEdge(1, 2, 1).Should().Be(4);
            g.Flow(0, 3).Should().Be(2);

            MFGraphInt.Edge e;
            e = new MFGraphInt.Edge(0, 1, 1, 1);
            g.GetEdge(0).Should().Be(e);
            e = new MFGraphInt.Edge(0, 2, 1, 1);
            g.GetEdge(1).Should().Be(e);
            e = new MFGraphInt.Edge(1, 3, 1, 1);
            g.GetEdge(2).Should().Be(e);
            e = new MFGraphInt.Edge(2, 3, 1, 1);
            g.GetEdge(3).Should().Be(e);
            e = new MFGraphInt.Edge(1, 2, 1, 0);
            g.GetEdge(4).Should().Be(e);

            g.MinCut(0).Should().Equal(new[] { true, false, false, false });
        }

        [Fact]
        public void NotSimple()
        {
            var g = new MFGraphInt(2);
            g.AddEdge(0, 1, 1).Should().Be(0);
            g.AddEdge(0, 1, 2).Should().Be(1);
            g.AddEdge(0, 1, 3).Should().Be(2);
            g.AddEdge(0, 1, 4).Should().Be(3);
            g.AddEdge(0, 1, 5).Should().Be(4);
            g.AddEdge(0, 0, 6).Should().Be(5);
            g.AddEdge(1, 1, 7).Should().Be(6);
            g.Flow(0, 1).Should().Be(15);

            MFGraphInt.Edge e;
            e = new MFGraphInt.Edge(0, 1, 1, 1);
            g.GetEdge(0).Should().Be(e);
            e = new MFGraphInt.Edge(0, 1, 2, 2);
            g.GetEdge(1).Should().Be(e);
            e = new MFGraphInt.Edge(0, 1, 3, 3);
            g.GetEdge(2).Should().Be(e);
            e = new MFGraphInt.Edge(0, 1, 4, 4);
            g.GetEdge(3).Should().Be(e);
            e = new MFGraphInt.Edge(0, 1, 5, 5);
            g.GetEdge(4).Should().Be(e);

            g.MinCut(0).Should().Equal(new[] { true, false });
        }

        [Fact]
        public void Cut()
        {
            var g = new MFGraphInt(3);
            g.AddEdge(0, 1, 2).Should().Be(0);
            g.AddEdge(1, 2, 1).Should().Be(1);
            g.Flow(0, 2).Should().Be(1);

            MFGraphInt.Edge e;
            e = new MFGraphInt.Edge(0, 1, 2, 1);
            g.GetEdge(0).Should().Be(e);
            e = new MFGraphInt.Edge(1, 2, 1, 1);
            g.GetEdge(1).Should().Be(e);


            g.MinCut(0).Should().Equal(new[] { true, true, false });
        }

        [Fact]
        public void Twice()
        {
            var g = new MFGraphInt(3);
            g.AddEdge(0, 1, 1).Should().Be(0);
            g.AddEdge(0, 2, 1).Should().Be(1);
            g.AddEdge(1, 2, 1).Should().Be(2);
            g.Flow(0, 2).Should().Be(2);

            MFGraphInt.Edge e;


            e = new MFGraphInt.Edge(0, 1, 1, 1);
            g.GetEdge(0).Should().Be(e);
            e = new MFGraphInt.Edge(0, 2, 1, 1);
            g.GetEdge(1).Should().Be(e);
            e = new MFGraphInt.Edge(1, 2, 1, 1);
            g.GetEdge(2).Should().Be(e);

            g.ChangeEdge(0, 100, 10);
            e = new MFGraphInt.Edge(0, 1, 100, 10);
            g.GetEdge(0).Should().Be(e);

            g.Flow(0, 2).Should().Be(0);
            g.Flow(0, 1).Should().Be(90);

            e = new MFGraphInt.Edge(0, 1, 100, 100);
            g.GetEdge(0).Should().Be(e);
            e = new MFGraphInt.Edge(0, 2, 1, 1);
            g.GetEdge(1).Should().Be(e);
            e = new MFGraphInt.Edge(1, 2, 1, 1);
            g.GetEdge(2).Should().Be(e);

            g.Flow(2, 0).Should().Be(2);

            e = new MFGraphInt.Edge(0, 1, 100, 99);
            g.GetEdge(0).Should().Be(e);
            e = new MFGraphInt.Edge(0, 2, 1, 0);
            g.GetEdge(1).Should().Be(e);
            e = new MFGraphInt.Edge(1, 2, 1, 0);
            g.GetEdge(2).Should().Be(e);
        }
        [Fact]
        public void Bound()
        {
            MFGraphInt.Edge e;

            const int INF = int.MaxValue;
            var g = new MFGraphInt(3);
            g.AddEdge(0, 1, INF).Should().Be(0);
            g.AddEdge(1, 0, INF).Should().Be(1);
            g.AddEdge(0, 2, INF).Should().Be(2);

            g.Flow(0, 2).Should().Be(INF);

            e = new MFGraphInt.Edge(0, 1, INF, 0);
            g.GetEdge(0).Should().Be(e);
            e = new MFGraphInt.Edge(1, 0, INF, 0);
            g.GetEdge(1).Should().Be(e);
            e = new MFGraphInt.Edge(0, 2, INF, INF);
            g.GetEdge(2).Should().Be(e);
        }
        [Fact]
        public void BoundUint()
        {
            MFGraph<uint, UIntOperator>.Edge e;

            const uint INF = uint.MaxValue;
            var g = new MFGraph<uint, UIntOperator>(3);
            g.AddEdge(0, 1, INF).Should().Be(0);
            g.AddEdge(1, 0, INF).Should().Be(1);
            g.AddEdge(0, 2, INF).Should().Be(2);

            g.Flow(0, 2).Should().Be(INF);

            e = new MFGraph<uint, UIntOperator>.Edge(0, 1, INF, 0);
            g.GetEdge(0).Should().Be(e);
            e = new MFGraph<uint, UIntOperator>.Edge(1, 0, INF, 0);
            g.GetEdge(1).Should().Be(e);
            e = new MFGraph<uint, UIntOperator>.Edge(0, 2, INF, INF);
            g.GetEdge(2).Should().Be(e);
        }
        [Fact]
        public void SelfLoop()
        {
            var g = new MFGraphInt(3);
            g.AddEdge(0, 0, 100).Should().Be(0);

            MFGraphInt.Edge e = new MFGraphInt.Edge(0, 0, 100, 0);
            g.GetEdge(0).Should().Be(e);
        }

        [Fact]
        public void Invalid()
        {
            var g = new MFGraphInt(2);

            g.Invoking(g => g.Flow(0, 2)).Should().ThrowContractAssert();
            g.Invoking(g => g.Flow(2, 0)).Should().ThrowContractAssert();
            g.Invoking(g => g.Flow(2, 0, 0)).Should().ThrowContractAssert();
            g.Invoking(g => g.Flow(0, 2, 0)).Should().ThrowContractAssert();
            g.Invoking(g => g.Flow(0, 0)).Should().ThrowContractAssert();
            g.Invoking(g => g.Flow(0, 0, 0)).Should().ThrowContractAssert();
            g.Invoking(g => g.Flow(0, 1)).Should().NotThrow();
            g.Invoking(g => g.Flow(1, 0, 1)).Should().NotThrow();

            g.Invoking(g => g.AddEdge(0, 2, 0)).Should().ThrowContractAssert();
            g.Invoking(g => g.AddEdge(2, 0, 0)).Should().ThrowContractAssert();
            g.Invoking(g => g.AddEdge(0, 0, -1)).Should().ThrowContractAssert();
            g.Invoking(g => g.AddEdge(0, 0, 0)).Should().NotThrow();
            g.Invoking(g => g.AddEdge(1, 0, 10)).Should().NotThrow();

            g.Invoking(g => g.GetEdge(-1)).Should().ThrowContractAssert();
            g.Invoking(g => g.GetEdge(2)).Should().ThrowContractAssert();
            g.Invoking(g => g.GetEdge(0)).Should().NotThrow();
            g.Invoking(g => g.GetEdge(1)).Should().NotThrow();

            g.Invoking(g => g.ChangeEdge(-1, 2, 2)).Should().ThrowContractAssert();
            g.Invoking(g => g.ChangeEdge(2, 2, 2)).Should().ThrowContractAssert();
            g.Invoking(g => g.ChangeEdge(0, 2, 2)).Should().NotThrow();
            g.Invoking(g => g.ChangeEdge(1, 2, 2)).Should().NotThrow();

            g.Invoking(g => g.ChangeEdge(0, 1, 2)).Should().ThrowContractAssert();
            g.Invoking(g => g.ChangeEdge(1, 1, 2)).Should().ThrowContractAssert();
            g.Invoking(g => g.ChangeEdge(0, 0, -1)).Should().ThrowContractAssert();
        }

        [Fact]
        public void Stress()
        {
            var mt = MTRandom.Create();
            for (int phase = 0; phase < 10000; phase++)
            {
                int n = mt.Next(2, 21);
                int m = mt.Next(1, 101);
                var (s, t) = mt.NextPair(0, n);
                if (mt.NextBool()) (s, t) = (t, s);

                var g = new MFGraphInt(n);
                for (int i = 0; i < m; i++)
                {
                    int u = mt.Next(0, n);
                    int v = mt.Next(0, n);
                    int c = mt.Next(0, 10001);
                    g.AddEdge(u, v, c);
                }
                int flow = g.Flow(s, t);
                int dual = 0;
                var cut = g.MinCut(s);
                var vFlow = new int[n];
                foreach (var e in g.Edges())
                {
                    vFlow[e.From] -= e.Flow;
                    vFlow[e.To] += e.Flow;
                    if (cut[e.From] && !cut[e.To]) dual += e.Cap;
                }
                dual.Should().Be(flow);
                vFlow[s].Should().Be(-flow);
                vFlow[t].Should().Be(flow);
                for (int i = 0; i < n; i++)
                {
                    if (i == s || i == t) continue;
                    vFlow[i].Should().Be(0);
                }
            }
        }

        [Theory]
        [Trait("Category", "Practice")]
        [InlineData(
// Based on https://atcoder.jp/contests/practice2/tasks/practice2_d
@"3 3
#..
..#
...
",

@"3
#><
><#
><.
"
)]
        public void Practice(string input, string expected)
            => new SolverRunner(Solver).Solve(input).Should().EqualLines(expected);
        static void Solver(TextReader reader, TextWriter writer)
        {
            int[] nm = reader.ReadLine().Split().Select(int.Parse).ToArray();
            (int n, int m) = (nm[0], nm[1]);

            char[][] grid = new char[n][];
            for (int i = 0; i < n; i++)
            {
                grid[i] = reader.ReadLine().ToCharArray();
            }

            var g = new MFGraphInt(n * m + 2);
            int s = n * m, t = n * m + 1;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (grid[i][j] == '#') continue;
                    int v = i * m + j;
                    if ((i + j) % 2 == 0)
                    {
                        g.AddEdge(s, v, 1);
                    }
                    else
                    {
                        g.AddEdge(v, t, 1);
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if ((i + j) % 2 != 0 || grid[i][j] == '#') continue;
                    int v0 = i * m + j;
                    if (i != 0 && grid[i - 1][j] == '.')
                    {
                        int v1 = (i - 1) * m + j;
                        g.AddEdge(v0, v1, 1);
                    }
                    if (j != 0 && grid[i][j - 1] == '.')
                    {
                        int v1 = i * m + (j - 1);
                        g.AddEdge(v0, v1, 1);
                    }
                    if (i + 1 < n && grid[i + 1][j] == '.')
                    {
                        int v1 = (i + 1) * m + j;
                        g.AddEdge(v0, v1, 1);
                    }
                    if (j + 1 < m && grid[i][j + 1] == '.')
                    {
                        int v1 = i * m + (j + 1);
                        g.AddEdge(v0, v1, 1);
                    }
                }
            }

            writer.WriteLine(g.Flow(s, t));

            MFGraph<int, IntOperator>.Edge[] edges = g.Edges();
            foreach (MFGraph<int, IntOperator>.Edge e in edges)
            {
                if (e.From == s || e.To == t || e.Flow == 0) continue;
                int i0 = e.From / m, j0 = e.From % m;
                int i1 = e.To / m, j1 = e.To % m;

                if (i0 == i1 + 1)
                {
                    grid[i1][j1] = 'v';
                    grid[i0][j0] = '^';
                }
                else if (j0 == j1 + 1)
                {
                    grid[i1][j1] = '>'; grid[i0][j0] = '<';
                }
                else if (i0 == i1 - 1)
                {
                    grid[i0][j0] = 'v';
                    grid[i1][j1] = '^';
                }
                else
                {
                    grid[i0][j0] = '>'; grid[i1][j1] = '<';
                }
            }

            for (int i = 0; i < n; i++)
            {
                writer.WriteLine(grid[i]);
            }
        }
    }
}
