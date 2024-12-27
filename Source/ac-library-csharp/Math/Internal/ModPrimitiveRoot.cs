using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#if NETCOREAPP3_1_OR_GREATER
using System.Numerics;
#endif
#if NET7_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace AtCoder.Internal
{
    public static class ModPrimitiveRoot
    {
        private static Dictionary<uint, int> primitiveRootsCache;

        /// <summary>
        /// <typeparamref name="TMod"/> の最小の原始根を求めます。
        /// </summary>
        /// <remarks>
        /// 制約: <typeparamref name="TMod"/> は素数
        /// </remarks>
        [MethodImpl(256)]
        public static int PrimitiveRoot<TMod>() where TMod : struct, IStaticMod
        {
            uint m = default(TMod).Mod;
            Contract.Assert(m >= 2, reason: $"{nameof(m)} must be greater or equal 2");
            Contract.Assert(default(TMod).IsPrime, reason: $"{nameof(m)} must be prime number");

            switch (m)
            {
                case 2: return 1;
                case 167772161: return 3;
                case 469762049: return 3;
                case 754974721: return 11;
                case 998244353: return 3;
            }

#if NETCOREAPP3_1_OR_GREATER
            primitiveRootsCache ??= new Dictionary<uint, int>();
#else
            if (primitiveRootsCache == null)
                primitiveRootsCache = new Dictionary<uint, int>();
#endif

#if NET7_0_OR_GREATER
            ref var result = ref CollectionsMarshal.GetValueRefOrAddDefault(primitiveRootsCache, m, out var exists);
            if (!exists)
            {
                result = PrimitiveRootCalculate<TMod>();
            }
            return result;
#else
            if (primitiveRootsCache.TryGetValue(m, out var p))
            {
                return p;
            }

            return primitiveRootsCache[m] = PrimitiveRootCalculate<TMod>();
#endif
        }
        static int PrimitiveRootCalculate<TMod>() where TMod : struct, IStaticMod
        {
            var m = default(TMod).Mod;
            Span<uint> divs = stackalloc uint[20];
            divs[0] = 2;
            int cnt = 1;
            var x = m - 1;
            x >>= BitOperations.TrailingZeroCount(x);

            for (uint i = 3; (long)i * i <= x; i += 2)
            {
                if (x % i == 0)
                {
                    divs[cnt++] = i;
                    do
                    {
                        x /= i;
                    } while (x % i == 0);
                }
            }

            if (x > 1)
            {
                divs[cnt++] = x;
            }
            divs = divs.Slice(0, cnt);

            for (int g = 2; ; g++)
            {
                foreach (var d in divs)
                    if (new StaticModInt<TMod>(g).Pow((m - 1) / d).Value == 1)
                        goto NEXT;
                return g;
            NEXT:;
            }
        }
    }
}
