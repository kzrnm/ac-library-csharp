using System.IO;
using System.Linq;
using Shouldly;
using MersenneTwister;
using Xunit;
using AtCoder.Internal;
using System;

namespace AtCoder
{
    public class MaxFlowGenericMathTest
    {
        [Fact]
        public void Simple()
        {
            var g = new MfGraph<int>(4);
            g.AddEdge(0, 1, 1).ShouldBe(0);
            g.AddEdge(0, 2, 1).ShouldBe(1);
            g.AddEdge(1, 3, 1).ShouldBe(2);
            g.AddEdge(2, 3, 1).ShouldBe(3);
            g.AddEdge(1, 2, 1).ShouldBe(4);
            g.Flow(0, 3).ShouldBe(2);

            MfGraph<int>.Edge e;
            e = new MfGraph<int>.Edge(0, 1, 1, 1);
            g.GetEdge(0).ShouldBe(e);
            e = new MfGraph<int>.Edge(0, 2, 1, 1);
            g.GetEdge(1).ShouldBe(e);
            e = new MfGraph<int>.Edge(1, 3, 1, 1);
            g.GetEdge(2).ShouldBe(e);
            e = new MfGraph<int>.Edge(2, 3, 1, 1);
            g.GetEdge(3).ShouldBe(e);
            e = new MfGraph<int>.Edge(1, 2, 1, 0);
            g.GetEdge(4).ShouldBe(e);

            g.MinCut(0).ShouldBe([true, false, false, false]);
        }

        [Fact]
        public void NotSimple()
        {
            var g = new MfGraph<int>(2);
            g.AddEdge(0, 1, 1).ShouldBe(0);
            g.AddEdge(0, 1, 2).ShouldBe(1);
            g.AddEdge(0, 1, 3).ShouldBe(2);
            g.AddEdge(0, 1, 4).ShouldBe(3);
            g.AddEdge(0, 1, 5).ShouldBe(4);
            g.AddEdge(0, 0, 6).ShouldBe(5);
            g.AddEdge(1, 1, 7).ShouldBe(6);
            g.Flow(0, 1).ShouldBe(15);

            MfGraph<int>.Edge e;
            e = new MfGraph<int>.Edge(0, 1, 1, 1);
            g.GetEdge(0).ShouldBe(e);
            e = new MfGraph<int>.Edge(0, 1, 2, 2);
            g.GetEdge(1).ShouldBe(e);
            e = new MfGraph<int>.Edge(0, 1, 3, 3);
            g.GetEdge(2).ShouldBe(e);
            e = new MfGraph<int>.Edge(0, 1, 4, 4);
            g.GetEdge(3).ShouldBe(e);
            e = new MfGraph<int>.Edge(0, 1, 5, 5);
            g.GetEdge(4).ShouldBe(e);

            g.MinCut(0).ShouldBe([true, false]);
        }

        [Fact]
        public void Cut()
        {
            var g = new MfGraph<int>(3);
            g.AddEdge(0, 1, 2).ShouldBe(0);
            g.AddEdge(1, 2, 1).ShouldBe(1);
            g.Flow(0, 2).ShouldBe(1);

            MfGraph<int>.Edge e;
            e = new MfGraph<int>.Edge(0, 1, 2, 1);
            g.GetEdge(0).ShouldBe(e);
            e = new MfGraph<int>.Edge(1, 2, 1, 1);
            g.GetEdge(1).ShouldBe(e);


            g.MinCut(0).ShouldBe([true, true, false]);
        }

