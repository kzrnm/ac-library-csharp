using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
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
    public readonly struct StaticModInt<T>
     : IEquatable<StaticModInt<T>>, IFormattable
#if GENERIC_MATH
     , INumberBase<StaticModInt<T>>
#endif
     where T : struct, IStaticMod
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
        public static StaticModInt<T> Zero => default;
        public static StaticModInt<T> One => new StaticModInt<T>(1u);

        /// <summary>
        /// <paramref name="v"/> に対して mod を取らずに StaticModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <para>定数倍高速化のための関数です。 <paramref name="v"/> に 0 未満または mod 以上の値を入れた場合の挙動は未定義です。</para>
        /// <para>制約: 0≤|<paramref name="v"/>|&lt;mod</para>
        /// </remarks>
        [MethodImpl(256)]
        public static StaticModInt<T> Raw(int v)
        {
            var u = unchecked((uint)v);
            //Contract.Assert(u < Mod, $"{nameof(u)} must be less than {nameof(Mod)}.");
            return new StaticModInt<T>(u);
        }

        /// <summary>
        /// StaticModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <paramref name="v"/>が 0 未満、もしくは mod 以上の場合、自動で mod を取ります。
        /// </remarks>
        [MethodImpl(256)]
        public StaticModInt(long v) : this(Round(v)) { }

        /// <summary>
        /// StaticModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <paramref name="v"/>が mod 以上の場合、自動で mod を取ります。
        /// </remarks>
        [MethodImpl(256)]
        public StaticModInt(ulong v) : this((uint)(v % op.Mod)) { }

        [MethodImpl(256)]
        private StaticModInt(uint v) => _v = v;

        [MethodImpl(256)]
        private static uint Round(long v)
        {
            var x = v % op.Mod;
            if (x < 0)
            {
                x += op.Mod;
            }
            return (uint)x;
        }

        [MethodImpl(256)]
        public static StaticModInt<T> operator ++(StaticModInt<T> v)
        {
            var x = v._v + 1;
            if (x == op.Mod)
            {
                x = 0;
            }
            return new StaticModInt<T>(x);
        }

        [MethodImpl(256)]
        public static StaticModInt<T> operator --(StaticModInt<T> v)
        {
            var x = v._v;
            if (x == 0)
            {
                x = op.Mod;
            }
            return new StaticModInt<T>(x - 1);
        }

        [MethodImpl(256)]
        public static StaticModInt<T> operator +(StaticModInt<T> lhs, StaticModInt<T> rhs)
        {
            var v = lhs._v + rhs._v;
            if (v >= op.Mod)
            {
                v -= op.Mod;
            }
            return new StaticModInt<T>(v);
        }

        [MethodImpl(256)]
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

        [MethodImpl(256)]
        public static StaticModInt<T> operator *(StaticModInt<T> lhs, StaticModInt<T> rhs) => new StaticModInt<T>((uint)((ulong)lhs._v * rhs._v % op.Mod));
        /// <summary>
        /// 除算を行います。
        /// </summary>
        /// <remarks>
        /// <para>- 制約: <paramref name="rhs"/> に乗法の逆元が存在する。（gcd(<paramref name="rhs"/>, mod) = 1）</para>
        /// <para>- 計算量: O(log(mod))</para>
        /// </remarks>
        [MethodImpl(256)]
        public static StaticModInt<T> operator /(StaticModInt<T> lhs, StaticModInt<T> rhs) => lhs * rhs.Inv();
        [MethodImpl(256)]
        public static StaticModInt<T> operator +(StaticModInt<T> v) => v;
        [MethodImpl(256)]
        public static StaticModInt<T> operator -(StaticModInt<T> v) => new StaticModInt<T>(v._v == 0 ? 0 : op.Mod - v._v);
        [MethodImpl(256)]
        public static bool operator ==(StaticModInt<T> lhs, StaticModInt<T> rhs) => lhs._v == rhs._v;
        [MethodImpl(256)]
        public static bool operator !=(StaticModInt<T> lhs, StaticModInt<T> rhs) => lhs._v != rhs._v;
        [MethodImpl(256)]
        public static implicit operator StaticModInt<T>(int v) => new StaticModInt<T>(v);
        [MethodImpl(256)]
        public static implicit operator StaticModInt<T>(uint v) => new StaticModInt<T>((long)v);
        [MethodImpl(256)]
        public static implicit operator StaticModInt<T>(long v) => new StaticModInt<T>(v);
        [MethodImpl(256)]
        public static implicit operator StaticModInt<T>(ulong v) => new StaticModInt<T>(v);

        /// <summary>
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        [MethodImpl(256)]
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
        [MethodImpl(256)]
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
        public string ToString(string format, IFormatProvider formatProvider) => _v.ToString(format, formatProvider);
        public override bool Equals(object obj) => obj is StaticModInt<T> m && Equals(m);
        [MethodImpl(256)] public bool Equals(StaticModInt<T> other) => _v == other._v;
        public override int GetHashCode() => _v.GetHashCode();

