using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace AtCoder.Internal
{
    public class BarrettTest
    {
        [Fact]
        public void Barrett()
        {
            for (uint m = 1; m <= 100; m++)
            {
                var bt = new Barrett(m);
                for (uint a = 0; a < m; a++)
                {
                    for (uint b = 0; b < m; b++)
                    {
                        bt.Mul(a, b).Should().Be((a * b) % m);
                    }
                }
            }

            new Barrett(1).Mul(0, 0).Should().Be(0);
        }

        [Fact]
        public void BarrettBorder()
        {
            for (uint mod = int.MaxValue; mod >= int.MaxValue - 20; mod--)
            {
                var bt = new Barrett(mod);
                var v = new List<uint>();
                for (uint i = 0; i < 10; i++)
                {
                    v.Add(i);
                    v.Add(mod - i);
                    v.Add(mod / 2 + i);
                    v.Add(mod / 2 - i);
                }
                foreach (var a in v)
                {
                    long a2 = a;
                    bt.Mul(a, bt.Mul(a, a)).Should().Be((uint)(a2 * a2 % mod * a2 % mod));
                    foreach (var b in v)
                    {
                        long b2 = b;
                        bt.Mul(a, b).Should().Be((uint)(a2 * b2 % mod));
                    }
                }
            }
        }

    }
}