        [Fact]
        public void Twice()
        {
            var g = new MfGraph<int>(3);
            g.AddEdge(0, 1, 1).ShouldBe(0);
            g.AddEdge(0, 2, 1).ShouldBe(1);
            g.AddEdge(1, 2, 1).ShouldBe(2);
            g.Flow(0, 2).ShouldBe(2);

            MfGraph<int>.Edge e;


            e = new MfGraph<int>.Edge(0, 1, 1, 1);
            g.GetEdge(0).ShouldBe(e);
            e = new MfGraph<int>.Edge(0, 2, 1, 1);
            g.GetEdge(1).ShouldBe(e);
            e = new MfGraph<int>.Edge(1, 2, 1, 1);
            g.GetEdge(2).ShouldBe(e);

            g.ChangeEdge(0, 100, 10);
            e = new MfGraph<int>.Edge(0, 1, 100, 10);
            g.GetEdge(0).ShouldBe(e);

            g.Flow(0, 2).ShouldBe(0);
            g.Flow(0, 1).ShouldBe(90);

            e = new MfGraph<int>.Edge(0, 1, 100, 100);
            g.GetEdge(0).ShouldBe(e);
            e = new MfGraph<int>.Edge(0, 2, 1, 1);
            g.GetEdge(1).ShouldBe(e);
            e = new MfGraph<int>.Edge(1, 2, 1, 1);
            g.GetEdge(2).ShouldBe(e);

            g.Flow(2, 0).ShouldBe(2);

            e = new MfGraph<int>.Edge(0, 1, 100, 99);
            g.GetEdge(0).ShouldBe(e);
            e = new MfGraph<int>.Edge(0, 2, 1, 0);
            g.GetEdge(1).ShouldBe(e);
            e = new MfGraph<int>.Edge(1, 2, 1, 0);
            g.GetEdge(2).ShouldBe(e);
        }
        [Fact]
        public void Bound()
        {
            MfGraph<int>.Edge e;

            const int INF = int.MaxValue;
            var g = new MfGraph<int>(3);
            g.AddEdge(0, 1, INF).ShouldBe(0);
            g.AddEdge(1, 0, INF).ShouldBe(1);
            g.AddEdge(0, 2, INF).ShouldBe(2);

            g.Flow(0, 2).ShouldBe(INF);

            e = new MfGraph<int>.Edge(0, 1, INF, 0);
            g.GetEdge(0).ShouldBe(e);
            e = new MfGraph<int>.Edge(1, 0, INF, 0);
            g.GetEdge(1).ShouldBe(e);
            e = new MfGraph<int>.Edge(0, 2, INF, INF);
            g.GetEdge(2).ShouldBe(e);
        }
        [Fact]
        public void BoundUint()
        {
            MfGraph<uint>.Edge e;

            const uint INF = uint.MaxValue;
            var g = new MfGraph<uint>(3);
            g.AddEdge(0, 1, INF).ShouldBe(0);
            g.AddEdge(1, 0, INF).ShouldBe(1);
            g.AddEdge(0, 2, INF).ShouldBe(2);

            g.Flow(0, 2).ShouldBe(INF);

            e = new MfGraph<uint>.Edge(0, 1, INF, 0);
            g.GetEdge(0).ShouldBe(e);
            e = new MfGraph<uint>.Edge(1, 0, INF, 0);
            g.GetEdge(1).ShouldBe(e);
            e = new MfGraph<uint>.Edge(0, 2, INF, INF);
            g.GetEdge(2).ShouldBe(e);
        }
        [Fact]
        public void SelfLoop()
        {
            var g = new MfGraph<int>(3);
            g.AddEdge(0, 0, 100).ShouldBe(0);

            var e = new MfGraph<int>.Edge(0, 0, 100, 0);
            g.GetEdge(0).ShouldBe(e);
        }

        [Fact]
        public void Invalid()
        {
            var g = new MfGraph<int>(2);

            new Action(() => g.Flow(0, 2)).ShouldThrow<ContractAssertException>();
            new Action(() => g.Flow(2, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.Flow(2, 0, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.Flow(0, 2, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.Flow(0, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.Flow(0, 0, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.Flow(0, 1)).ShouldNotThrow();
            new Action(() => g.Flow(1, 0, 1)).ShouldNotThrow();

            new Action(() => g.AddEdge(0, 2, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.AddEdge(2, 0, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.AddEdge(0, 0, -1)).ShouldThrow<ContractAssertException>();
            new Action(() => g.AddEdge(0, 0, 0)).ShouldNotThrow();
            new Action(() => g.AddEdge(1, 0, 10)).ShouldNotThrow();

            new Action(() => g.GetEdge(-1)).ShouldThrow<ContractAssertException>();
            new Action(() => g.GetEdge(2)).ShouldThrow<ContractAssertException>();
            new Action(() => g.GetEdge(0)).ShouldNotThrow();
            new Action(() => g.GetEdge(1)).ShouldNotThrow();

            new Action(() => g.ChangeEdge(-1, 2, 2)).ShouldThrow<ContractAssertException>();
            new Action(() => g.ChangeEdge(2, 2, 2)).ShouldThrow<ContractAssertException>();
            new Action(() => g.ChangeEdge(0, 2, 2)).ShouldNotThrow();
            new Action(() => g.ChangeEdge(1, 2, 2)).ShouldNotThrow();

            new Action(() => g.ChangeEdge(0, 1, 2)).ShouldThrow<ContractAssertException>();
            new Action(() => g.ChangeEdge(1, 1, 2)).ShouldThrow<ContractAssertException>();
            new Action(() => g.ChangeEdge(0, 0, -1)).ShouldThrow<ContractAssertException>();
        }

        [Fact]
        public void Stress()
        {
            const int size = Global.IsCi ? 10000 : 1000;

            var mt = MTRandom.Create();
            for (int phase = 0; phase < size; phase++)
            {
                int n = mt.Next(2, 21);
                int m = mt.Next(1, 101);
                var (s, t) = mt.NextPair(0, n);
                if (mt.NextBool()) (s, t) = (t, s);

                var g = new MfGraph<int>(n);
                for (int i = 0; i < m; i++)
                {
                    int u = mt.Next(0, n);
                    int v = mt.Next(0, n);
                    int c = mt.Next(0, size + 1);
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
                dual.ShouldBe(flow);
                vFlow[s].ShouldBe(-flow);
                vFlow[t].ShouldBe(flow);
                for (int i = 0; i < n; i++)
                {
                    if (i == s || i == t) continue;
                    vFlow[i].ShouldBe(0);
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
            => new SolverRunner(Solver).Solve(input).ShouldEqualLines(expected);
        static void Solver(TextReader reader, TextWriter writer)
        {
            int[] nm = reader.ReadLine().Split().Select(int.Parse).ToArray();
            (int n, int m) = (nm[0], nm[1]);

            char[][] grid = new char[n][];
            for (int i = 0; i < n; i++)
            {
                grid[i] = reader.ReadLine().ToCharArray();
            }

            var g = new MfGraph<int>(n * m + 2);
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

            MfGraph<int>.Edge[] edges = g.Edges();
            foreach (MfGraph<int>.Edge e in edges)
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
