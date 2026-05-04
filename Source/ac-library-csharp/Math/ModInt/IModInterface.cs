using System;
using System.Globalization;
using System.Numerics;

namespace AtCoder
{
    public interface IModInt<T> : INumberBase<T> where T : INumberBase<T>
    {
        /// <summary>
        /// 自身を x として、 xy≡1 なる y を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: gcd(x, mod) = 1</para>
        /// </remarks>
        T Inv();

        /// <summary>
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        T Pow(ulong n);

        /// <summary>
        /// 格納されている値を返します。
        /// </summary>
        int Value { get; }

        /// <summary>
        /// mod を返します。
        /// </summary>
        abstract static int Mod { get; }
        abstract static T Create(uint v);
        abstract static T Parse(ReadOnlySpan<char> s);
        abstract static bool TryParse(ReadOnlySpan<char> s, out T result);
    }
    public interface IModIntNumberBase<T> : IModInt<T> where T : IModInt<T>
    {
        static int INumberBase<T>.Radix => 2;
        static T IAdditiveIdentity<T, T>.AdditiveIdentity => default;
        static T IMultiplicativeIdentity<T, T>.MultiplicativeIdentity => T.One;
        static T INumberBase<T>.Abs(T v) => v;
        static bool INumberBase<T>.IsCanonical(T v) => true;
        static bool INumberBase<T>.IsComplexNumber(T v) => false;
        static bool INumberBase<T>.IsRealNumber(T v) => true;
        static bool INumberBase<T>.IsImaginaryNumber(T v) => false;
        static bool INumberBase<T>.IsEvenInteger(T v) => int.IsEvenInteger(v.Value);
        static bool INumberBase<T>.IsOddInteger(T v) => int.IsOddInteger(v.Value);
        static bool INumberBase<T>.IsFinite(T v) => true;
        static bool INumberBase<T>.IsInfinity(T v) => false;
        static bool INumberBase<T>.IsInteger(T v) => true;
        static bool INumberBase<T>.IsPositive(T v) => true;
        static bool INumberBase<T>.IsNegative(T v) => false;
        static bool INumberBase<T>.IsPositiveInfinity(T v) => false;
        static bool INumberBase<T>.IsNegativeInfinity(T v) => false;
        static bool INumberBase<T>.IsNormal(T v) => v.Value != 0;
        static bool INumberBase<T>.IsZero(T v) => v.Value == 0;
        static bool INumberBase<T>.IsSubnormal(T v) => false;
        static bool INumberBase<T>.IsNaN(T v) => false;

        static T INumberBase<T>.MaxMagnitude(T x, T y) => T.Create((uint)int.Max(x.Value, y.Value));
        static T INumberBase<T>.MaxMagnitudeNumber(T x, T y) => T.Create((uint)int.Max(x.Value, y.Value));
        static T INumberBase<T>.MinMagnitude(T x, T y) => T.Create((uint)int.Min(x.Value, y.Value));
        static T INumberBase<T>.MinMagnitudeNumber(T x, T y) => T.Create((uint)int.Min(x.Value, y.Value));

        static T INumberBase<T>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider) => T.Parse(s);
        static T INumberBase<T>.Parse(string s, NumberStyles style, IFormatProvider provider) => T.Parse(s);
        static T ISpanParsable<T>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => T.Parse(s);
        static T IParsable<T>.Parse(string s, IFormatProvider provider) => T.Parse(s);
        static bool ISpanParsable<T>.TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out T result) => T.TryParse(s, out result);
        static bool IParsable<T>.TryParse(string s, IFormatProvider provider, out T result) => T.TryParse(s, out result);
        static bool INumberBase<T>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out T result) => T.TryParse(s, out result);
        static bool INumberBase<T>.TryParse(string s, NumberStyles style, IFormatProvider provider, out T result) => T.TryParse(s, out result);
        bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider) => Value.TryFormat(destination, out charsWritten, format, provider);

        static bool Cnv<TF, TT>(TF v, out TT r) where TF : INumberBase<TF> where TT : INumberBase<TT>
            => typeof(TF) == typeof(TT)
            ? (r = (TT)(object)v) is { }
            : TT.TryConvertFromChecked(v, out r) || TF.TryConvertToChecked(v, out r);
        static bool ConvF<TOther>(TOther v, out T r) where TOther : INumberBase<TOther>
        {
            var d = TOther.CreateTruncating((uint)T.Mod);
            var q = v / d;
            v -= q * d;
            var rt = Cnv<TOther, uint>(v, out var u);
            r = T.Create(u);
            return rt;
        }
        static bool INumberBase<T>.TryConvertFromChecked<TOther>(TOther v, out T r)
            => ConvF(v, out r);
        static bool INumberBase<T>.TryConvertFromSaturating<TOther>(TOther v, out T r)
            => ConvF(v, out r);
        static bool INumberBase<T>.TryConvertFromTruncating<TOther>(TOther v, out T r)
            => ConvF(v, out r);

        static bool INumberBase<T>.TryConvertToChecked<TOther>(T v, out TOther r) => Cnv(v.Value, out r);
        static bool INumberBase<T>.TryConvertToSaturating<TOther>(T v, out TOther r) => Cnv(v.Value, out r);
        static bool INumberBase<T>.TryConvertToTruncating<TOther>(T v, out TOther r) => Cnv(v.Value, out r);
    }

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
}
