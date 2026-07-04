using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

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
     : IEquatable<DynamicModInt<T>>, IFormattable, IModIntNumberBase<DynamicModInt<T>>
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
        public static DynamicModInt<T> One => new(1u);

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
#if SOURCE_EMBEDDING
            Contract.Assert(bt != null, $"{nameof(DynamicModInt<T>)}<{nameof(T)}>.{nameof(Mod)} is undefined.");
            Contract.Assert(u < Mod, $"{nameof(u)} must be less than {nameof(Mod)}.");
#endif
            return new(u);
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
            return new(x);
        }

        [MethodImpl(256)]
        public static DynamicModInt<T> operator --(DynamicModInt<T> v)
        {
            var x = v._v;
            if (x == 0)
            {
                x = bt.Mod;
            }
            return new(x - 1);
        }

        [MethodImpl(256)]
        public static DynamicModInt<T> operator +(DynamicModInt<T> lhs, DynamicModInt<T> rhs)
        {
            var v = lhs._v + rhs._v;
            if (v >= bt.Mod)
            {
                v -= bt.Mod;
            }
            return new(v);
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
                return new(v);
            }
        }

        [MethodImpl(256)]
        public static DynamicModInt<T> operator *(DynamicModInt<T> lhs, DynamicModInt<T> rhs)
        {
            uint z = bt.Mul(lhs._v, rhs._v);
            return new(z);
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
        public static DynamicModInt<T> operator -(DynamicModInt<T> v) => new(v._v == 0 ? 0 : Mod - v.Value);
        [MethodImpl(256)]
        public static bool operator ==(DynamicModInt<T> lhs, DynamicModInt<T> rhs) => lhs._v == rhs._v;
        [MethodImpl(256)]
        public static bool operator !=(DynamicModInt<T> lhs, DynamicModInt<T> rhs) => lhs._v != rhs._v;
        [MethodImpl(256)]
        public static implicit operator DynamicModInt<T>(int v) => new(v);
        [MethodImpl(256)]
        public static implicit operator DynamicModInt<T>(uint v) => new((ulong)v);
        [MethodImpl(256)]
        public static implicit operator DynamicModInt<T>(long v) => new(v);
        [MethodImpl(256)]
        public static implicit operator DynamicModInt<T>(ulong v) => new(v);

        /// <summary>
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        [MethodImpl(256)]
        public DynamicModInt<T> Pow(long n) => new(bt.Pow(Value, n));

        /// <summary>
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        [MethodImpl(256)]
        public DynamicModInt<T> Pow(ulong n) => new(bt.Pow(Value, n));

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
            return new(x);
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
    }
}