#if GENERIC_MATH
        static int INumberBase<StaticModInt<T>>.Radix => 2;
        static StaticModInt<T> IAdditiveIdentity<StaticModInt<T>, StaticModInt<T>>.AdditiveIdentity => default;
        static StaticModInt<T> IMultiplicativeIdentity<StaticModInt<T>, StaticModInt<T>>.MultiplicativeIdentity => new StaticModInt<T>(1u);
        static StaticModInt<T> INumberBase<StaticModInt<T>>.Abs(StaticModInt<T> v) => v;
        static bool INumberBase<StaticModInt<T>>.IsCanonical(StaticModInt<T> v) => true;
        static bool INumberBase<StaticModInt<T>>.IsComplexNumber(StaticModInt<T> v) => false;
        static bool INumberBase<StaticModInt<T>>.IsRealNumber(StaticModInt<T> v) => true;
        static bool INumberBase<StaticModInt<T>>.IsImaginaryNumber(StaticModInt<T> v) => false;
        static bool INumberBase<StaticModInt<T>>.IsEvenInteger(StaticModInt<T> v) => uint.IsEvenInteger(v._v);
        static bool INumberBase<StaticModInt<T>>.IsOddInteger(StaticModInt<T> v) => uint.IsOddInteger(v._v);
        static bool INumberBase<StaticModInt<T>>.IsFinite(StaticModInt<T> v) => true;
        static bool INumberBase<StaticModInt<T>>.IsInfinity(StaticModInt<T> v) => false;
        static bool INumberBase<StaticModInt<T>>.IsInteger(StaticModInt<T> v) => true;
        static bool INumberBase<StaticModInt<T>>.IsPositive(StaticModInt<T> v) => true;
        static bool INumberBase<StaticModInt<T>>.IsNegative(StaticModInt<T> v) => false;
        static bool INumberBase<StaticModInt<T>>.IsPositiveInfinity(StaticModInt<T> v) => false;
        static bool INumberBase<StaticModInt<T>>.IsNegativeInfinity(StaticModInt<T> v) => false;
        static bool INumberBase<StaticModInt<T>>.IsNormal(StaticModInt<T> v) => v._v != 0;
        static bool INumberBase<StaticModInt<T>>.IsSubnormal(StaticModInt<T> v) => false;
        static bool INumberBase<StaticModInt<T>>.IsZero(StaticModInt<T> v) => v._v == 0;
        static bool INumberBase<StaticModInt<T>>.IsNaN(StaticModInt<T> v) => false;
        static StaticModInt<T> INumberBase<StaticModInt<T>>.MaxMagnitude(StaticModInt<T> x, StaticModInt<T> y) => new StaticModInt<T>(uint.Max(x._v, y._v));
        static StaticModInt<T> INumberBase<StaticModInt<T>>.MaxMagnitudeNumber(StaticModInt<T> x, StaticModInt<T> y) => new StaticModInt<T>(uint.Max(x._v, y._v));
        static StaticModInt<T> INumberBase<StaticModInt<T>>.MinMagnitude(StaticModInt<T> x, StaticModInt<T> y) => new StaticModInt<T>(uint.Min(x._v, y._v));
        static StaticModInt<T> INumberBase<StaticModInt<T>>.MinMagnitudeNumber(StaticModInt<T> x, StaticModInt<T> y) => new StaticModInt<T>(uint.Min(x._v, y._v));
        static StaticModInt<T> INumberBase<StaticModInt<T>>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider) => long.Parse(s, style, provider);
        static StaticModInt<T> INumberBase<StaticModInt<T>>.Parse(string s, NumberStyles style, IFormatProvider provider) => long.Parse(s, style, provider);
        static StaticModInt<T> ISpanParsable<StaticModInt<T>>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => long.Parse(s, provider);
        static StaticModInt<T> IParsable<StaticModInt<T>>.Parse(string s, IFormatProvider provider) => long.Parse(s, provider);
        static bool ISpanParsable<StaticModInt<T>>.TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out StaticModInt<T> result)
        => TryParse(s, NumberStyles.None, provider, out result);
        static bool IParsable<StaticModInt<T>>.TryParse(string s, IFormatProvider provider, out StaticModInt<T> result)
        => TryParse(s, NumberStyles.None, provider, out result);
        static bool INumberBase<StaticModInt<T>>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out StaticModInt<T> result)
        => TryParse(s, style, provider, out result);
        static bool INumberBase<StaticModInt<T>>.TryParse(string s, NumberStyles style, IFormatProvider provider, out StaticModInt<T> result)
        => TryParse(s, style, provider, out result);
        private static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out StaticModInt<T> result)
        {
            var b = long.TryParse(s, style, provider, out var r);
            result = r;
            return b;
        }
        bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider) => _v.TryFormat(destination, out charsWritten, format, provider);


        static bool INumberBase<StaticModInt<T>>.TryConvertFromChecked<TOther>(TOther v, out StaticModInt<T> r)
        {
            if (WrapChecked(v, out long l))
            {
                r = l;
                return true;
            }
            if (WrapChecked(v, out ulong u))
            {
                r = u;
                return true;
            }
            r = default;
            return false;
        }
        static bool INumberBase<StaticModInt<T>>.TryConvertFromSaturating<TOther>(TOther v, out StaticModInt<T> r)
        {
            if (WrapSaturating(v, out long l))
            {
                r = l;
                return true;
            }
            if (WrapSaturating(v, out ulong u))
            {
                r = u;
                return true;
            }
            r = default;
            return false;
        }
        static bool INumberBase<StaticModInt<T>>.TryConvertFromTruncating<TOther>(TOther v, out StaticModInt<T> r)
        {
            if (WrapTruncating(v, out long l))
            {
                r = l;
                return true;
            }
            if (WrapTruncating(v, out ulong u))
            {
                r = u;
                return true;
            }
            r = default;
            return false;
        }
        static bool INumberBase<StaticModInt<T>>.TryConvertToChecked<TOther>(StaticModInt<T> v, out TOther r) where TOther : default => WrapChecked(v._v, out r);
        static bool INumberBase<StaticModInt<T>>.TryConvertToSaturating<TOther>(StaticModInt<T> v, out TOther r) where TOther : default => WrapSaturating(v._v, out r);
        static bool INumberBase<StaticModInt<T>>.TryConvertToTruncating<TOther>(StaticModInt<T> v, out TOther r) where TOther : default => WrapTruncating(v._v, out r);

        [MethodImpl(256)]
        static bool WrapChecked<TFrom, TTo>(TFrom v, out TTo r) where TFrom : INumberBase<TFrom> where TTo : INumberBase<TTo>
            => typeof(TFrom) == typeof(TTo)
            ? (r = (TTo)(object)v) is { }
            : TTo.TryConvertFromChecked(v, out r) || TFrom.TryConvertToChecked(v, out r);
        [MethodImpl(256)]
        static bool WrapSaturating<TFrom, TTo>(TFrom v, out TTo r) where TFrom : INumberBase<TFrom> where TTo : INumberBase<TTo>
            => typeof(TFrom) == typeof(TTo)
            ? (r = (TTo)(object)v) is { }
            : TTo.TryConvertFromSaturating(v, out r) || TFrom.TryConvertToSaturating(v, out r);
        [MethodImpl(256)]
        static bool WrapTruncating<TFrom, TTo>(TFrom v, out TTo r) where TFrom : INumberBase<TFrom> where TTo : INumberBase<TTo>
            => typeof(TFrom) == typeof(TTo)
            ? (r = (TTo)(object)v) is { }
            : TTo.TryConvertFromTruncating(v, out r) || TFrom.TryConvertToTruncating(v, out r);
#endif
    }
}
