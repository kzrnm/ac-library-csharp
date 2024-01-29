using System.Collections.Generic;
using System.Numerics;
using FluentAssertions;
using Xunit;
#if NET7_0_OR_GREATER
using System;
using System.Runtime.InteropServices;
#endif

namespace AtCoder.Internal
{
    public interface IPrimitiveRootFactory : IStaticMod
    {
        int PrimitiveRoot();
    }
    public partial class InternalMathTest
    {
        static long Gcd(long a, long b)
        {
            (0 <= a && 0 <= b).Should().BeTrue();
            if (b == 0) return a;
            return Gcd(b, a % b);
        }

        private static bool IsPrimeNaive(long n)
        {
            (0 <= n && n <= int.MaxValue).Should().BeTrue();
            if (n == 0 || n == 1) return false;
            for (long i = 2; i * i <= n; i++)
            {
                if (n % i == 0) return false;
            }
            return true;
        }

        [Fact]
        public void IsPrime()
        {
            const int size = Global.IsCi ? 10000 : 500;

            InternalMath.IsPrime(121).Should().BeFalse();
            InternalMath.IsPrime(11 * 13).Should().BeFalse();
            InternalMath.IsPrime(1_000_000_007).Should().BeTrue();
            InternalMath.IsPrime(1_000_000_008).Should().BeFalse();
            InternalMath.IsPrime(1_000_000_009).Should().BeTrue();

            for (int i = 0; i <= size; i++)
            {
                InternalMath.IsPrime(i).Should().Be(IsPrimeNaive(i));
            }
            for (int i = 0; i <= size; i++)
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
                    (long first, long second) = InternalMath.InvGcd(a, b);
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
            const int size = Global.IsCi ? 10000 : 500;

            for (int m = 2; m <= size; m++)
            {
                if (!InternalMath.IsPrime(m)) continue;
                //int n = InternalMath.PrimitiveRoot(m);
                int n = mods[m].PrimitiveRoot();
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
            MathUtil.IsPrimitiveRoot(2, InternalMath.PrimitiveRoot<Mod2>()).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(3, InternalMath.PrimitiveRoot<Mod3>()).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(5, InternalMath.PrimitiveRoot<Mod5>()).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(7, InternalMath.PrimitiveRoot<Mod7>()).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(11, InternalMath.PrimitiveRoot<Mod11>()).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(998244353, InternalMath.PrimitiveRoot<Mod998244353>()).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(1000000007, InternalMath.PrimitiveRoot<Mod1000000007>()).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(469762049, InternalMath.PrimitiveRoot<Mod469762049>()).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(167772161, InternalMath.PrimitiveRoot<Mod167772161>()).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(754974721, InternalMath.PrimitiveRoot<Mod754974721>()).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(324013369, InternalMath.PrimitiveRoot<Mod324013369>()).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(831143041, InternalMath.PrimitiveRoot<Mod831143041>()).Should().BeTrue();
            MathUtil.IsPrimitiveRoot(1685283601, InternalMath.PrimitiveRoot<Mod1685283601>()).Should().BeTrue();
        }

        [Fact]
        public void PrimitiveRootTest()
        {

            for (int i = 0; i < 1000; i++)
            {
                int x = int.MaxValue - i;
                if (!InternalMath.IsPrime(x)) continue;
                MathUtil.IsPrimitiveRoot(x, mods[x].PrimitiveRoot()).Should().BeTrue();
            }
        }

#if NET7_0_OR_GREATER
        [Theory]
        [InlineData(3ul, 5ul)]
        [InlineData((1ul << 32) - 1, 5ul)]
        [InlineData(0xF0000000F0000000, 0xF0000000F0000000)]
        [InlineData(0xF000000000000000, 0xF0000000F0000000)]
        [InlineData(0x00000000F0000000, 0xF0000000F0000000)]
        [InlineData((ulong)int.MaxValue, ulong.MaxValue)]
        [InlineData((ulong)uint.MaxValue, ulong.MaxValue)]
        [InlineData((ulong)int.MaxValue, (ulong)int.MaxValue)]
        [InlineData((ulong)uint.MaxValue, (ulong)uint.MaxValue)]
        [InlineData(ulong.MaxValue, ulong.MaxValue)]
        public void Mul128Bit(ulong a, ulong b)
        {
            for (int i = -10; i <= 10; i++)
                for (int j = -10; j <= 10; j++)
                {
                    var x = a + (ulong)i;
                    var y = b + (ulong)j;
                    ulong expected = Math.BigMul(x, y, out _);
                    InternalMath.Mul128Bit(x, y).Should().Be(expected);
                    Mul128.Mul128Bit(x, y).Should().Be(expected);
                }
        }
        [Fact]
        public void Mul128BitRandom()
        {
            var rnd = new Random(227);
            var arr = new ulong[2];
            for (int i = 0; i < 50000; i++)
            {
                rnd.NextBytes(MemoryMarshal.Cast<ulong, byte>(arr));
                var x = arr[0];
                var y = arr[1];
                ulong expected = Math.BigMul(x, y, out _);
                Mul128.Mul128BitLogic(x, y).Should().Be(expected);
            }
        }
#endif
    }
}
