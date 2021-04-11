using System.Collections.Generic;
using System.Runtime.InteropServices;
using AtCoder.Internal;
using FluentAssertions;
using MersenneTwister;
using Xunit;

namespace AtCoder
{
    public class ModIntTest
    {
        private static long Gcd(long a, long b)
        {
            if (b == 0) return a;
            return Gcd(b, a % b);
        }


        private struct DynamicBorderID : IDynamicModID { }
        [Fact]
        public void DynamicBorder()
        {
            for (int mod = int.MaxValue; mod >= int.MaxValue - 20; mod--)
            {
                DynamicModInt<DynamicBorderID>.Mod = mod;
                var v = new List<long>();
                for (int i = 0; i < 10; i++)
                {
                    v.Add(i);
                    v.Add(mod - i);
                    v.Add(mod / 2 + i);
                    v.Add(mod / 2 - i);
                }
                foreach (var a in v)
                {
                    new DynamicModInt<DynamicBorderID>(a).Pow(3).Value
                        .Should().Be((int)(((a * a) % mod * a) % mod));
                    foreach (var b in v)
                    {
                        (new DynamicModInt<DynamicBorderID>(a) + b).Value.Should().Be((int)InternalMath.SafeMod(a + b, mod));
                        (new DynamicModInt<DynamicBorderID>(a) - b).Value.Should().Be((int)InternalMath.SafeMod(a - b, mod));
                        (new DynamicModInt<DynamicBorderID>(a) * b).Value.Should().Be((int)InternalMath.SafeMod(a * b, mod));
                    }
                }
            }
        }

