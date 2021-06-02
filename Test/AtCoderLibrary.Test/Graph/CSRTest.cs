using AtCoder.Internal;
using FluentAssertions;
using Xunit;

namespace AtCoder
{
    public class CSRTest
    {
        [Fact]
        public void Enumerator()
        {
            var g = new CSR<int>(5, new (int, int)[] {
                (2,4),
                (0,1),
                (4,3),
                (0,2),
                (1,3),
            });
            g.Should().Equal(new (int, int)[] {
                (0,1),
                (0,2),
                (1,3),
                (2,4),
                (4,3),
            });
        }
    }
}
