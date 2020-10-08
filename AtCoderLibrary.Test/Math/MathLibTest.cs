using System;
using System.Collections.Generic;
using System.Text;
using AtCoder.Internal;
using FluentAssertions;
using Xunit;
using static AtCoder.MathUtil;

namespace AtCoder
{
    public class MathLibTest
    {
        [Fact]
        public void PowMod()
        {
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
            for (int a = -100; a <= 100; a++)
            {
                for (int b = 0; b <= 100; b++)
                {
                    for (int c = 1; c <= 100; c++)
                    {
                        MathLib.PowMod(a, b, c).Should().Be(Native(a, b, c));
                    }
                }
            }
        }

        public static TheoryData InvBoundHandTestData = new TheoryData<long, long, long>
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
            MathLib.InvMod(x, mod).Should().Be(expected);
        }

        [Fact]
        public void InvMod()
        {
            for (long a = -100; a <= 100; a++)
            {
                for (long b = 1; b <= 1000; b++)
                {
                    if (Gcd(InternalMath.SafeMod(a, b), b) != 1) continue;
                    long c = MathLib.InvMod(a, b);
                    c.Should().BeGreaterOrEqualTo(0);
                    b.Should().BeGreaterThan(c);
                    ((a * c % b + b) % b).Should().Be(1 % b);
                }
            }
        }


        [Fact]
        public void CRTHand()
        {
            MathLib.CRT(new long[] { 1, 2, 1 }, new long[] { 2, 3, 2 }).Should().Be((5, 6));
        }
        [Fact]
        public void CRT2()
        {
            for (int a = 1; a <= 20; a++)
            {
                for (int b = 1; b <= 20; b++)
                {
                    for (int c = -10; c <= 10; c++)
                    {
                        for (int d = -10; d <= 10; d++)
                        {
                            var res = MathLib.CRT(new long[] { c, d }, new long[] { a, b });
                            if (res.Item2 == 0)
                            {
                                for (int x = 0; x < a * b / Gcd(a, b); x++)
                                {
                                    (x % a != c || x % b != d).Should().BeTrue();
                                }
                                continue;
                            }
                            res.Item2.Should().Be(a * b / Gcd(a, b));
                            (res.Item1 % a).Should().Be(InternalMath.SafeMod(c, a));
                            (res.Item1 % b).Should().Be(InternalMath.SafeMod(d, b));
                        }
                    }
                }
            }
        }
        [Fact]
        public void CRT3()
        {
            for (int a = 1; a <= 5; a++)
            {
                for (int b = 1; b <= 5; b++)
                {
                    for (int c = 1; c <= 5; c++)
                    {
                        for (int d = -5; d <= 5; d++)
                        {
                            for (int e = -5; e <= 5; e++)
                            {
                                for (int f = -5; f <= 5; f++)
                                {
                                    var res = MathLib.CRT(new long[] { d, e, f }, new long[] { a, b, c });
                                    long lcm = a * b / Gcd(a, b);
                                    lcm = lcm * c / Gcd(lcm, c);
                                    if (res.Item2 == 0)
                                    {
                                        for (int x = 0; x < lcm; x++)
                                        {
                                            (x % a != d || x % b != e || x % c != f).Should().BeTrue();
                                        }
                                        continue;
                                    }
                                    res.Item2.Should().Be(lcm);
                                    (res.Item1 % a).Should().Be(InternalMath.SafeMod(d, a));
                                    (res.Item1 % b).Should().Be(InternalMath.SafeMod(e, b));
                                    (res.Item1 % c).Should().Be(InternalMath.SafeMod(f, c));
                                }
                            }
                        }
                    }
                }
            }
        }
        [Fact]
        public void CRTOverflow()
        {
            long r0 = 0;
            long r1 = 1_000_000_000_000L - 2;
            long m0 = 900577;
            long m1 = 1_000_000_000_000L;
            var res = MathLib.CRT(new long[] { r0, r1 }, new long[] { m0, m1 });
            res.Item2.Should().Be(m0 * m1);
            (res.Item1 % m0).Should().Be(r0);
            (res.Item1 % m1).Should().Be(r1);
        }

        [SkippableFact]
        public void CRTBound()
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
                        var res = MathLib.CRT(new long[] { ans % a, ans % b }, new long[] { a, b });
                        long lcm = a / Gcd(a, b) * b;
                        res.Should().Be((ans % lcm, lcm));
                    }
                    (a, b) = (b, a);
                }
            }

            var factorInf = new long[] { 49, 73, 127, 337, 92737, 649657 };
            do
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
                    var res = MathLib.CRT(r.ToArray(), m.ToArray());
                    res.Should().Be((ans % INF, INF));
                }
            } while (NextPermutation(factorInf));

            var factorInfn1 = new long[] { 2, 3, 715827883, 2147483647 };
            do
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
                    var res = MathLib.CRT(r.ToArray(), m.ToArray());
                    res.Should().Be((ans % (INF - 1), INF - 1));
                }
            } while (NextPermutation(factorInfn1));

            static bool NextPermutation(long[] arr)
            {
                Skip.If(true, "NextPermutation is not implemented.");
                return false;
            }
        }
    }
}