        private struct Mod1ID : IStaticMod, IDynamicModID
        {
            public uint Mod => 1;
            public bool IsPrime => false;
        }
        [Fact]
        public void Mod1()
        {
            DynamicModInt<Mod1ID>.Mod = 1;
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    (new DynamicModInt<Mod1ID>(i) * j).Value.Should().Be(0);
                }
            }
            (new DynamicModInt<Mod1ID>(1234) + 5678).Value.Should().Be(0);
            (new DynamicModInt<Mod1ID>(1234) - 5678).Value.Should().Be(0);
            (new DynamicModInt<Mod1ID>(1234) * 5678).Value.Should().Be(0);
            //(new DynamicModInt<Mod1ID>(1234).Pow(5678)).Value.Should().Be(0); // faild in Debug.Assert
            (new DynamicModInt<Mod1ID>(0).Inv()).Value.Should().Be(0);

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    (new StaticModInt<Mod1ID>(i) * j).Value.Should().Be(0);
                }
            }
            (new StaticModInt<Mod1ID>(1234) + 5678).Value.Should().Be(0);
            (new StaticModInt<Mod1ID>(1234) - 5678).Value.Should().Be(0);
            (new StaticModInt<Mod1ID>(1234) * 5678).Value.Should().Be(0);
            //(new StaticModInt<Mod1ID>(1234).Pow(5678)).Value.Should().Be(0); // faild in Debug.Assert
            (new StaticModInt<Mod1ID>(0).Inv()).Value.Should().Be(0);
        }

        private struct InvID11 : IStaticMod, IDynamicModID
        {
            public uint Mod => 11;
            public bool IsPrime => true;
        }
        private struct InvID12 : IStaticMod, IDynamicModID
        {
            public uint Mod => 12;
            public bool IsPrime => false;
        }
        private struct InvID1000000007 : IStaticMod, IDynamicModID
        {
            public uint Mod => 1000000007;
            public bool IsPrime => false;
        }
        private struct InvID1000000008 : IStaticMod, IDynamicModID
        {
            public uint Mod => 1000000008;
            public bool IsPrime => false;
        }
        private struct InvID998244353 : IStaticMod, IDynamicModID
        {
            public uint Mod => 998244353;
            public bool IsPrime => true;
        }
        [Fact]
        public void Inv()
        {
            for (int i = 1; i < 10; i++)
            {
                int x = new StaticModInt<InvID11>(i).Inv().Value;
                ((long)x * i % 11).Should().Be(1);
            }


            for (int i = 1; i < 12; i++)
            {
                if (Gcd(i, 12) != 1) continue;
                int x = new StaticModInt<InvID12>(i).Inv().Value;
                ((long)x * i % 12).Should().Be(1);
            }

            for (int i = 1; i < 100000; i++)
            {
                int x = new StaticModInt<InvID1000000007>(i).Inv().Value;
                ((long)x * i % 1000000007).Should().Be(1);
            }

            for (int i = 1; i < 100000; i++)
            {
                if (Gcd(i, 1000000008) != 1) continue;
                int x = new StaticModInt<InvID1000000008>(i).Inv().Value;
                ((long)x * i % 1000000008).Should().Be(1);
            }

            for (int i = 1; i < 100000; i++)
            {
                int x = new StaticModInt<InvID998244353>(i).Inv().Value;
                ((long)x * i % 998244353).Should().Be(1);
            }

            DynamicModInt<InvID998244353>.Mod = 998244353;
            for (int i = 1; i < 100000; i++)
            {
                int x = new DynamicModInt<InvID998244353>(i).Inv().Value;
                x.Should().BeGreaterOrEqualTo(0);
                x.Should().BeLessOrEqualTo(998244353 - 1);
                ((long)x * i % 998244353).Should().Be(1);
            }


            DynamicModInt<InvID1000000008>.Mod = 1000000008;
            for (int i = 1; i < 100000; i++)
            {
                if (Gcd(i, 1000000008) != 1) continue;
                int x = new DynamicModInt<InvID1000000008>(i).Inv().Value;
                ((long)x * i % 1000000008).Should().Be(1);
            }
        }

        private struct IncrementID : IStaticMod, IDynamicModID
        {
            public uint Mod => 11;
            public bool IsPrime => true;
        }
        [Fact]
        public void Increment()
        {
            DynamicModInt<IncrementID>.Mod = 11;
            {
                StaticModInt<IncrementID> a;
                a = 8;
                (++a).Value.Should().Be(9);
                (++a).Value.Should().Be(10);
                (++a).Value.Should().Be(0);
                (++a).Value.Should().Be(1);
                a = 3;
                (--a).Value.Should().Be(2);
                (--a).Value.Should().Be(1);
                (--a).Value.Should().Be(0);
                (--a).Value.Should().Be(10);
                a = 8;
                (a++).Value.Should().Be(8);
                (a++).Value.Should().Be(9);
                (a++).Value.Should().Be(10);
                (a++).Value.Should().Be(0);
                a.Value.Should().Be(1);
                a = 3;
                (a--).Value.Should().Be(3);
                (a--).Value.Should().Be(2);
                (a--).Value.Should().Be(1);
                (a--).Value.Should().Be(0);
                a.Value.Should().Be(10);
            }
            {
                DynamicModInt<IncrementID> a;
                a = 8;
                (++a).Value.Should().Be(9);
                (++a).Value.Should().Be(10);
                (++a).Value.Should().Be(0);
                (++a).Value.Should().Be(1);
                a = 3;
                (--a).Value.Should().Be(2);
                (--a).Value.Should().Be(1);
                (--a).Value.Should().Be(0);
                (--a).Value.Should().Be(10);
                a = 8;
                (a++).Value.Should().Be(8);
                (a++).Value.Should().Be(9);
                (a++).Value.Should().Be(10);
                (a++).Value.Should().Be(0);
                a.Value.Should().Be(1);
                a = 3;
                (a--).Value.Should().Be(3);
                (a--).Value.Should().Be(2);
                (a--).Value.Should().Be(1);
                (a--).Value.Should().Be(0);
                a.Value.Should().Be(10);
            }
        }


        private struct DynamicUsageID : IDynamicModID { }
        [Fact]
        public void DynamicUsage()
        {
            DynamicModInt<DynamicUsageID>.Mod = 998244353;
            DynamicModInt<DynamicUsageID>.Mod.Should().Be(998244353);
            (new DynamicModInt<DynamicUsageID>(1) + new DynamicModInt<DynamicUsageID>(2)).Value.Should().Be(3);
            (1 + new DynamicModInt<DynamicUsageID>(2)).Value.Should().Be(3);
            (new DynamicModInt<DynamicUsageID>(1) + 2).Value.Should().Be(3);

            DynamicModInt<DynamicUsageID>.Mod = 3;
            DynamicModInt<DynamicUsageID>.Mod.Should().Be(3);
            (new DynamicModInt<DynamicUsageID>(2) - new DynamicModInt<DynamicUsageID>(1)).Value.Should().Be(1);
            (new DynamicModInt<DynamicUsageID>(1) + new DynamicModInt<DynamicUsageID>(2)).Value.Should().Be(0);

            DynamicModInt<DynamicUsageID>.Mod = 11;
            DynamicModInt<DynamicUsageID>.Mod.Should().Be(11);
            (new DynamicModInt<DynamicUsageID>(3) * new DynamicModInt<DynamicUsageID>(5)).Value.Should().Be(4);

            (+new DynamicModInt<DynamicUsageID>(4)).Should().Be(new DynamicModInt<DynamicUsageID>(4));
            (-new DynamicModInt<DynamicUsageID>(4)).Should().Be(new DynamicModInt<DynamicUsageID>(7));

            (new DynamicModInt<DynamicUsageID>(1) == new DynamicModInt<DynamicUsageID>(3)).Should().BeFalse();
            (new DynamicModInt<DynamicUsageID>(1) != new DynamicModInt<DynamicUsageID>(3)).Should().BeTrue();
            (new DynamicModInt<DynamicUsageID>(1) == new DynamicModInt<DynamicUsageID>(12)).Should().BeTrue();
            (new DynamicModInt<DynamicUsageID>(1) != new DynamicModInt<DynamicUsageID>(12)).Should().BeFalse();

            new DynamicModInt<DynamicUsageID>(3).Invoking(m => m.Pow(-1)).Should().ThrowContractAssert();
        }

        private struct ConstructorID : IStaticMod, IDynamicModID
        {
            public uint Mod => 11;
            public bool IsPrime => true;
        }
        [Fact]
        public void Constructor()
        {
            DynamicModInt<ConstructorID>.Mod = 11;
            new DynamicModInt<ConstructorID>(3).Value.Should().Be(3);
            new DynamicModInt<ConstructorID>(-10).Value.Should().Be(1);
            (1 + new DynamicModInt<ConstructorID>(1)).Value.Should().Be(2);
        }

        [Fact]
        public void ConstructorStatic()
        {
            new StaticModInt<ConstructorID>(3).Value.Should().Be(3);
            new StaticModInt<ConstructorID>(-10).Value.Should().Be(1);
            (1 + new StaticModInt<ConstructorID>(1)).Value.Should().Be(2);
        }

        private struct MemoryID : IStaticMod, IDynamicModID
        {
            public uint Mod => 101;
            public bool IsPrime => true;
        }
        [Fact]
        public void Memory()
        {
            DynamicModInt<MemoryID>.Mod = 101;

            var mt = MTRandom.Create();
            for (int n = 0; n < 100; n++)
            {
                var arr = new DynamicModInt<MemoryID>[n];
                var expected = new uint[n];
                for (int i = 0; i < n; i++)
                {
                    var v = mt.NextUInt();
                    arr[i] = v;
                    expected[i] = v % 101;
                }
                MemoryMarshal.Cast<DynamicModInt<MemoryID>, uint>(arr).ToArray()
                    .Should().Equal(expected);
            }
        }

        [Fact]
        public void MemoryStatic()
        {
            var mt = MTRandom.Create();
            for (int n = 0; n < 100; n++)
            {
                var arr = new StaticModInt<MemoryID>[n];
                var expected = new uint[n];
                for (int i = 0; i < n; i++)
                {
                    var v = mt.NextUInt();
                    arr[i] = v;
                    expected[i] = v % 101;
                }
                MemoryMarshal.Cast<StaticModInt<MemoryID>, uint>(arr).ToArray()
                    .Should().Equal(expected);
            }
        }


    }
}
