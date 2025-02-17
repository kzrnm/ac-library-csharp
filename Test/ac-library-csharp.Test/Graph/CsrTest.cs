using AtCoder.Internal;
using Shouldly;
using Xunit;

namespace AtCoder
{
    public class CsrTest
    {
        [Fact]
        public void Enumerator()
        {
            var g = new Csr<int>(5, [
                (2,4),
                (0,1),
                (4,3),
                (0,2),
                (1,3),
            ]);
            g.ShouldBe([
                (0,1),
                (0,2),
                (1,3),
                (2,4),
                (4,3),
            ]);
        }
    }
}
