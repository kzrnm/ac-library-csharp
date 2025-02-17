using System.Collections.Generic;
using AtCoder.Internal;
using Shouldly;
using Xunit;

namespace AtCoder
{
    public class MathLibTest
    {
        static long Gcd(long a, long b)
        {
            (0 <= a && 0 <= b).ShouldBeTrue();
            if (b == 0) return a;
            return Gcd(b, a % b);
        }

        [Fact]
        public void PowMod()
        {
            const int size = Global.IsCi ? 100 : 50;
            static long Native(long x, long n, int mod)
            {
                uint mmod = (uint)mod;
                ulong y = (ulong)InternalMath.SafeMod(x, mmod);
                ulong z = 1 % mmod;
                for (long i = 0; i < n; i++)
                {
                    z = (z * y) % mmod;
                }
                return (long)z;
            }
            for (int a = -size; a <= size; a++)
            {
                for (int b = 0; b <= size; b++)
                {
                    for (int c = 1; c <= size; c++)
                    {
                        MathLib.PowMod(a, b, c).ShouldBe(Native(a, b, c));
                    }
                }
            }
        }

        public static TheoryData InvBoundHandTestData => new TheoryData<long, long, long>
        {
            { long.MinValue, long.MaxValue, MathLib.InvMod(-1, long.MaxValue) },
            { long.MaxValue, long.MaxValue-1, 1 },
            { long.MaxValue-1, long.MaxValue, long.MaxValue-1 },
            { long.MaxValue/2+1, long.MaxValue, 2 },
        };
        [Theory]
        [MemberData(nameof(InvBoundHandTestData))]
        public void InvBoundHand(long x, long mod, long expected)
        {
            MathLib.InvMod(x, mod).ShouldBe(expected);
        }

        [Fact]
        public void InvMod()
        {
            const int size = Global.IsCi ? 100 : 50;
            for (long a = -size; a <= size; a++)
            {
                for (long b = 1; b <= size * 10; b++)
                {
                    if (Gcd(InternalMath.SafeMod(a, b), b) != 1) continue;
                    long c = MathLib.InvMod(a, b);
                    c.ShouldBeGreaterThanOrEqualTo(0);
                    b.ShouldBeGreaterThan(c);
                    ((a * c % b + b) % b).ShouldBe(1 % b);
                }
            }
        }


        [Fact]
        public void CrtHand()
        {
            MathLib.Crt([1, 2, 1], [2, 3, 2]).ShouldBe((5, 6));
        }
        [Fact]
        public void Crt2()
        {
            const int size = Global.IsCi ? 10 : 6;
            for (int a = 1; a <= size * 2; a++)
            {
                for (int b = 1; b <= size * 2; b++)
                {
                    for (int c = -size; c <= size; c++)
                    {
                        for (int d = -size; d <= size; d++)
                        {
                            var (y, m) = MathLib.Crt([c, d], [a, b]);
                            if (m == 0)
                            {
                                for (int x = 0; x < a * b / Gcd(a, b); x++)
                                {
                                    (x % a != c || x % b != d).ShouldBeTrue();
                                }
                                continue;
                            }
                            m.ShouldBe(a * b / Gcd(a, b));
                            (y % a).ShouldBe(InternalMath.SafeMod(c, a));
                            (y % b).ShouldBe(InternalMath.SafeMod(d, b));
                        }
                    }
                }
            }
        }
        [Fact]
        public void Crt3()
        {
            const int size = Global.IsCi ? 6 : 4;
            for (int a = 1; a <= size; a++)
            {
                for (int b = 1; b <= size; b++)
                {
                    for (int c = 1; c <= size; c++)
                    {
                        for (int d = -size; d <= size; d++)
                        {
                            for (int e = -size; e <= size; e++)
                            {
                                for (int f = -size; f <= size; f++)
                                {
                                    var (y, m) = MathLib.Crt([d, e, f], [a, b, c]);
                                    long lcm = a * b / Gcd(a, b);
                                    lcm = lcm * c / Gcd(lcm, c);
                                    if (m == 0)
                                    {
                                        for (int x = 0; x < lcm; x++)
                                        {
                                            (x % a != d || x % b != e || x % c != f).ShouldBeTrue();
                                        }
                                        continue;
                                    }
                                    m.ShouldBe(lcm);
                                    (y % a).ShouldBe(InternalMath.SafeMod(d, a));
                                    (y % b).ShouldBe(InternalMath.SafeMod(e, b));
                                    (y % c).ShouldBe(InternalMath.SafeMod(f, c));
                                }
                            }
                        }
                    }
                }
            }
        }
        [Fact]
        public void CrtOverflow()
        {
            long r0 = 0;
            long r1 = 1_000_000_000_000L - 2;
            long m0 = 900577;
            long m1 = 1_000_000_000_000L;
            var (y, m) = MathLib.Crt([r0, r1], [m0, m1]);
            m.ShouldBe(m0 * m1);
            (y % m0).ShouldBe(r0);
            (y % m1).ShouldBe(r1);
        }

        [Fact]
        public void CrtBound()
        {
            const long INF = long.MaxValue;
            var pred = new List<long>();
            for (int i = 1; i <= 10; i++)
            {
                pred.Add(i);
                pred.Add(INF - (i - 1));
            }
            pred.Add(998244353);
            pred.Add(1_000_000_007);
            pred.Add(1_000_000_009);

            foreach (var ab in new (long, long)[] {
                (INF, INF),
                (1, INF),
                (INF, 1),
                (7, INF),
                (INF / 337, 337),
                (2, (INF - 1) / 2),
            })
            {
                var (a, b) = ab;
                for (int ph = 0; ph < 2; ph++)
                {
                    foreach (long ans in pred)
                    {
                        var res = MathLib.Crt([ans % a, ans % b], [a, b]);
                        long lcm = a / Gcd(a, b) * b;
                        res.ShouldBe((ans % lcm, lcm));
                    }
                    (a, b) = (b, a);
                }
            }

            foreach (var factorInf in StlFunction.Permutations([49, 73, 127, 337, 92737, 649657]))
            {
                foreach (long ans in pred)
                {
                    var r = new List<long>();
                    var m = new List<long>();
                    foreach (long f in factorInf)
                    {
                        r.Add(ans % f);
                        m.Add(f);
                    }
                    var res = MathLib.Crt(r.ToArray(), m.ToArray());
                    res.ShouldBe((ans % INF, INF));
                }
            }

            foreach (var factorInfn1 in StlFunction.Permutations([2, 3, 715827883, 2147483647]))
            {
                foreach (long ans in pred)
                {
                    var r = new List<long>();
                    var m = new List<long>();
                    foreach (long f in factorInfn1)
                    {
                        r.Add(ans % f);
                        m.Add(f);
                    }
                    var res = MathLib.Crt(r.ToArray(), m.ToArray());
                    res.ShouldBe((ans % (INF - 1), INF - 1));
                }
            }
        }
    }
}
