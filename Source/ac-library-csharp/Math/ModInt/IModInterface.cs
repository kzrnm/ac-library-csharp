using System;
using System.Numerics;
using System.Runtime.CompilerServices;

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
        static abstract int Mod { get; }

        /// <summary>
        /// <paramref name="v"/> に対して mod を取らずに StaticModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <para>定数倍高速化のための関数です。 <paramref name="v"/> に 0 未満または mod 以上の値を入れた場合の挙動は未定義です。</para>
        /// <para>制約: 0≤|<paramref name="v"/>|&lt;mod</para>
        /// </remarks>
        static abstract T Raw(int v);
    }
    public interface IModIntNumberBase<T> : IModInt<T>, IIntBase<T> where T : IModInt<T>, IIntBase<T>
    {
        static T INumberBase<T>.Abs(T v) => v;
        static bool INumberBase<T>.IsPositive(T v) => true;
        static bool INumberBase<T>.IsNegative(T v) => false;
        static bool INumberBase<T>.IsEvenInteger(T v) => int.IsEvenInteger(v.Value);
        static bool INumberBase<T>.IsOddInteger(T v) => int.IsOddInteger(v.Value);

        static T INumberBase<T>.MaxMagnitude(T x, T y) => T.Raw(int.Max(x.Value, y.Value));
        static T INumberBase<T>.MaxMagnitudeNumber(T x, T y) => T.Raw(int.Max(x.Value, y.Value));
        static T INumberBase<T>.MinMagnitude(T x, T y) => T.Raw(int.Min(x.Value, y.Value));
        static T INumberBase<T>.MinMagnitudeNumber(T x, T y) => T.Raw(int.Min(x.Value, y.Value));

        bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider) => Value.TryFormat(destination, out charsWritten, format, provider);

        static bool Cnv<TF, TT>(TF v, out TT r) where TF : INumberBase<TF> where TT : INumberBase<TT>
            => typeof(TF) == typeof(TT)
            ? (r = (TT)(object)v) is { }
            : TT.TryConvertFromChecked(v, out r) || TF.TryConvertToChecked(v, out r);
        [MethodImpl(256)]
        static bool ConvF<TF>(TF v, out T r) where TF : INumberBase<TF>
        {
            BigInteger b;
            if (typeof(TF) == typeof(BigInteger))
                b = (BigInteger)(object)v;
            else if (!Cft(v, out b) && !TF.TryConvertToTruncating(v, out b))
            {
                r = default;
                return false;
            }
            var m = (int)(b % T.Mod);
            if (m < 0) m += T.Mod;
            r = T.Raw(m);
            return true;
            [MethodImpl(256)] static bool Cft<B>(TF v, out B r) where B : INumberBase<B> => B.TryConvertFromTruncating(v, out r);
        }
        static bool INumberBase<T>.TryConvertFromChecked<TF>(TF v, out T r)
            => ConvF(v, out r);
        static bool INumberBase<T>.TryConvertFromSaturating<TF>(TF v, out T r)
            => ConvF(v, out r);
        static bool INumberBase<T>.TryConvertFromTruncating<TF>(TF v, out T r)
            => ConvF(v, out r);

        static bool INumberBase<T>.TryConvertToChecked<TT>(T v, out TT r) => Cnv(v.Value, out r);
        static bool INumberBase<T>.TryConvertToSaturating<TT>(T v, out TT r) => Cnv(v.Value, out r);
        static bool INumberBase<T>.TryConvertToTruncating<TT>(T v, out TT r) => Cnv(v.Value, out r);
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
