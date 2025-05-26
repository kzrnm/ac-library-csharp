using System;
using AtCoder.Internal;
using MersenneTwister;
using Shouldly;
using Xunit;

namespace AtCoder
{
    public class MinCostFlowGenericMathTest
    {
        [Fact]
        public void Simple()
        {
            var g = new McfGraph<int>(4);
            g.AddEdge(0, 1, 1, 1);
            g.AddEdge(0, 2, 1, 1);
            g.AddEdge(1, 3, 1, 1);
            g.AddEdge(2, 3, 1, 1);
            g.AddEdge(1, 2, 1, 1);
            g.Slope(0, 3, 10).ShouldBe([(0, 0), (2, 4)]);

            McfGraph<int>.Edge e;

            e = new McfGraph<int>.Edge(0, 1, 1, 1, 1);
            g.GetEdge(0).ShouldBe(e);
            e = new McfGraph<int>.Edge(0, 2, 1, 1, 1);
            g.GetEdge(1).ShouldBe(e);
            e = new McfGraph<int>.Edge(1, 3, 1, 1, 1);
            g.GetEdge(2).ShouldBe(e);
            e = new McfGraph<int>.Edge(2, 3, 1, 1, 1);
            g.GetEdge(3).ShouldBe(e);
            e = new McfGraph<int>.Edge(1, 2, 1, 0, 1);
            g.GetEdge(4).ShouldBe(e);
        }

        [Fact]
        public void Cast()
        {
            var g = new McfGraph<int, long>(4);
            g.AddEdge(0, 1, 1 << 28, 1L << 33);
            g.AddEdge(0, 1, 1, 1);
            g.Slope(0, 1).ShouldBe([(0, 0L), (1, 1), ((1 << 28) + 1, (1L << 61) + 1)]);

            McfGraph<int, long>.Edge e;

            e = new McfGraph<int, long>.Edge(0, 1, 1 << 28, 1 << 28, 1L << 33);
            g.GetEdge(0).ShouldBe(e);
            e = new McfGraph<int, long>.Edge(0, 1, 1, 1, 1);
            g.GetEdge(1).ShouldBe(e);
        }
        [Fact]
        public void Usage()
        {
            {
                var g = new McfGraph<int>(2);
                g.AddEdge(0, 1, 1, 2);
                g.Flow(0, 1).ShouldBe((1, 2));
            }
            {
                var g = new McfGraph<int>(2);
                g.AddEdge(0, 1, 1, 2);
                g.Slope(0, 1).ShouldBe([(0, 0), (1, 2)]);
            }
        }
        [Fact]
        public void OutOfRange()
        {
            var g = new McfGraph<int>(2);
            new Action(() => g.Slope(-1, 3)).ShouldThrow<ContractAssertException>();
            new Action(() => g.Slope(3, 3)).ShouldThrow<ContractAssertException>();

            new Action(() => g.Flow(0, 2)).ShouldThrow<ContractAssertException>();
            new Action(() => g.Flow(2, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.Flow(2, 0, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.Flow(0, 2, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.Flow(0, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.Flow(0, 0, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.Flow(0, 1)).ShouldNotThrow();
            new Action(() => g.Flow(1, 0, 1)).ShouldNotThrow();

            new Action(() => g.AddEdge(0, 2, 0, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.AddEdge(2, 0, 0, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.AddEdge(0, 0, -1, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.AddEdge(0, 0, 0, -1)).ShouldThrow<ContractAssertException>();
            new Action(() => g.AddEdge(0, 0, 0, 0)).ShouldNotThrow();
            new Action(() => g.AddEdge(1, 0, 10, 0)).ShouldNotThrow();

            new Action(() => g.GetEdge(-1)).ShouldThrow<ContractAssertException>();
            new Action(() => g.GetEdge(2)).ShouldThrow<ContractAssertException>();
            new Action(() => g.GetEdge(0)).ShouldNotThrow();
            new Action(() => g.GetEdge(1)).ShouldNotThrow();
        }

        [Fact]
        public void SelfLoop()
        {
            var g = new McfGraph<int>(3);
            g.AddEdge(0, 0, 100, 123).ShouldBe(0);

            McfGraph<int>.Edge e = new(0, 0, 100, 0, 123);
            g.GetEdge(0).ShouldBe(e);
        }

        [Fact]
        public void SameCostPaths()
        {
            var g = new McfGraph<int>(3);
            g.AddEdge(0, 1, 1, 1).ShouldBe(0);
            g.AddEdge(1, 2, 1, 0).ShouldBe(1);
            g.AddEdge(0, 2, 2, 1).ShouldBe(2);
            g.Slope(0, 2).ShouldBe([(0, 0), (3, 3)]);
        }

        [Fact]
        public void Invalid()
        {
            var g = new McfGraph<int>(2);
            new Action(() => g.AddEdge(0, 0, -1, 0)).ShouldThrow<ContractAssertException>();
            new Action(() => g.AddEdge(0, 0, 0, -1)).ShouldThrow<ContractAssertException>();
        }

        [Fact]
        public void Stress()
        {
            var mt = MTRandom.Create();
            for (int phase = 0; phase < 1000; phase++)
            {
                int n = mt.Next(2, 21);
                int m = mt.Next(1, 101);
                var (s, t) = mt.NextPair(0, n);
                if (mt.NextBool()) (s, t) = (t, s);

                var gMf = new MfGraph<int>(n);
                var g = new McfGraph<int>(n);
                for (int i = 0; i < m; i++)
                {
                    int u = mt.Next(0, n);
                    int v = mt.Next(0, n);
                    int cap = mt.Next(0, 11);
                    int costIn = mt.Next(0, 10001);
                    g.AddEdge(u, v, cap, costIn);
                    gMf.AddEdge(u, v, cap);
                }
                var (flow, cost) = g.Flow(s, t);
                gMf.Flow(s, t).ShouldBe(flow);

                int cost2 = 0;
                var vCap = new int[n];
                foreach (var e in g.Edges())
                {
                    vCap[e.From] -= e.Flow;
                    vCap[e.To] += e.Flow;
                    cost2 += e.Flow * e.Cost;
                }
                cost.ShouldBe(cost2);

                for (int i = 0; i < n; i++)
                {
                    if (i == s)
                    {
                        (-flow).ShouldBe(vCap[i]);
                    }
                    else if (i == t)
                    {
                        flow.ShouldBe(vCap[i]);
                    }
                    else
                    {
                        vCap[i].ShouldBe(0);
                    }
                }

                // check: there is no negative-cycle
                var dist = new int[n];
                while (true)
                {
                    bool update = false;
                    foreach (var e in g.Edges())
                    {
                        if (e.Flow < e.Cap)
                        {
                            int ndist = dist[e.From] + e.Cost;
                            if (ndist < dist[e.To])
                            {
                                update = true;
                                dist[e.To] = ndist;
                            }
                        }
                        if (e.Flow != 0)
                        {
                            int ndist = dist[e.To] - e.Cost;
                            if (ndist < dist[e.From])
                            {
                                update = true;
                                dist[e.From] = ndist;
                            }
                        }
                    }
                    if (!update) break;
                }
            }
        }
    }
}
