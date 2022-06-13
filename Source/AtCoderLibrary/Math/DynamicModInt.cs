using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    /// <summary>
    /// 実行時に決定する mod の ID を表します。
    /// </summary>
    /// <example>
    /// <code>
    /// public readonly struct ModID123 : IDynamicModID { }
    /// </code>
    /// </example>
    [IsOperator]
    public interface IDynamicModID { }
    public static class DynamicModIDExtension
    {
        public static void SetMod<T>(this T _, int mod) where T : struct, IDynamicModID => DynamicModInt<T>.Mod = mod;
    }
    public readonly struct DynamicModID0 : IDynamicModID { }
    public readonly struct DynamicModID1 : IDynamicModID { }
    public readonly struct DynamicModID2 : IDynamicModID { }

    /// <summary>
    /// 四則演算時に自動で mod を取る整数型。実行時に mod が決まる場合でも使用可能です。
    /// </summary>
    /// <remarks>
    /// 使用前に DynamicModInt&lt;<typeparamref name="T"/>&gt;.Mod に mod の値を設定する必要があります。
    /// </remarks>
    /// <typeparam name="T">mod の ID を表す構造体</typeparam>
    /// <example>
    /// <code>
    /// using AtCoder.ModInt = AtCoder.DynamicModInt&lt;AtCoder.ModID0&gt;;
    ///
    /// void SomeMethod()
    /// {
    ///     ModInt.Mod = 1000000009;
    ///     var m = new ModInt(1);
    ///     m -= 2;
    ///     Console.WriteLine(m);   // 1000000008
    /// }
    /// </code>
    /// </example>
    public readonly struct DynamicModInt<T>
     : IEquatable<DynamicModInt<T>>, IFormattable
#if GENERIC_MATH
     , INumberBase<DynamicModInt<T>>
