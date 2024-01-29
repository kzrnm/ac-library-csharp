using System.Runtime.CompilerServices;

namespace AtCoder.Internal
{
    /// <summary>
    /// Mod 関連の計算
    /// </summary>
    public static class ModCalc
    {
        /// <summary>
        /// <paramref name="x"/>y≡1(mod <paramref name="m"/>) なる y のうち、0≤y&lt;<paramref name="m"/> を満たすものを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: gcd(<paramref name="x"/>,<paramref name="m"/>)=1, 1≤<paramref name="m"/></para>
        /// <para>計算量: O(log<paramref name="m"/>)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static long InvMod(long x, long m)
        {
            Contract.Assert(1 <= m, reason: $"1 <= {nameof(m)}");
            var (g, res) = InvGcd(x, m);
            Contract.Assert(g == 1, reason: $"gcd({nameof(x)}, {nameof(m)}) must be 1.");
            return res;
        }

        /// <summary>
        /// g=gcd(a,b),xa=g(mod b) となるような 0≤x&lt;b/g の(g, x)
        /// </summary>
        /// <remarks>
        /// <para>制約: 1≤<paramref name="b"/></para>
        /// </remarks>
        [MethodImpl(256)]
        public static (long, long) InvGcd(long a, long b)
        {
            a = SafeMod(a, b);
            if (a == 0) return (b, 0);

            long s = b, t = a;
            long m0 = 0, m1 = 1;

            long u;
            while (true)
            {
                if (t == 0)
                {
                    if (m0 < 0) m0 += b / s;
                    return (s, m0);
                }
                u = s / t;
                s -= t * u;
                m0 -= m1 * u;

                if (s == 0)
                {
                    if (m1 < 0) m1 += b / t;
                    return (t, m1);
                }
                u = t / s;
                t -= s * u;
                m1 -= m0 * u;
            }
        }

        [MethodImpl(256)]
        public static long SafeMod(long x, long m)
        {
            x %= m;
            if (x < 0) x += m;
            return x;
        }

        /// <summary>
        /// <paramref name="x"/>^<paramref name="n"/> mod <paramref name="m"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>, 1≤<paramref name="m"/></para>
        /// <para>計算量: O(log<paramref name="n"/>)</para>
        /// </remarks>
        [MethodImpl(256)]
        public static uint PowMod(long x, long n, int m)
        {
            Contract.Assert(0 <= n && 1 <= m, reason: $"0 <= {nameof(n)} && 1 <= {nameof(m)}");
            if (m == 1) return 0;
            return new Barrett((uint)m).Pow(x, n);
        }

        /// <summary>
        /// 同じ長さ n の配列 <paramref name="r"/>, <paramref name="m"/> について、x≡<paramref name="r"/>[i] (mod <paramref name="m"/>[i]),∀i∈{0,1,⋯,n−1} を解きます。
        /// </summary>
        /// <remarks>
        /// <para>制約: |<paramref name="r"/>|=|<paramref name="m"/>|, 1≤<paramref name="m"/>[i], lcm(m[i]) が ll に収まる</para>
        /// <para>計算量: O(nloglcm(<paramref name="m"/>))</para>
        /// </remarks>
        /// <returns>答えは(存在するならば) y,z(0≤y&lt;z=lcm(<paramref name="m"/>[i])) を用いて x≡y(mod z) の形で書ける。答えがない場合は(0,0)、n=0 の時は(0,1)、それ以外の場合は(y,z)。</returns>
        [MethodImpl(256)]
        public static (long y, long z) Crt(long[] r, long[] m)
        {
            Contract.Assert(r.Length == m.Length, reason: $"Length of {nameof(r)} and {nameof(m)} must be same.");

            long r0 = 0, m0 = 1;
            for (int i = 0; i < m.Length; i++)
            {
                Contract.Assert(1 <= m[i], reason: $"All of {nameof(m)} must be greater or equal 1.");
                long r1 = SafeMod(r[i], m[i]);
                long m1 = m[i];
                if (m0 < m1)
                {
                    (r0, r1) = (r1, r0);
                    (m0, m1) = (m1, m0);
                }
                if (m0 % m1 == 0)
                {
                    if (r0 % m1 != r1) return (0, 0);
                    continue;
                }
                var (g, im) = InvGcd(m0, m1);

                long u1 = (m1 / g);
                if ((r1 - r0) % g != 0) return (0, 0);

                long x = (r1 - r0) / g % u1 * im % u1;
                r0 += x * m0;
                m0 *= u1;
                if (r0 < 0) r0 += m0;
            }
            return (r0, m0);
        }
    }
}
