using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AtCoder.Internal
{
    public static partial class InternalMath
    {
        private static readonly Dictionary<int, int> primitiveRootsCache = new Dictionary<int, int>()
        {
            { 2, 1 },
            { 167772161, 3 },
            { 469762049, 3 },
            { 754974721, 11 },
            { 998244353, 3 }
        };

        /// <summary>
        /// <paramref name="m"/> の最小の原始根を求めます。
        /// </summary>
        /// <remarks>
        /// 制約: <paramref name="m"/> は素数
        /// </remarks>
        public static int PrimitiveRoot(int m)
        {
            Debug.Assert(m >= 2);

            if (primitiveRootsCache.TryGetValue(m, out var p))
            {
                return p;
            }

            return primitiveRootsCache[m] = Calculate(m);

            int Calculate(int m)
            {
                Span<int> divs = stackalloc int[20];
                divs[0] = 2;
                int cnt = 1;
                int x = (m - 1) / 2;

                while (x % 2 == 0)
                {
                    x >>= 1;
                }

                for (int i = 3; (long)i * i <= x; i += 2)
                {
                    if (x % i == 0)
                    {
                        divs[cnt++] = i;
                        while (x % i == 0)
                        {
                            x /= i;
                        }
                    }
                }

                if (x > 1)
                {
                    divs[cnt++] = x;
                }

                for (int g = 2; ; g++)
                {
                    bool ok = true;
                    for (int i = 0; i < cnt; i++)
                    {
                        if (Math.PowMod(g, (m - 1) / divs[i], m) == 1)
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (ok)
                    {
                        return g;
                    }
                }
            }
        }
    }
}
