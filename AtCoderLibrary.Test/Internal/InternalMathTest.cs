using System.Collections.Generic;
using System.Numerics;
using FluentAssertions;
using Xunit;
using static AtCoder.MathUtil;

namespace AtCoder.Internal
{
    public class InternalMathTest
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

        [Fact]
        public void IsPrime()
        {
            InternalMath.IsPrime(121).Should().BeFalse();
            InternalMath.IsPrime(11 * 13).Should().BeFalse();
            InternalMath.IsPrime(1_000_000_007).Should().BeTrue();
            InternalMath.IsPrime(1_000_000_008).Should().BeFalse();
            InternalMath.IsPrime(1_000_000_009).Should().BeTrue();

            for (int i = 0; i <= 10000; i++)
            {
                InternalMath.IsPrime(i).Should().Be(IsPrimeNaive(i));
            }
            for (int i = 0; i <= 10000; i++)
            {
                int x = int.MaxValue - i;
                InternalMath.IsPrime(x).Should().Be(IsPrimeNaive(x));
            }
        }

        [Fact]
        public void SafeMod()
        {
            var preds = new List<long>();
            for (int i = 0; i <= 100; i++)
            {
                preds.Add(i);
                preds.Add(-i);
                preds.Add(i);
                preds.Add(long.MinValue + i);
                preds.Add(long.MaxValue - i);
            }

            foreach (var a in preds)
            {
                foreach (var b in preds)
                {
                    if (b <= 0) continue;
                    var ans = (long)((new BigInteger(a) % b + b) % b);
                    InternalMath.SafeMod(a, b).Should().Be(ans);
                }
            }
        }

        [Fact]
        public void InvGcdBound()
        {
            var pred = new List<long>();
            for (int i = 0; i <= 10; i++)
            {
                pred.Add(i);
                pred.Add(-i);
                pred.Add(long.MinValue + i);
                pred.Add(long.MaxValue - i);

                pred.Add(long.MinValue / 2 + i);
                pred.Add(long.MinValue / 2 - i);
                pred.Add(long.MaxValue / 2 + i);
                pred.Add(long.MaxValue / 2 - i);

                pred.Add(long.MinValue / 3 + i);
                pred.Add(long.MinValue / 3 - i);
                pred.Add(long.MaxValue / 3 + i);
                pred.Add(long.MaxValue / 3 - i);
            }
            pred.Add(998244353);
            pred.Add(1_000_000_007);
            pred.Add(1_000_000_009);
            pred.Add(-998244353);
            pred.Add(-1_000_000_007);
            pred.Add(-1_000_000_009);

            foreach (var a in pred)
            {
                foreach (var b in pred)
                {
                    if (b <= 0) continue;
                    long a2 = InternalMath.SafeMod(a, b);
                    (long first, long second) = InternalMath.InvGCD(a, b);
                    var g = Gcd(a2, b);
                    first.Should().Be(g);
                    second.Should().BeGreaterOrEqualTo(0);

                    (b / first).Should().BeGreaterOrEqualTo(second);
                    ((long)(new BigInteger(second) * a2 % b)).Should().Be(g % b);
                }
            }
        }

        [Fact]
        public void PrimitiveRootTestNaive()
        {
            for (int m = 2; m <= 10000; m++)
            {
                if (!InternalMath.IsPrime(m)) continue;
                int n = InternalMath.PrimitiveRoot(m);
                n.Should().BeGreaterOrEqualTo(1);
                m.Should().BeGreaterThan(n);
                int x = 1;
                for (int i = 1; i <= m - 2; i++)
                {
                    x = (int)((long)x * n % m);
                    // x == n^i
                    x.Should().NotBe(1);
                }
                x = (int)((long)x * n % m);
                x.Should().Be(1);
            }
        }

        [Fact]
        public void PrimitiveRootTemplateTest()
        {
            MathUtil.IsPrimitiveRoot(2, InternalMath.PrimitiveRoot(2)).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(3, InternalMath.PrimitiveRoot(3)).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(5, InternalMath.PrimitiveRoot(5)).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(7, InternalMath.PrimitiveRoot(7)).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(11, InternalMath.PrimitiveRoot(11)).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(998244353, InternalMath.PrimitiveRoot(998244353)).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(1000000007, InternalMath.PrimitiveRoot(1000000007)).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(469762049, InternalMath.PrimitiveRoot(469762049)).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(167772161, InternalMath.PrimitiveRoot(167772161)).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(754974721, InternalMath.PrimitiveRoot(754974721)).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(324013369, InternalMath.PrimitiveRoot(324013369)).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(831143041, InternalMath.PrimitiveRoot(831143041)).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(1685283601, InternalMath.PrimitiveRoot(1685283601)).Should().BeTrue();
        }

        [Fact]
        public void PrimitiveRootTest()
        {
            for (int i = 0; i < 1000; i++)
            {
                int x = int.MaxValue - i;
                if (!InternalMath.IsPrime(x)) continue;
                MathUtil.IsPrimitiveRoot(x, InternalMath.PrimitiveRoot(x)).Should().BeTrue();
            }
        }
    }
}
