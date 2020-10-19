using FluentAssertions;
using Xunit;

namespace AtCoder
{
    public class MinCostFlowTest
    {
        [Fact]
        public void Simple()
        {
            var g = new McfGraphInt(4);
            g.AddEdge(0, 1, 1, 1);
            g.AddEdge(0, 2, 1, 1);
            g.AddEdge(1, 3, 1, 1);
            g.AddEdge(2, 3, 1, 1);
            g.AddEdge(1, 2, 1, 1);
            g.Slope(0, 3, 10).Should().Equal(new[] { (0, 0), (2, 4) });

            McfGraphInt.Edge e;

            e = new McfGraphInt.Edge(0, 1, 1, 1, 1);
            g.GetEdge(0).Should().Be(e);
            e = new McfGraphInt.Edge(0, 2, 1, 1, 1);
            g.GetEdge(1).Should().Be(e);
            e = new McfGraphInt.Edge(1, 3, 1, 1, 1);
            g.GetEdge(2).Should().Be(e);
            e = new McfGraphInt.Edge(2, 3, 1, 1, 1);
            g.GetEdge(3).Should().Be(e);
            e = new McfGraphInt.Edge(1, 2, 1, 0, 1);
            g.GetEdge(4).Should().Be(e);
        }
        [Fact]
        public void Usage()
        {
            {
                var g = new McfGraphInt(2);
                g.AddEdge(0, 1, 1, 2);
                g.Flow(0, 1).Should().Be((1, 2));
            }
            {
                var g = new McfGraphInt(2);
                g.AddEdge(0, 1, 1, 2);
                g.Slope(0, 1).Should().Equal(new[] { (0, 0), (1, 2) });
            }
        }
        [Fact]
        public void OutOfRange()
        {
            var g = new McfGraphInt(10);
            g.Invoking(g => g.Slope(-1, 3)).Should().ThrowDebugAssertIfDebug();
            g.Invoking(g => g.Slope(3, 3)).Should().ThrowDebugAssertIfDebug();
        }

        [Fact]
        public void SelfLoop()
        {
            var g = new McfGraphInt(3);
            g.AddEdge(0, 0, 100, 123).Should().Be(0);

            McfGraphInt.Edge e = new McfGraphInt.Edge(0, 0, 100, 0, 123);
            g.GetEdge(0).Should().Be(e);
        }

        [Fact]
        public void SameCostPaths()
        {
            var g = new McfGraphInt(3);
            g.AddEdge(0, 1, 1, 1).Should().Be(0);
            g.AddEdge(1, 2, 1, 0).Should().Be(1);
            g.AddEdge(0, 2, 2, 1).Should().Be(2);
            g.Slope(0, 2).Should().Equal(new[] { (0, 0), (3, 3) });
        }

        [Fact]
        public void Invalid()
        {
            var g = new McfGraphInt(2);
            g.Invoking(g => g.AddEdge(0, 0, -1, 0)).Should().ThrowDebugAssertIfDebug();
            g.Invoking(g => g.AddEdge(0, 0, 0, -1)).Should().ThrowDebugAssertIfDebug();
        }
    }
}
