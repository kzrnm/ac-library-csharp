using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using AtCoder.Internal;
using MersenneTwister;
using Shouldly;
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


        private struct DynamicBorderID { }
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
                        .ShouldBe((int)(((a * a) % mod * a) % mod));
                    foreach (var b in v)
                    {
                        (new DynamicModInt<DynamicBorderID>(a) + b).Value.ShouldBe((int)InternalMath.SafeMod(a + b, mod));
                        (new DynamicModInt<DynamicBorderID>(a) - b).Value.ShouldBe((int)InternalMath.SafeMod(a - b, mod));
                        (new DynamicModInt<DynamicBorderID>(a) * b).Value.ShouldBe((int)InternalMath.SafeMod(a * b, mod));
                    }
                }
            }
        }

        private readonly struct Mod1ID : IStaticMod
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
                    (new DynamicModInt<Mod1ID>(i) * j).Value.ShouldBe(0);
                }
            }
            (new DynamicModInt<Mod1ID>(1234) + 5678).Value.ShouldBe(0);
            (new DynamicModInt<Mod1ID>(1234) - 5678).Value.ShouldBe(0);
            (new DynamicModInt<Mod1ID>(1234) * 5678).Value.ShouldBe(0);
            //(new DynamicModInt<Mod1ID>(1234).Pow(5678)).Value.ShouldBe(0); // faild in Debug.Assert
            (new DynamicModInt<Mod1ID>(0).Inv()).Value.ShouldBe(0);

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    (new StaticModInt<Mod1ID>(i) * j).Value.ShouldBe(0);
                }
            }
            (new StaticModInt<Mod1ID>(1234) + 5678).Value.ShouldBe(0);
            (new StaticModInt<Mod1ID>(1234) - 5678).Value.ShouldBe(0);
            (new StaticModInt<Mod1ID>(1234) * 5678).Value.ShouldBe(0);
            //(new StaticModInt<Mod1ID>(1234).Pow(5678)).Value.ShouldBe(0); // faild in Debug.Assert
            (new StaticModInt<Mod1ID>(0).Inv()).Value.ShouldBe(0);
        }

        private readonly struct ModID11 : IStaticMod
        {
            public uint Mod => 11;
            public bool IsPrime => true;
        }
        private readonly struct ModID12 : IStaticMod
        {
            public uint Mod => 12;
            public bool IsPrime => false;
        }
        private readonly struct ModID1000000007 : IStaticMod
        {
            public uint Mod => 1000000007;
            public bool IsPrime => false;
        }
        private readonly struct ModID1000000008 : IStaticMod
        {
            public uint Mod => 1000000008;
            public bool IsPrime => false;
        }
        private readonly struct ModID998244353 : IStaticMod
        {
            public uint Mod => 998244353;
            public bool IsPrime => true;
        }
        [Fact]
        public void Inv()
        {
            RunStatic<ModID11>();
            RunStatic<ModID12>();
            RunStatic<ModID1000000007>();
            RunStatic<ModID1000000008>();
            RunStatic<ModID998244353>();

            DynamicModInt<ModID998244353>.Mod = 998244353;
            RunDynamic<ModID998244353>();

            DynamicModInt<ModID1000000008>.Mod = 1000000008;
            RunDynamic<ModID1000000008>();

            void RunStatic<T>() where T : struct, IStaticMod
            {
                var mod = (int)new T().Mod;
                var nums = Enumerable.Range(-100, 200).Concat(Enumerable.Range(mod - 100, 200));
                foreach (var n in nums)
                {
                    int i = (int)InternalMath.SafeMod(n, mod);
                    if (i == 0 || !new T().IsPrime && Gcd(i, mod) != 1) continue;
                    int x = new StaticModInt<T>(i).Inv().Value;
                    ((long)x * i % mod).ShouldBe(1);
                }
            }

            void RunDynamic<T>() where T : struct
            {
                var mod = DynamicModInt<T>.Mod;
                var nums = Enumerable.Range(-100, 200).Concat(Enumerable.Range(mod - 100, 200));
                foreach (var n in nums)
                {
                    int i = (int)InternalMath.SafeMod(n, mod);
                    if (i == 0 || !InternalMath.IsPrime(mod) && Gcd(i, mod) != 1) continue;
                    int x = new DynamicModInt<T>(i).Inv().Value;
                    ((long)x * i % mod).ShouldBe(1);
                }
            }
        }

        [Fact]
        public void Pow()
        {
            RunStatic<ModID11>();
            RunStatic<ModID12>();
            RunStatic<ModID1000000007>();
            RunStatic<ModID1000000008>();
            RunStatic<ModID998244353>();

            DynamicModInt<ModID998244353>.Mod = 998244353;
            RunDynamic<ModID998244353>();

            DynamicModInt<ModID1000000008>.Mod = 1000000008;
            RunDynamic<ModID1000000008>();

            void RunStatic<T>() where T : struct, IStaticMod
            {
                var mod = (int)new T().Mod;
                var nums = Enumerable.Range(-100, 200).Concat(Enumerable.Range(mod - 100, 200));
                foreach (var n in nums)
                {
                    StaticModInt<T> num = n;
                    StaticModInt<T> expected = 1;
                    for (int i = 0; i < 200; i++)
                    {
                        num.Pow(i).ShouldBe(expected);
                        expected *= num;
                    }
                }
            }

            void RunDynamic<T>() where T : struct
            {
                var mod = DynamicModInt<T>.Mod;
                var nums = Enumerable.Range(-100, 200).Concat(Enumerable.Range(mod - 100, 200));
                foreach (var n in nums)
                {
                    DynamicModInt<T> num = n;
                    DynamicModInt<T> expected = 1;
                    for (int i = 0; i < 200; i++)
                    {
                        num.Pow(i).ShouldBe(expected);
                        expected *= num;
                    }
                }
            }
        }

        private readonly struct IncrementID : IStaticMod
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
                (++a).Value.ShouldBe(9);
                (++a).Value.ShouldBe(10);
                (++a).Value.ShouldBe(0);
                (++a).Value.ShouldBe(1);
                a = 3;
                (--a).Value.ShouldBe(2);
                (--a).Value.ShouldBe(1);
                (--a).Value.ShouldBe(0);
                (--a).Value.ShouldBe(10);
                a = 8;
                (a++).Value.ShouldBe(8);
                (a++).Value.ShouldBe(9);
                (a++).Value.ShouldBe(10);
                (a++).Value.ShouldBe(0);
                a.Value.ShouldBe(1);
                a = 3;
                (a--).Value.ShouldBe(3);
                (a--).Value.ShouldBe(2);
                (a--).Value.ShouldBe(1);
                (a--).Value.ShouldBe(0);
                a.Value.ShouldBe(10);
            }
            {
                DynamicModInt<IncrementID> a;
                a = 8;
                (++a).Value.ShouldBe(9);
                (++a).Value.ShouldBe(10);
                (++a).Value.ShouldBe(0);
                (++a).Value.ShouldBe(1);
                a = 3;
                (--a).Value.ShouldBe(2);
                (--a).Value.ShouldBe(1);
                (--a).Value.ShouldBe(0);
                (--a).Value.ShouldBe(10);
                a = 8;
                (a++).Value.ShouldBe(8);
                (a++).Value.ShouldBe(9);
                (a++).Value.ShouldBe(10);
                (a++).Value.ShouldBe(0);
                a.Value.ShouldBe(1);
                a = 3;
                (a--).Value.ShouldBe(3);
                (a--).Value.ShouldBe(2);
                (a--).Value.ShouldBe(1);
                (a--).Value.ShouldBe(0);
                a.Value.ShouldBe(10);
            }
        }

        [Fact]
        public void Parse()
        {
            var bigs = new[]{
                BigInteger.Pow(10, 1000),
                BigInteger.Pow(10, 1000),
            };
            var invalids = new[]
            {
                "2-2",
                "ABC",
                "111.0",
                "111,1",
            };
            RunStatic<ModID11>();
            RunStatic<ModID12>();
            RunStatic<ModID1000000007>();
            RunStatic<ModID1000000008>();
            RunStatic<ModID998244353>();

            DynamicModInt<ModID998244353>.Mod = 998244353;
            RunDynamic<ModID998244353>();

            DynamicModInt<ModID1000000008>.Mod = 1000000008;
            RunDynamic<ModID1000000008>();

            void RunStatic<T>() where T : struct, IStaticMod
            {
                var mod = (int)new T().Mod;
                var nums = Enumerable.Range(-100, 200).Concat(Enumerable.Range(mod - 100, 200));
                foreach (var n in nums)
                {
                    var s = n.ToString();
                    var expected = (n % mod + mod) % mod;
                    StaticModInt<T>.TryParse(s, out var num1).ShouldBeTrue();
                    var num2 = StaticModInt<T>.Parse(s);

                    num1.Value.ShouldBe(expected);
                    num2.Value.ShouldBe(expected);
                }

                foreach (var n in bigs)
                {
                    var s = n.ToString();
                    var expected = (int)(n % mod + mod) % mod;
                    StaticModInt<T>.TryParse(s, out var num1).ShouldBeTrue();
                    var num2 = StaticModInt<T>.Parse(s);

                    num1.Value.ShouldBe(expected);
                    num2.Value.ShouldBe(expected);
                }

                foreach (var s in invalids)
                {
                    StaticModInt<T>.TryParse(s, out _).ShouldBeFalse();
                    new Action(() => StaticModInt<T>.Parse(s)).ShouldThrow<FormatException>();
                }
            }

            void RunDynamic<T>() where T : struct
            {
                var mod = DynamicModInt<T>.Mod;
                var nums = Enumerable.Range(-100, 200).Concat(Enumerable.Range(mod - 100, 200));
                foreach (var n in nums)
                {
                    var s = n.ToString();
                    var expected = (n % mod + mod) % mod;
                    DynamicModInt<T>.TryParse(s, out var num1).ShouldBeTrue();
                    var num2 = DynamicModInt<T>.Parse(s);

                    num1.Value.ShouldBe(expected);
                    num2.Value.ShouldBe(expected);
                }

                foreach (var n in bigs)
                {
                    var s = n.ToString();
                    var expected = (int)(n % mod + mod) % mod;
                    DynamicModInt<T>.TryParse(s, out var num1).ShouldBeTrue();
                    var num2 = DynamicModInt<T>.Parse(s);

                    num1.Value.ShouldBe(expected);
                    num2.Value.ShouldBe(expected);
                }

                foreach (var s in invalids)
                {
                    DynamicModInt<T>.TryParse(s, out _).ShouldBeFalse();
                    new Action(() => DynamicModInt<T>.Parse(s)).ShouldThrow<FormatException>();
                }
            }
        }


        private struct DynamicUsageID { }
        [Fact]
        public void DynamicUsage()
        {
            DynamicModInt<DynamicUsageID>.Mod = 998244353;
            DynamicModInt<DynamicUsageID>.Mod.ShouldBe(998244353);
            (new DynamicModInt<DynamicUsageID>(1) + new DynamicModInt<DynamicUsageID>(2)).Value.ShouldBe(3);
            (1 + new DynamicModInt<DynamicUsageID>(2)).Value.ShouldBe(3);
            (new DynamicModInt<DynamicUsageID>(1) + 2).Value.ShouldBe(3);

            DynamicModInt<DynamicUsageID>.Mod = 3;
            DynamicModInt<DynamicUsageID>.Mod.ShouldBe(3);
            (new DynamicModInt<DynamicUsageID>(2) - new DynamicModInt<DynamicUsageID>(1)).Value.ShouldBe(1);
            (new DynamicModInt<DynamicUsageID>(1) + new DynamicModInt<DynamicUsageID>(2)).Value.ShouldBe(0);

            DynamicModInt<DynamicUsageID>.Mod = 11;
            DynamicModInt<DynamicUsageID>.Mod.ShouldBe(11);
            (new DynamicModInt<DynamicUsageID>(3) * new DynamicModInt<DynamicUsageID>(5)).Value.ShouldBe(4);

            (+new DynamicModInt<DynamicUsageID>(4)).ShouldBe(new DynamicModInt<DynamicUsageID>(4));
            (-new DynamicModInt<DynamicUsageID>(4)).ShouldBe(new DynamicModInt<DynamicUsageID>(7));

            (new DynamicModInt<DynamicUsageID>(1) == new DynamicModInt<DynamicUsageID>(3)).ShouldBeFalse();
            (new DynamicModInt<DynamicUsageID>(1) != new DynamicModInt<DynamicUsageID>(3)).ShouldBeTrue();
            (new DynamicModInt<DynamicUsageID>(1) == new DynamicModInt<DynamicUsageID>(12)).ShouldBeTrue();
            (new DynamicModInt<DynamicUsageID>(1) != new DynamicModInt<DynamicUsageID>(12)).ShouldBeFalse();

            Should.Throw<ContractAssertException>(() => new DynamicModInt<DynamicUsageID>(3).Pow(-1));
        }

        private readonly struct ConstructorID : IStaticMod
        {
            public uint Mod => 11;
            public bool IsPrime => true;
        }
        [Fact]
        public void Constructor()
        {
            DynamicModInt<ConstructorID>.Mod = 11;
            new DynamicModInt<ConstructorID>(3).Value.ShouldBe(3);
            new DynamicModInt<ConstructorID>(-10).Value.ShouldBe(1);
            (1 + new DynamicModInt<ConstructorID>(1)).Value.ShouldBe(2);
        }

        [Fact]
        public void ConstructorStatic()
        {
            new StaticModInt<ConstructorID>(3).Value.ShouldBe(3);
            new StaticModInt<ConstructorID>(-10).Value.ShouldBe(1);
            (1 + new StaticModInt<ConstructorID>(1)).Value.ShouldBe(2);
        }


        private readonly struct MemoryID : IStaticMod
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
                    .ShouldBe(expected);
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
                    .ShouldBe(expected);
            }
        }

        [Fact]
        public void Plus()
        {
            RunStatic<ModID11>();
            RunStatic<ModID12>();
            RunStatic<ModID1000000007>();
            RunStatic<ModID1000000008>();
            RunStatic<ModID998244353>();

            DynamicModInt<ModID998244353>.Mod = 998244353;
            RunDynamic<ModID998244353>();

            DynamicModInt<ModID1000000008>.Mod = 1000000008;
            RunDynamic<ModID1000000008>();

            void RunStatic<T>() where T : struct, IStaticMod
            {
                var mod = (int)new T().Mod;
                var nums = Enumerable.Range(-100, 200).Concat(Enumerable.Range(mod - 100, 200));
                foreach (var n in nums)
                {
                    int i = (int)InternalMath.SafeMod(n, mod);
                    int x = (+new StaticModInt<T>(i)).Value;
                    x.ShouldBe(i % mod);
                }
            }

            void RunDynamic<T>() where T : struct
            {
                var mod = DynamicModInt<T>.Mod;
                var nums = Enumerable.Range(-100, 200).Concat(Enumerable.Range(mod - 100, 200));
                foreach (var n in nums)
                {
                    int i = (int)InternalMath.SafeMod(n, mod);
                    int x = (+new DynamicModInt<T>(i)).Value;
                    x.ShouldBe(i % mod);
                }
            }
        }

        [Fact]
        public void Minus()
        {
            RunStatic<ModID11>();
            RunStatic<ModID12>();
            RunStatic<ModID1000000007>();
            RunStatic<ModID1000000008>();
            RunStatic<ModID998244353>();

            DynamicModInt<ModID998244353>.Mod = 998244353;
            RunDynamic<ModID998244353>();

            DynamicModInt<ModID1000000008>.Mod = 1000000008;
            RunDynamic<ModID1000000008>();

            void RunStatic<T>() where T : struct, IStaticMod
            {
                var mod = (int)new T().Mod;
                var nums = Enumerable.Range(-100, 200).Concat(Enumerable.Range(mod - 100, 200));
                foreach (var n in nums)
                {
                    int i = (int)InternalMath.SafeMod(n, mod);
                    int x = (-new StaticModInt<T>(i)).Value;
                    x.ShouldBe((mod - i) % mod);
                }
            }

            void RunDynamic<T>() where T : struct
            {
                var mod = DynamicModInt<T>.Mod;
                var nums = Enumerable.Range(-100, 200).Concat(Enumerable.Range(mod - 100, 200));
                foreach (var n in nums)
                {
                    int i = (int)InternalMath.SafeMod(n, mod);
                    int x = (-new DynamicModInt<T>(i)).Value;
                    x.ShouldBe((mod - i) % mod);
                }
            }
        }

