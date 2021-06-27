using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AtCoder
{
    public class SCCTest
    {
        [Fact]
        public void Empty()
        {
            new SccGraph(0).SCC().Should().Equal(Array.Empty<int[]>());
        }
        [Fact]
        public void Simple()
        {
            var graph = new SccGraph(2);
            graph.AddEdge(0, 1);
            graph.AddEdge(1, 0);
            graph.SCC().Should().HaveCount(1);
        }
        [Fact]
        public void SelfLoop()
        {
            var graph = new SccGraph(2);
            graph.AddEdge(0, 0);
            graph.AddEdge(0, 0);
            graph.AddEdge(1, 1);
            graph.SCC().Should().HaveCount(2);
        }


        [Fact]
        public void Invalid()
        {
            var graph = new SccGraph(2);
            graph.Invoking(graph => graph.AddEdge(0, 2)).Should().ThrowContractAssert();
            graph.Invoking(graph => graph.AddEdge(2, 0)).Should().ThrowContractAssert();
            graph.Invoking(graph => graph.AddEdge(-1, 1)).Should().ThrowContractAssert();
            graph.Invoking(graph => graph.AddEdge(1, -1)).Should().ThrowContractAssert();
            graph.Invoking(graph => graph.AddEdge(0, 1)).Should().NotThrow();
            graph.Invoking(graph => graph.AddEdge(1, 0)).Should().NotThrow();
        }

        [Theory]
        [Trait("Category", "Practice")]
        [InlineData(
// Based on Library Checker https://github.com/yosupo06/library-checker-problems
// Strongly Connected Components
// Apache License 2.0
@"6 7
1 4
5 2
3 0
5 5
4 1
0 3
4 2
",

@"4
1 5
2 1 4
1 2
2 0 3
"
)]
        public void Practice(string input, string expected)
            => new SolverRunner(Solver).Solve(input).Should().EqualLines(expected);
        static void Solver(TextReader reader, TextWriter writer)
        {
            int[] nm = reader.ReadLine().Split().Select(int.Parse).ToArray();
            (int n, int m) = (nm[0], nm[1]);

            var g = new SccGraph(n);

            for (int i = 0; i < m; i++)
            {
                int[] uv = reader.ReadLine().Split().Select(int.Parse).ToArray();
                (int u, int v) = (uv[0], uv[1]);
                g.AddEdge(u, v);
            }

            var scc = g.SCC();

            writer.WriteLine(scc.Length);
            foreach (var v in scc)
            {
                writer.Write(v.Length);
                writer.Write(' ');
                writer.WriteLine(string.Join(" ", v));
            }
        }
    }
}
