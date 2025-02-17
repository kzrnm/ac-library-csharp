using System;
using System.Linq;
using Shouldly;
using MersenneTwister;
using Xunit;

namespace AtCoder
{
    public class ConvolutionTest
    {
        private readonly struct Mod924844033 : IStaticMod
        {
            public uint Mod => 924844033;
            public bool IsPrime => true;
        }
        private readonly struct Mod641 : IStaticMod
        {
            public uint Mod => 641;
            public bool IsPrime => true;
        }
        private readonly struct Mod18433 : IStaticMod
        {
            public uint Mod => 18433;
            public bool IsPrime => true;
        }

        #region Native
        static long[] ConvLongNative(long[] a, long[] b)
        {
            int n = a.Length, m = b.Length;
            var c = new long[n + m - 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    c[i + j] += a[i] * b[j];
                }
            }
            return c;
        }
        static StaticModInt<T>[] ConvNative<T>(StaticModInt<T>[] a, StaticModInt<T>[] b) where T : struct, IStaticMod
        {
            int n = a.Length, m = b.Length;
            var c = new StaticModInt<T>[n + m - 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    c[i + j] += a[i] * b[j];
                }
            }
            return c;
        }

        static long[] ConvNative<T>(long[] a, long[] b) where T : struct, IStaticMod
        {
            int n = a.Length, m = b.Length;
            var c = new long[n + m - 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    c[i + j] += a[i] * b[j] % default(T).Mod;
                    if (c[i + j] >= default(T).Mod) c[i + j] -= default(T).Mod;
                }
            }
            return c;
        }
        static int[] ConvNative<T>(int[] a, int[] b) where T : struct, IStaticMod
            => ConvNative<T>(a.Select(n => (long)n).ToArray(), b.Select(n => (long)n).ToArray()).Select(n => (int)n).ToArray();
        static uint[] ConvNative<T>(uint[] a, uint[] b) where T : struct, IStaticMod
            => ConvNative<T>(a.Select(n => (long)n).ToArray(), b.Select(n => (long)n).ToArray()).Select(n => (uint)n).ToArray();
        static ulong[] ConvNative<T>(ulong[] a, ulong[] b) where T : struct, IStaticMod
             => ConvNative<T>(a.Select(n => (long)n).ToArray(), b.Select(n => (long)n).ToArray()).Select(n => (ulong)n).ToArray();
        #endregion Native

        public static TheoryData EmptyIntTestData => new TheoryData<int[], int[], int[]>
        {
            { [], [], [] },
            { [], [1,2], [] },
            { [1,2], [], [] },
            { [1], [], [] },
        };

        [Theory]
        [Trait("Category", "Empty")]
        [MemberData(nameof(EmptyIntTestData))]
        public void EmptyInt(int[] a, int[] b, int[] expected)
        {
            MathLib.Convolution(a, b).ShouldBe(expected);
        }
        public static TheoryData EmptyLongTestData => new TheoryData<long[], long[], long[]>
        {
            { Array.Empty<long>(), Array.Empty<long>(), Array.Empty<long>() },
            { Array.Empty<long>(), new long[]{ 1, 2 }, Array.Empty<long>() },
        };
        [Theory]
        [Trait("Category", "Empty")]
        [MemberData(nameof(EmptyLongTestData))]
        public void EmptyLong(long[] a, long[] b, long[] expected)
        {
            MathLib.Convolution(a, b).ShouldBe(expected);
        }
        public static TheoryData EmptyModIntTestData => new TheoryData<StaticModInt<Mod998244353>[], StaticModInt<Mod998244353>[], StaticModInt<Mod998244353>[]>
        {
            { Array.Empty<StaticModInt<Mod998244353>>(), Array.Empty<StaticModInt<Mod998244353>>(), Array.Empty<StaticModInt<Mod998244353>>() },
            { Array.Empty<StaticModInt<Mod998244353>>(), new StaticModInt<Mod998244353>[]{ 1, 2 }, Array.Empty<StaticModInt<Mod998244353>>() },
        };
        [Theory]
        [Trait("Category", "Empty")]
        [MemberData(nameof(EmptyModIntTestData))]
        public void EmptyModInt(StaticModInt<Mod998244353>[] a, StaticModInt<Mod998244353>[] b, StaticModInt<Mod998244353>[] expected)
        {
            MathLib.Convolution(a, b).ShouldBe(expected);
        }

        [Fact]
        public void Mid()
        {
            var mt = MTRandom.Create();
            int n = 1234, m = 2345;
            var a = new StaticModInt<Mod998244353>[n];
            var b = new StaticModInt<Mod998244353>[m];
            for (int i = 0; i < n; i++)
            {
                a[i] = mt.NextUInt();
            }
            for (int i = 0; i < m; i++)
            {
                b[i] = mt.NextUInt();
            }
            MathLib.Convolution(a, b).ShouldBe(ConvNative(a, b));
        }

        #region Simple
        [Fact]
        public void SimpleSMod()
        {
            var mt = MTRandom.Create();
            for (int n = 1; n < 20; n++)
            {
                for (int m = 1; m < 20; m++)
                {
                    var a = new StaticModInt<Mod998244353>[n];
                    var b = new StaticModInt<Mod998244353>[m];

                    for (int i = 0; i < n; i++)
                    {
                        a[i] = mt.NextUInt();
                    }
                    for (int i = 0; i < m; i++)
                    {
                        b[i] = mt.NextUInt();
                    }
                    MathLib.Convolution(a, b).ShouldBe(ConvNative(a, b));
                }
            }
            for (int n = 1; n < 20; n++)
            {
                for (int m = 1; m < 20; m++)
                {
                    var a = new StaticModInt<Mod924844033>[n];
                    var b = new StaticModInt<Mod924844033>[m];

                    for (int i = 0; i < n; i++)
                    {
                        a[i] = mt.NextUInt();
                    }
                    for (int i = 0; i < m; i++)
                    {
                        b[i] = mt.NextUInt();
                    }
                    MathLib.Convolution(a, b).ShouldBe(ConvNative(a, b));
                }
            }
        }

        [Fact]
        public void SimpleInt()
        {
            var mt = MTRandom.Create();
            for (int n = 1; n < 20; n++)
            {
                for (int m = 1; m < 20; m++)
                {
                    var a = new int[n];
                    var b = new int[m];

                    for (int i = 0; i < n; i++)
                    {
                        a[i] = (int)(mt.NextUInt() % default(Mod998244353).Mod);
                    }
                    for (int i = 0; i < m; i++)
                    {
                        b[i] = (int)(mt.NextUInt() % default(Mod998244353).Mod);
                    }
                    MathLib.Convolution<Mod998244353>(a, b).ShouldBe(ConvNative<Mod998244353>(a, b));
                }
            }
            for (int n = 1; n < 20; n++)
            {
                for (int m = 1; m < 20; m++)
                {
                    var a = new int[n];
                    var b = new int[m];

                    for (int i = 0; i < n; i++)
                    {
                        a[i] = (int)(mt.NextUInt() % default(Mod924844033).Mod);
                    }
                    for (int i = 0; i < m; i++)
                    {
                        b[i] = (int)(mt.NextUInt() % default(Mod924844033).Mod);
                    }
                    MathLib.Convolution<Mod924844033>(a, b).ShouldBe(ConvNative<Mod924844033>(a, b));
                }
            }
        }
        [Fact]
        public void SimpleUInt()
        {
            var mt = MTRandom.Create();
            for (int n = 1; n < 20; n++)
            {
                for (int m = 1; m < 20; m++)
                {
                    var a = new uint[n];
                    var b = new uint[m];

                    for (int i = 0; i < n; i++)
                    {
                        a[i] = mt.NextUInt() % default(Mod998244353).Mod;
                    }
                    for (int i = 0; i < m; i++)
                    {
                        b[i] = mt.NextUInt() % default(Mod998244353).Mod;
                    }
                    MathLib.Convolution<Mod998244353>(a, b).ShouldBe(ConvNative<Mod998244353>(a, b));
                }
            }
            for (int n = 1; n < 20; n++)
            {
                for (int m = 1; m < 20; m++)
                {
                    var a = new uint[n];
                    var b = new uint[m];

                    for (int i = 0; i < n; i++)
                    {
                        a[i] = mt.NextUInt() % default(Mod924844033).Mod;
                    }
                    for (int i = 0; i < m; i++)
                    {
                        b[i] = mt.NextUInt() % default(Mod924844033).Mod;
                    }
                    MathLib.Convolution<Mod924844033>(a, b).ShouldBe(ConvNative<Mod924844033>(a, b));
                }
            }
        }

        [Fact]
        public void SimpleLong()
        {
            var mt = MTRandom.Create();
            for (int n = 1; n < 20; n++)
            {
                for (int m = 1; m < 20; m++)
                {
                    var a = new long[n];
                    var b = new long[m];

                    for (int i = 0; i < n; i++)
                    {
                        a[i] = mt.NextUInt() % default(Mod998244353).Mod;
                    }
                    for (int i = 0; i < m; i++)
                    {
                        b[i] = mt.NextUInt() % default(Mod998244353).Mod;
                    }
                    MathLib.Convolution<Mod998244353>(a, b).ShouldBe(ConvNative<Mod998244353>(a, b));
                }
            }
            for (int n = 1; n < 20; n++)
            {
                for (int m = 1; m < 20; m++)
                {
                    var a = new long[n];
                    var b = new long[m];

                    for (int i = 0; i < n; i++)
                    {
                        a[i] = mt.NextUInt() % default(Mod924844033).Mod;
                    }
                    for (int i = 0; i < m; i++)
                    {
                        b[i] = mt.NextUInt() % default(Mod924844033).Mod;
                    }
                    MathLib.Convolution<Mod924844033>(a, b).ShouldBe(ConvNative<Mod924844033>(a, b));
                }
            }
        }


        [Fact]
        public void SimpleULong()
        {
            var mt = MTRandom.Create();
            for (int n = 1; n < 20; n++)
            {
                for (int m = 1; m < 20; m++)
                {
                    var a = new ulong[n];
                    var b = new ulong[m];

                    for (int i = 0; i < n; i++)
                    {
                        a[i] = mt.NextUInt() % default(Mod998244353).Mod;
                    }
                    for (int i = 0; i < m; i++)
                    {
                        b[i] = mt.NextUInt() % default(Mod998244353).Mod;
                    }
                    MathLib.Convolution<Mod998244353>(a, b).ShouldBe(ConvNative<Mod998244353>(a, b));
                }
            }
            for (int n = 1; n < 20; n++)
            {
                for (int m = 1; m < 20; m++)
                {
                    var a = new ulong[n];
                    var b = new ulong[m];

                    for (int i = 0; i < n; i++)
                    {
                        a[i] = mt.NextUInt() % default(Mod924844033).Mod;
                    }
                    for (int i = 0; i < m; i++)
                    {
                        b[i] = mt.NextUInt() % default(Mod924844033).Mod;
                    }
                    MathLib.Convolution<Mod924844033>(a, b).ShouldBe(ConvNative<Mod924844033>(a, b));
                }
            }
        }
        #endregion Simple

        [Fact]
        public void ConvLong()
        {
            var mt = MTRandom.Create();
            for (int n = 1; n < 20; n++)
            {
                for (int m = 1; m < 20; m++)
                {
                    var a = new long[n];
                    var b = new long[m];
                    for (int i = 0; i < n; i++)
                    {
                        a[i] = (long)(mt.NextUInt() % 1_000_000) - 500_000;
                    }
                    for (int i = 0; i < m; i++)
                    {
                        b[i] = (long)(mt.NextUInt() % 1_000_000) - 500_000;
                    }
                    MathLib.ConvolutionLong(a, b).ShouldBe(ConvLongNative(a, b));
                }
            }
        }

        [Fact]
        public void ConvLongBound()
        {
            const ulong MOD1 = 469762049;  // 2^26
            const ulong MOD2 = 167772161;  // 2^25
            const ulong MOD3 = 754974721;  // 2^24
            const ulong M2M3 = MOD2 * MOD3;
            const ulong M1M3 = MOD1 * MOD3;
            const ulong M1M2 = MOD1 * MOD2;
            for (int i = -1000; i <= 1000; i++)
            {
                var a = new long[] { unchecked((long)(0UL - M1M2 - M1M3 - M2M3) + i) };
                var b = new long[] { 1 };
                MathLib.ConvolutionLong(a, b).ShouldBe(a);
            }
            for (int i = 0; i < 1000; i++)
            {
                var a = new long[] { long.MinValue + i };
                var b = new long[] { 1 };
                MathLib.ConvolutionLong(a, b).ShouldBe(a);
            }
            for (int i = 0; i < 1000; i++)
            {
                var a = new long[] { long.MaxValue - i };
                var b = new long[] { 1 };
                MathLib.ConvolutionLong(a, b).ShouldBe(a);
            }
        }

        [Fact]
        public void Conv641()
        {
            // 641 = 128 * 5 + 1
            var mt = MTRandom.Create();
            var a = new long[64];
            var b = new long[65];
            for (int i = 0; i < 64; i++)
            {
                a[i] = mt.Next(0, (int)default(Mod641).Mod);
            }
            for (int i = 0; i < 65; i++)
            {
                b[i] = mt.Next(0, (int)default(Mod641).Mod);
            }

            MathLib.Convolution<Mod641>(a, b).ShouldBe(ConvNative<Mod641>(a, b));
        }
        [Fact]
        public void Conv18433()
        {
            // 18433 = 2048 * 9 + 1
            var mt = MTRandom.Create();
            var a = new long[1024];
            var b = new long[1025];
            for (int i = 0; i < 1024; i++)
            {
                a[i] = mt.Next(0, (int)default(Mod18433).Mod);
            }
            for (int i = 0; i < 1025; i++)
            {
                b[i] = mt.Next(0, (int)default(Mod18433).Mod);
            }

            MathLib.Convolution<Mod18433>(a, b).ShouldBe(ConvNative<Mod18433>(a, b));
        }
    }
}
