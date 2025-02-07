using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AtCoder.Internal;
#if GENERIC_MATH
using System.Globalization;
using System.Numerics;
#endif

namespace AtCoder
{
    /// <summary>
    /// 実行時に決定する mod の ID を表します。
    /// </summary>
    /// <example>
    /// <code>
    /// public readonly struct ModID123 : IDynamicModIntId { }
    /// </code>
    /// </example>
    [IsOperator]
    public interface IDynamicModIntId { }
    public static class DynamicModIntIdExtension
    {
        public static void SetMod<T>(this T _, int mod) where T : struct, IDynamicModIntId => DynamicModInt<T>.Mod = mod;
    }
    public readonly struct DynamicModIntId0 : IDynamicModIntId { }
    public readonly struct DynamicModIntId1 : IDynamicModIntId { }
    public readonly struct DynamicModIntId2 : IDynamicModIntId { }

    /// <summary>
    /// 四則演算時に自動で mod を取る整数型。実行時に mod が決まる場合でも使用可能です。
    /// </summary>
    /// <remarks>
    /// 使用前に DynamicModInt&lt;<typeparamref name="T"/>&gt;.Mod に mod の値を設定する必要があります。
    /// </remarks>
    /// <typeparam name="T">mod の ID を表す構造体</typeparam>
    /// <example>
    /// <code>
    /// using AtCoder.ModInt = AtCoder.DynamicModInt&lt;AtCoder.DynamicModIntId0&gt;;
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
    [DebuggerDisplay("{Value,nq}")]
    public readonly struct DynamicModInt<T>
     : IEquatable<DynamicModInt<T>>, IFormattable, IModInt<DynamicModInt<T>>
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
#if EMBEDDING
            Contract.Assert(bt != null, $"{nameof(DynamicModInt<T>)}<{nameof(T)}>.{nameof(Mod)} is undefined.");
            Contract.Assert(u < Mod, $"{nameof(u)} must be less than {nameof(Mod)}.");
#endif
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
        public DynamicModInt(long v) : this((uint)ModCalc.SafeMod(v, bt.Mod)) { }

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
        public static implicit operator DynamicModInt<T>(uint v) => new DynamicModInt<T>((ulong)v);
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
        public DynamicModInt<T> Pow(long n) => new DynamicModInt<T>(bt.Pow(Value, n));

        /// <summary>
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        [MethodImpl(256)]
        public DynamicModInt<T> Pow(ulong n) => new DynamicModInt<T>(bt.Pow(Value, n));

        /// <summary>
        /// 自身を x として、 xy≡1 なる y を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: gcd(x, mod) = 1</para>
        /// </remarks>
        [MethodImpl(256)]
        public DynamicModInt<T> Inv()
        {
            var (g, x) = ModCalc.InvGcd(_v, bt.Mod);
            Contract.Assert(g == 1, reason: $"gcd({nameof(x)}, {nameof(Mod)}) must be 1.");
            return new DynamicModInt<T>(x);
        }

        public override string ToString() => _v.ToString();
        public string ToString(string format, IFormatProvider formatProvider) => _v.ToString(format, formatProvider);
        public override bool Equals(object obj) => obj is DynamicModInt<T> m && Equals(m);
        [MethodImpl(256)] public bool Equals(DynamicModInt<T> other) => Value == other.Value;
        public override int GetHashCode() => _v.GetHashCode();

