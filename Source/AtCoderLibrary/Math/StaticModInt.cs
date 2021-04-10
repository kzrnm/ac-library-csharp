using System;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    using static MethodImplOptions;
    /// <summary>
    /// コンパイル時に決定する mod を表します。
    /// </summary>
    /// <example>
    /// <code>
    /// public readonly struct Mod1000000009 : IStaticMod
    /// {
    ///     public uint Mod => 1000000009;
    ///     public bool IsPrime => true;
    /// }
    /// </code>
    /// </example>
    [IsOperator]
    public interface IStaticMod
    {
        /// <summary>
        /// mod を取得します。
        /// </summary>
        uint Mod { get; }

        /// <summary>
        /// mod が素数であるか識別します。
        /// </summary>
        bool IsPrime { get; }
    }
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct Mod1000000007 : IStaticMod
    {
        public uint Mod => 1000000007;
        public bool IsPrime => true;
    }

    public readonly struct Mod998244353 : IStaticMod
    {
        public uint Mod => 998244353;
        public bool IsPrime => true;
    }

    public readonly struct StaticModIntOperator<T> : IArithmeticOperator<StaticModInt<T>>
        where T : struct, IStaticMod
    {
        public StaticModInt<T> MultiplyIdentity => StaticModInt<T>.Raw(1);
        [MethodImpl(AggressiveInlining)]
        public StaticModInt<T> Add(StaticModInt<T> x, StaticModInt<T> y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public StaticModInt<T> Subtract(StaticModInt<T> x, StaticModInt<T> y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public StaticModInt<T> Multiply(StaticModInt<T> x, StaticModInt<T> y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public StaticModInt<T> Divide(StaticModInt<T> x, StaticModInt<T> y) => x / y;
        [MethodImpl(AggressiveInlining)]
        public StaticModInt<T> Modulo(StaticModInt<T> x, StaticModInt<T> y) => throw new NotSupportedException();
        [MethodImpl(AggressiveInlining)]
        public StaticModInt<T> Minus(StaticModInt<T> x) => -x;
        [MethodImpl(AggressiveInlining)]
        public StaticModInt<T> Increment(StaticModInt<T> x) => ++x;
        [MethodImpl(AggressiveInlining)]
        public StaticModInt<T> Decrement(StaticModInt<T> x) => --x;
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types

    /// <summary>
    /// 四則演算時に自動で mod を取る整数型。mod の値はコンパイル時に決定している必要があります。
    /// </summary>
    /// <typeparam name="T">定数 mod を表す構造体</typeparam>
    /// <example>
    /// <code>
    /// using ModInt = AtCoder.StaticModInt&lt;AtCoder.Mod1000000007&gt;;
    ///
    /// void SomeMethod()
    /// {
    ///     var m = new ModInt(1);
    ///     m -= 2;
    ///     Console.WriteLine(m);   // 1000000006
    /// }
    /// </code>
    /// </example>
    public readonly struct StaticModInt<T> : IEquatable<StaticModInt<T>> where T : struct, IStaticMod
    {
        internal readonly uint _v;
        private static readonly T op = default;

        /// <summary>
        /// 格納されている値を返します。
        /// </summary>
        public int Value => (int)_v;

        /// <summary>
        /// mod を返します。
        /// </summary>
        public static int Mod => (int)op.Mod;

        /// <summary>
        /// <paramref name="v"/> に対して mod を取らずに StaticModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <para>定数倍高速化のための関数です。 <paramref name="v"/> に 0 未満または mod 以上の値を入れた場合の挙動は未定義です。</para>
        /// <para>制約: 0≤|<paramref name="v"/>|&lt;mod</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static StaticModInt<T> Raw(int v)
        {
            var u = unchecked((uint)v);
            Contract.Assert(u < Mod, $"{nameof(u)} must be less than {nameof(Mod)}.");
            return new StaticModInt<T>(u);
        }

        /// <summary>
        /// StaticModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <paramref name="v"/>が 0 未満、もしくは mod 以上の場合、自動で mod を取ります。
        /// </remarks>
        public StaticModInt(long v) : this(Round(v)) { }

        /// <summary>
        /// StaticModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <paramref name="v"/>が mod 以上の場合、自動で mod を取ります。
        /// </remarks>
        public StaticModInt(ulong v) : this((uint)(v % op.Mod)) { }

        private StaticModInt(uint v) => _v = v;

        [MethodImpl(AggressiveInlining)]
        private static uint Round(long v)
        {
            var x = v % op.Mod;
            if (x < 0)
            {
                x += op.Mod;
            }
            return (uint)x;
        }

        [MethodImpl(AggressiveInlining)]
        public static StaticModInt<T> operator ++(StaticModInt<T> value)
        {
            var v = value._v + 1;
            if (v == op.Mod)
            {
                v = 0;
            }
            return new StaticModInt<T>(v);
        }

        [MethodImpl(AggressiveInlining)]
        public static StaticModInt<T> operator --(StaticModInt<T> value)
        {
            var v = value._v;
            if (v == 0)
            {
                v = op.Mod;
            }
            return new StaticModInt<T>(v - 1);
        }

        [MethodImpl(AggressiveInlining)]
        public static StaticModInt<T> operator +(StaticModInt<T> lhs, StaticModInt<T> rhs)
        {
            var v = lhs._v + rhs._v;
            if (v >= op.Mod)
            {
                v -= op.Mod;
            }
            return new StaticModInt<T>(v);
        }

        [MethodImpl(AggressiveInlining)]
        public static StaticModInt<T> operator -(StaticModInt<T> lhs, StaticModInt<T> rhs)
        {
            unchecked
            {
                var v = lhs._v - rhs._v;
                if (v >= op.Mod)
                {
                    v += op.Mod;
                }
                return new StaticModInt<T>(v);
            }
        }

        [MethodImpl(AggressiveInlining)]
        public static StaticModInt<T> operator *(StaticModInt<T> lhs, StaticModInt<T> rhs) => new StaticModInt<T>((uint)((ulong)lhs._v * rhs._v % op.Mod));
        /// <summary>
        /// 除算を行います。
        /// </summary>
        /// <remarks>
        /// <para>- 制約: <paramref name="rhs"/> に乗法の逆元が存在する。（gcd(<paramref name="rhs"/>, mod) = 1）</para>
        /// <para>- 計算量: O(log(mod))</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public static StaticModInt<T> operator /(StaticModInt<T> lhs, StaticModInt<T> rhs) => lhs * rhs.Inv();
        [MethodImpl(AggressiveInlining)]
        public static StaticModInt<T> operator +(StaticModInt<T> value) => value;
        [MethodImpl(AggressiveInlining)]
        public static StaticModInt<T> operator -(StaticModInt<T> value) => new StaticModInt<T>(op.Mod - value._v);
        [MethodImpl(AggressiveInlining)]
        public static bool operator ==(StaticModInt<T> lhs, StaticModInt<T> rhs) => lhs._v == rhs._v;
        [MethodImpl(AggressiveInlining)]
        public static bool operator !=(StaticModInt<T> lhs, StaticModInt<T> rhs) => lhs._v != rhs._v;
        [MethodImpl(AggressiveInlining)]
        public static implicit operator StaticModInt<T>(int value) => new StaticModInt<T>(value);
        [MethodImpl(AggressiveInlining)]
        public static implicit operator StaticModInt<T>(long value) => new StaticModInt<T>(value);

        /// <summary>
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        public StaticModInt<T> Pow(long n)
        {
            Contract.Assert(0 <= n, $"{nameof(n)} must be positive.");
            var x = this;
            var r = new StaticModInt<T>(1U);

            while (n > 0)
            {
                if ((n & 1) > 0)
                {
                    r *= x;
                }
                x *= x;
                n >>= 1;
            }

            return r;
        }

        /// <summary>
        /// 自身を x として、 xy≡1 なる y を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: gcd(x, mod) = 1</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public StaticModInt<T> Inv()
        {
            if (op.IsPrime)
            {
                Contract.Assert(_v > 0, reason: $"{nameof(Value)} must be positive.");
                return Pow(op.Mod - 2);
            }
            else
            {
                var (g, x) = InternalMath.InvGCD(_v, op.Mod);
                Contract.Assert(g == 1, reason: $"gcd({nameof(x)}, {nameof(Mod)}) must be 1.");
                return new StaticModInt<T>(x);
            }
        }

        public override string ToString() => _v.ToString();
        public override bool Equals(object obj) => obj is StaticModInt<T> m && Equals(m);
        public bool Equals(StaticModInt<T> other) => _v == other._v;
        public override int GetHashCode() => _v.GetHashCode();
    }
}