#endif
     where T : struct
    {
        internal readonly uint _v;
        internal static Barrett bt;

        /// <summary>
        /// 格納されている値を返します。
        /// </summary>
        public int Value => (int)_v;

        /// <summary>
        /// mod を返します。
        /// </summary>
        public static int Mod
        {
            [MethodImpl(256)]
            get => (int)bt.Mod;
            [MethodImpl(256)]
            set
            {
                Contract.Assert(1 <= value, reason: $"{nameof(Mod)} must be positive.");
                bt = new Barrett((uint)value);
            }
        }

        public static DynamicModInt<T> Zero => default;
        public static DynamicModInt<T> One => new DynamicModInt<T>(1u);

        /// <summary>
        /// <paramref name="v"/> に対して mod を取らずに DynamicModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <para>定数倍高速化のための関数です。 <paramref name="v"/> に 0 未満または mod 以上の値を入れた場合の挙動は未定義です。</para>
        /// <para>制約: 0≤|<paramref name="v"/>|&lt;mod</para>
        /// </remarks>
        [MethodImpl(256)]
        public static DynamicModInt<T> Raw(int v)
        {
            var u = unchecked((uint)v);
            Contract.Assert(bt != null, $"{nameof(DynamicModInt<T>)}<{nameof(T)}>.{nameof(Mod)} is undefined.");
            Contract.Assert(u < Mod, $"{nameof(u)} must be less than {nameof(Mod)}.");
            return new DynamicModInt<T>(u);
        }

        /// <summary>
        /// DynamicModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <para>- 使用前に DynamicModInt&lt;<typeparamref name="T"/>&gt;.Mod に mod の値を設定する必要があります。</para>
        /// <para>- <paramref name="v"/> が 0 未満、もしくは mod 以上の場合、自動で mod を取ります。</para>
        /// </remarks>
        [MethodImpl(256)]
        public DynamicModInt(long v) : this(Round(v)) { }

        /// <summary>
        /// DynamicModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <para>- 使用前に DynamicModInt&lt;<typeparamref name="T"/>&gt;.Mod に mod の値を設定する必要があります。</para>
        /// <para>- <paramref name="v"/> が 0 未満、もしくは mod 以上の場合、自動で mod を取ります。</para>
        /// </remarks>
        [MethodImpl(256)]
        public DynamicModInt(ulong v) : this((uint)(v % bt.Mod)) { }

        [MethodImpl(256)]
        private DynamicModInt(uint v) => _v = v;

        [MethodImpl(256)]
        private static uint Round(long v)
        {
            Contract.Assert(bt != null, $"{nameof(DynamicModInt<T>)}<{nameof(T)}>.{nameof(Mod)} is undefined.");
            var x = v % bt.Mod;
            if (x < 0)
            {
                x += bt.Mod;
            }
            return (uint)x;
        }

        [MethodImpl(256)]
        public static DynamicModInt<T> operator ++(DynamicModInt<T> v)
        {
            var x = v._v + 1;
            if (x == bt.Mod)
            {
                x = 0;
            }
            return new DynamicModInt<T>(x);
        }

        [MethodImpl(256)]
        public static DynamicModInt<T> operator --(DynamicModInt<T> v)
        {
            var x = v._v;
            if (x == 0)
            {
                x = bt.Mod;
            }
            return new DynamicModInt<T>(x - 1);
        }

        [MethodImpl(256)]
        public static DynamicModInt<T> operator +(DynamicModInt<T> lhs, DynamicModInt<T> rhs)
        {
            var v = lhs._v + rhs._v;
            if (v >= bt.Mod)
            {
                v -= bt.Mod;
            }
            return new DynamicModInt<T>(v);
        }

        [MethodImpl(256)]
        public static DynamicModInt<T> operator -(DynamicModInt<T> lhs, DynamicModInt<T> rhs)
        {
            unchecked
            {
                var v = lhs._v - rhs._v;
                if (v >= bt.Mod)
                {
                    v += bt.Mod;
                }
                return new DynamicModInt<T>(v);
            }
        }

        [MethodImpl(256)]
        public static DynamicModInt<T> operator *(DynamicModInt<T> lhs, DynamicModInt<T> rhs)
        {
            uint z = bt.Mul(lhs._v, rhs._v);
            return new DynamicModInt<T>(z);
        }

        /// <summary>
        /// 除算を行います。
        /// </summary>
        /// <remarks>
        /// <para>- 制約: <paramref name="rhs"/> に乗法の逆元が存在する。（gcd(<paramref name="rhs"/>, mod) = 1）</para>
        /// <para>- 計算量: O(log(mod))</para>
        /// </remarks>
        [MethodImpl(256)]
        public static DynamicModInt<T> operator /(DynamicModInt<T> lhs, DynamicModInt<T> rhs) => lhs * rhs.Inv();

        [MethodImpl(256)]
        public static DynamicModInt<T> operator +(DynamicModInt<T> v) => v;
        [MethodImpl(256)]
        public static DynamicModInt<T> operator -(DynamicModInt<T> v) => new DynamicModInt<T>(v._v == 0 ? 0 : Mod - v.Value);
        [MethodImpl(256)]
        public static bool operator ==(DynamicModInt<T> lhs, DynamicModInt<T> rhs) => lhs._v == rhs._v;
        [MethodImpl(256)]
        public static bool operator !=(DynamicModInt<T> lhs, DynamicModInt<T> rhs) => lhs._v != rhs._v;
        [MethodImpl(256)]
        public static implicit operator DynamicModInt<T>(int v) => new DynamicModInt<T>(v);
        [MethodImpl(256)]
        public static implicit operator DynamicModInt<T>(uint v) => new DynamicModInt<T>((long)v);
        [MethodImpl(256)]
        public static implicit operator DynamicModInt<T>(long v) => new DynamicModInt<T>(v);
        [MethodImpl(256)]
        public static implicit operator DynamicModInt<T>(ulong v) => new DynamicModInt<T>(v);

        /// <summary>
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        [MethodImpl(256)]
        public DynamicModInt<T> Pow(long n)
        {
            Contract.Assert(0 <= n, $"{nameof(n)} must be positive.");
            var x = this;
            var r = Raw(1);

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
        public DynamicModInt<T> Inv()
        {
            var (g, x) = InternalMath.InvGCD(_v, bt.Mod);
            Contract.Assert(g == 1, reason: $"gcd({nameof(x)}, {nameof(Mod)}) must be 1.");
            return new DynamicModInt<T>(x);
        }

        public override string ToString() => _v.ToString();
        public string ToString(string format, IFormatProvider formatProvider) => _v.ToString(format, formatProvider);
        public override bool Equals(object obj) => obj is DynamicModInt<T> m && Equals(m);
        [MethodImpl(256)] public bool Equals(DynamicModInt<T> other) => Value == other.Value;
        public override int GetHashCode() => _v.GetHashCode();

#if GENERIC_MATH
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.One => new DynamicModInt<T>(1u);
        static int INumberBase<DynamicModInt<T>>.Radix => 2;
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.Zero => default;
        static DynamicModInt<T> IAdditiveIdentity<DynamicModInt<T>, DynamicModInt<T>>.AdditiveIdentity => default;
        static DynamicModInt<T> IMultiplicativeIdentity<DynamicModInt<T>, DynamicModInt<T>>.MultiplicativeIdentity => new DynamicModInt<T>(1u);
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.Abs(DynamicModInt<T> value) => value;
        static bool INumberBase<DynamicModInt<T>>.IsCanonical(DynamicModInt<T> value) => true;
        static bool INumberBase<DynamicModInt<T>>.IsComplexNumber(DynamicModInt<T> value) => false;
        static bool INumberBase<DynamicModInt<T>>.IsRealNumber(DynamicModInt<T> value) => true;
        static bool INumberBase<DynamicModInt<T>>.IsImaginaryNumber(DynamicModInt<T> value) => false;
        static bool INumberBase<DynamicModInt<T>>.IsEvenInteger(DynamicModInt<T> value) => uint.IsEvenInteger(value._v);
        static bool INumberBase<DynamicModInt<T>>.IsOddInteger(DynamicModInt<T> value) => uint.IsOddInteger(value._v);
        static bool INumberBase<DynamicModInt<T>>.IsFinite(DynamicModInt<T> value) => true;
        static bool INumberBase<DynamicModInt<T>>.IsInfinity(DynamicModInt<T> value) => false;
        static bool INumberBase<DynamicModInt<T>>.IsInteger(DynamicModInt<T> value) => true;
        static bool INumberBase<DynamicModInt<T>>.IsPositive(DynamicModInt<T> value) => true;
        static bool INumberBase<DynamicModInt<T>>.IsNegative(DynamicModInt<T> value) => false;
        static bool INumberBase<DynamicModInt<T>>.IsPositiveInfinity(DynamicModInt<T> value) => false;
        static bool INumberBase<DynamicModInt<T>>.IsNegativeInfinity(DynamicModInt<T> value) => false;
        static bool INumberBase<DynamicModInt<T>>.IsNormal(DynamicModInt<T> value) => value._v != 0;
        static bool INumberBase<DynamicModInt<T>>.IsSubnormal(DynamicModInt<T> value) => false;
        static bool INumberBase<DynamicModInt<T>>.IsZero(DynamicModInt<T> value) => value._v == 0;
        static bool INumberBase<DynamicModInt<T>>.IsNaN(DynamicModInt<T> value) => false;
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.MaxMagnitude(DynamicModInt<T> x, DynamicModInt<T> y) => new DynamicModInt<T>(uint.Max(x._v, y._v));
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.MaxMagnitudeNumber(DynamicModInt<T> x, DynamicModInt<T> y) => new DynamicModInt<T>(uint.Max(x._v, y._v));
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.MinMagnitude(DynamicModInt<T> x, DynamicModInt<T> y) => new DynamicModInt<T>(uint.Min(x._v, y._v));
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.MinMagnitudeNumber(DynamicModInt<T> x, DynamicModInt<T> y) => new DynamicModInt<T>(uint.Min(x._v, y._v));
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider) => long.Parse(s, style, provider);
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.Parse(string s, NumberStyles style, IFormatProvider provider) => long.Parse(s, style, provider);
        static DynamicModInt<T> ISpanParsable<DynamicModInt<T>>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => long.Parse(s, provider);
        static DynamicModInt<T> IParsable<DynamicModInt<T>>.Parse(string s, IFormatProvider provider) => long.Parse(s, provider);
        static bool ISpanParsable<DynamicModInt<T>>.TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out DynamicModInt<T> result)
        => TryParse(s, NumberStyles.None, provider, out result);
        static bool IParsable<DynamicModInt<T>>.TryParse(string s, IFormatProvider provider, out DynamicModInt<T> result)
        => TryParse(s, NumberStyles.None, provider, out result);
        static bool INumberBase<DynamicModInt<T>>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out DynamicModInt<T> result)
        => TryParse(s, style, provider, out result);
        static bool INumberBase<DynamicModInt<T>>.TryParse(string s, NumberStyles style, IFormatProvider provider, out DynamicModInt<T> result)
        => TryParse(s, style, provider, out result);
        private static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out DynamicModInt<T> result)
        {
            var b = long.TryParse(s, style, provider, out var r);
            result = r;
            return b;
        }
        bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider) => _v.TryFormat(destination, out charsWritten, format, provider);


        static bool INumberBase<DynamicModInt<T>>.TryConvertFromChecked<TOther>(TOther value, out DynamicModInt<T> result) => TryConvertFrom(value, out result);
        static bool INumberBase<DynamicModInt<T>>.TryConvertFromSaturating<TOther>(TOther value, out DynamicModInt<T> result) => TryConvertFrom(value, out result);
        static bool INumberBase<DynamicModInt<T>>.TryConvertFromTruncating<TOther>(TOther value, out DynamicModInt<T> result) => TryConvertFrom(value, out result);
        static bool INumberBase<DynamicModInt<T>>.TryConvertToChecked<TOther>(DynamicModInt<T> value, out TOther result) where TOther : default => TryConvertTo(value, out result);
        static bool INumberBase<DynamicModInt<T>>.TryConvertToSaturating<TOther>(DynamicModInt<T> value, out TOther result) where TOther : default => TryConvertTo(value, out result);
        static bool INumberBase<DynamicModInt<T>>.TryConvertToTruncating<TOther>(DynamicModInt<T> value, out TOther result) where TOther : default => TryConvertTo(value, out result);
        private static bool TryConvertFrom<TOther>(TOther v, out DynamicModInt<T> r)
        {
            if (typeof(TOther) == typeof(int))
            {
                r = (uint)(object)v;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                r = (long)(object)v;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                r = (uint)(object)v;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                r = (uint)(((ulong)(object)v) % (uint)Mod);
                return true;
            }
            else
            {
                r = default;
                return false;
            }
        }
        private static bool TryConvertTo<TOther>(DynamicModInt<T> v, out TOther r)
        {
            if (typeof(TOther) == typeof(int))
            {
                int rr = (int)v._v;
                r = (TOther)(object)rr;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long rr = (int)v._v;
                r = (TOther)(object)rr;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                uint rr = v._v;
                r = (TOther)(object)rr;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                ulong rr = v._v;
                r = (TOther)(object)rr;
                return true;
            }
            else
            {
                r = default;
                return false;
            }
        }
#endif
    }
}