#if GENERIC_MATH
        [Fact]
        public void ConvertFrom()
        {
            const int size = Global.IsCi ? 100000 : 5000;

            RunStatic<ModID11>();
            RunStatic<ModID12>();
            RunStatic<ModID1000000007>();
            RunStatic<ModID1000000008>();
            RunStatic<ModID998244353>();

            DynamicModInt<ModID998244353>.Mod = 998244353;
            RunDynamic<ModID998244353>();

            DynamicModInt<ModID1000000008>.Mod = 1000000008;
            RunDynamic<ModID1000000008>();

            void RunStatic<T>() where T : struct, IStaticMod
            {
                var mod = (int)new T().Mod;
                var max = System.Math.Min(size, mod);
                for (int i = 0; i < max; i++)
                {
                    int x = ConvertFrom<StaticModInt<T>, int>(i).Value;
                    x.ShouldBe(i % mod);

                    x = ConvertFrom<StaticModInt<T>, long>(i).Value;
                    x.ShouldBe(i % mod);

                    x = ConvertFrom<StaticModInt<T>, uint>((uint)i).Value;
                    x.ShouldBe(i % mod);

                    x = ConvertFrom<StaticModInt<T>, ulong>((ulong)i).Value;
                    x.ShouldBe(i % mod);
                }
            }

            void RunDynamic<T>() where T : struct
            {
                var mod = DynamicModInt<T>.Mod;
                var max = System.Math.Min(size, mod);
                for (int i = 0; i < max; i++)
                {
                    int x = ConvertFrom<DynamicModInt<T>, int>(i).Value;
                    x.ShouldBe(i % mod);

                    x = ConvertFrom<DynamicModInt<T>, long>(i).Value;
                    x.ShouldBe(i % mod);

                    x = ConvertFrom<DynamicModInt<T>, uint>((uint)i).Value;
                    x.ShouldBe(i % mod);

                    x = ConvertFrom<DynamicModInt<T>, ulong>((ulong)i).Value;
                    x.ShouldBe(i % mod);
                }
            }

            TModInt ConvertFrom<TModInt, TOther>(TOther v) where TModInt : INumberBase<TModInt> where TOther : INumberBase<TOther>
                => TModInt.CreateChecked(v);
        }

        [Fact]
        public void ConvertTo()
        {
            const int size = Global.IsCi ? 100000 : 5000;

            RunStatic<ModID11>();
            RunStatic<ModID12>();
            RunStatic<ModID1000000007>();
            RunStatic<ModID1000000008>();
            RunStatic<ModID998244353>();

            DynamicModInt<ModID998244353>.Mod = 998244353;
            RunDynamic<ModID998244353>();

            DynamicModInt<ModID1000000008>.Mod = 1000000008;
            RunDynamic<ModID1000000008>();

            void RunStatic<T>() where T : struct, IStaticMod
            {
                var mod = (int)new T().Mod;
                var max = System.Math.Min(size, mod);
                for (int i = 0; i < max; i++)
                {
                    {
                        var x = ConvertTo<StaticModInt<T>, int>(i);
                        x.ShouldBe((int)(i % mod));
                    }
                    {
                        var x = ConvertTo<StaticModInt<T>, long>(i);
                        x.ShouldBe((long)(i % mod));
                    }
                    {
                        var x = ConvertTo<StaticModInt<T>, uint>(i);
                        x.ShouldBe((uint)(i % mod));
                    }
                    {
                        var x = ConvertTo<StaticModInt<T>, ulong>(i);
                        x.ShouldBe((ulong)(i % mod));
                    }
                }
            }

            void RunDynamic<T>() where T : struct
            {
                var mod = DynamicModInt<T>.Mod;
                var max = System.Math.Min(size, mod);
                for (int i = 0; i < max; i++)
                {
                    {
                        var x = ConvertTo<DynamicModInt<T>, int>(i);
                        x.ShouldBe((int)(i % mod));
                    }
                    {
                        var x = ConvertTo<DynamicModInt<T>, long>(i);
                        x.ShouldBe((long)(i % mod));
                    }
                    {
                        var x = ConvertTo<DynamicModInt<T>, uint>(i);
                        x.ShouldBe((uint)(i % mod));
                    }
                    {
                        var x = ConvertTo<DynamicModInt<T>, ulong>(i);
                        x.ShouldBe((ulong)(i % mod));
                    }
                }
            }

            TOther ConvertTo<TModInt, TOther>(TModInt v) where TModInt : INumberBase<TModInt> where TOther : INumberBase<TOther>
                => TOther.CreateChecked(v);
        }
#endif
    }
}