        public static bool TryParse(ReadOnlySpan<char> s, out DynamicModInt<T> result)
        {
            result = Zero;
            DynamicModInt<T> ten = 10u;
            s = s.Trim();
            bool minus = false;
            if (s.Length > 0 && s[0] == '-')
            {
                minus = true;
                s = s.Slice(1);
            }
            for (int i = 0; i < s.Length; i++)
            {
                var d = (uint)(s[i] - '0');
                if (d >= 10) return false;
                result = result * ten + d;
            }
            if (minus)
                result = -result;
            return true;
        }
        public static DynamicModInt<T> Parse(ReadOnlySpan<char> s)
        {
            if (!TryParse(s, out var r))
                Throw();
            return r;
            void Throw() => throw new FormatException();
        }

#if GENERIC_MATH
        static int INumberBase<DynamicModInt<T>>.Radix => 2;
        static DynamicModInt<T> IAdditiveIdentity<DynamicModInt<T>, DynamicModInt<T>>.AdditiveIdentity => default;
        static DynamicModInt<T> IMultiplicativeIdentity<DynamicModInt<T>, DynamicModInt<T>>.MultiplicativeIdentity => new DynamicModInt<T>(1u);
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.Abs(DynamicModInt<T> v) => v;
        static bool INumberBase<DynamicModInt<T>>.IsCanonical(DynamicModInt<T> v) => true;
        static bool INumberBase<DynamicModInt<T>>.IsComplexNumber(DynamicModInt<T> v) => false;
        static bool INumberBase<DynamicModInt<T>>.IsRealNumber(DynamicModInt<T> v) => true;
        static bool INumberBase<DynamicModInt<T>>.IsImaginaryNumber(DynamicModInt<T> v) => false;
        static bool INumberBase<DynamicModInt<T>>.IsEvenInteger(DynamicModInt<T> v) => uint.IsEvenInteger(v._v);
        static bool INumberBase<DynamicModInt<T>>.IsOddInteger(DynamicModInt<T> v) => uint.IsOddInteger(v._v);
        static bool INumberBase<DynamicModInt<T>>.IsFinite(DynamicModInt<T> v) => true;
        static bool INumberBase<DynamicModInt<T>>.IsInfinity(DynamicModInt<T> v) => false;
        static bool INumberBase<DynamicModInt<T>>.IsInteger(DynamicModInt<T> v) => true;
        static bool INumberBase<DynamicModInt<T>>.IsPositive(DynamicModInt<T> v) => true;
        static bool INumberBase<DynamicModInt<T>>.IsNegative(DynamicModInt<T> v) => false;
        static bool INumberBase<DynamicModInt<T>>.IsPositiveInfinity(DynamicModInt<T> v) => false;
        static bool INumberBase<DynamicModInt<T>>.IsNegativeInfinity(DynamicModInt<T> v) => false;
        static bool INumberBase<DynamicModInt<T>>.IsNormal(DynamicModInt<T> v) => v._v != 0;
        static bool INumberBase<DynamicModInt<T>>.IsSubnormal(DynamicModInt<T> v) => false;
        static bool INumberBase<DynamicModInt<T>>.IsZero(DynamicModInt<T> v) => v._v == 0;
        static bool INumberBase<DynamicModInt<T>>.IsNaN(DynamicModInt<T> v) => false;
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.MaxMagnitude(DynamicModInt<T> x, DynamicModInt<T> y) => new DynamicModInt<T>(uint.Max(x._v, y._v));
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.MaxMagnitudeNumber(DynamicModInt<T> x, DynamicModInt<T> y) => new DynamicModInt<T>(uint.Max(x._v, y._v));
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.MinMagnitude(DynamicModInt<T> x, DynamicModInt<T> y) => new DynamicModInt<T>(uint.Min(x._v, y._v));
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.MinMagnitudeNumber(DynamicModInt<T> x, DynamicModInt<T> y) => new DynamicModInt<T>(uint.Min(x._v, y._v));

        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider) => Parse(s);
        static DynamicModInt<T> INumberBase<DynamicModInt<T>>.Parse(string s, NumberStyles style, IFormatProvider provider) => Parse(s);
        static DynamicModInt<T> ISpanParsable<DynamicModInt<T>>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => Parse(s);
        static DynamicModInt<T> IParsable<DynamicModInt<T>>.Parse(string s, IFormatProvider provider) => Parse(s);
        static bool ISpanParsable<DynamicModInt<T>>.TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out DynamicModInt<T> result) => TryParse(s, out result);
        static bool IParsable<DynamicModInt<T>>.TryParse(string s, IFormatProvider provider, out DynamicModInt<T> result) => TryParse(s, out result);
        static bool INumberBase<DynamicModInt<T>>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out DynamicModInt<T> result) => TryParse(s, out result);
        static bool INumberBase<DynamicModInt<T>>.TryParse(string s, NumberStyles style, IFormatProvider provider, out DynamicModInt<T> result) => TryParse(s, out result);

        bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider) => _v.TryFormat(destination, out charsWritten, format, provider);


        static bool INumberBase<DynamicModInt<T>>.TryConvertFromChecked<TOther>(TOther v, out DynamicModInt<T> r)
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
        static bool INumberBase<DynamicModInt<T>>.TryConvertFromSaturating<TOther>(TOther v, out DynamicModInt<T> r)
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
        static bool INumberBase<DynamicModInt<T>>.TryConvertFromTruncating<TOther>(TOther v, out DynamicModInt<T> r)
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
        static bool INumberBase<DynamicModInt<T>>.TryConvertToChecked<TOther>(DynamicModInt<T> v, out TOther r) where TOther : default => WrapChecked(v._v, out r);
        static bool INumberBase<DynamicModInt<T>>.TryConvertToSaturating<TOther>(DynamicModInt<T> v, out TOther r) where TOther : default => WrapSaturating(v._v, out r);
        static bool INumberBase<DynamicModInt<T>>.TryConvertToTruncating<TOther>(DynamicModInt<T> v, out TOther r) where TOther : default => WrapTruncating(v._v, out r);

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
