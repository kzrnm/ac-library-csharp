using System;
using System.Globalization;
using System.Numerics;

namespace AtCoder
{
    public interface IIntBase<T> : INumberBase<T> where T : IIntBase<T>
    {
        static T INumberBase<T>.Zero => default;
        static T IAdditiveIdentity<T, T>.AdditiveIdentity => T.Zero;
        static T IMultiplicativeIdentity<T, T>.MultiplicativeIdentity => T.One;
        static int INumberBase<T>.Radix => 2;
        static bool INumberBase<T>.IsZero(T v) => v == T.Zero;
        static bool INumberBase<T>.IsCanonical(T v) => true;
        static bool INumberBase<T>.IsSubnormal(T v) => false;
        static bool INumberBase<T>.IsComplexNumber(T v) => false;
        static bool INumberBase<T>.IsRealNumber(T v) => true;
        static bool INumberBase<T>.IsImaginaryNumber(T v) => false;
        static bool INumberBase<T>.IsFinite(T v) => true;
        static bool INumberBase<T>.IsInfinity(T v) => false;
        static bool INumberBase<T>.IsNegativeInfinity(T v) => false;
        static bool INumberBase<T>.IsPositiveInfinity(T v) => false;
        static bool INumberBase<T>.IsInteger(T v) => true;
        static bool INumberBase<T>.IsNaN(T v) => false;
        static bool INumberBase<T>.IsNormal(T v) => !T.IsZero(v);

        static abstract bool TryParse(ReadOnlySpan<char> s, out T r);
        static virtual T Parse(ReadOnlySpan<char> s)
            => T.TryParse(s, out T r) ? r : throw new FormatException();

        static bool INumberBase<T>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out T res) => T.TryParse(s, out res);
        static bool INumberBase<T>.TryParse(string s, NumberStyles style, IFormatProvider provider, out T res) => T.TryParse(s, out res);
        static bool ISpanParsable<T>.TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out T res) => T.TryParse(s, out res);
        static bool IParsable<T>.TryParse(string s, IFormatProvider provider, out T res) => T.TryParse(s, out res);

        static T INumberBase<T>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider) => T.Parse(s);
        static T INumberBase<T>.Parse(string s, NumberStyles style, IFormatProvider provider) => T.Parse(s);
        static T IParsable<T>.Parse(string s, IFormatProvider provider) => T.Parse(s);
        static T ISpanParsable<T>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => T.Parse(s);
    }
}
