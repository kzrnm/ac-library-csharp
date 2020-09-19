using System.Collections.Generic;
using System.Diagnostics;

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

#pragma warning disable CA1815
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
    /// 実行時に決定する mod の ID を表します。
    /// </summary>
    /// <example>
    /// <code>
    /// public readonly struct ModID123 : IDynamicModID { }
    /// </code>
    /// </example>
    public interface IDynamicModID { }

    public readonly struct ModID0 : IDynamicModID { }
    public readonly struct ModID1 : IDynamicModID { }
    public readonly struct ModID2 : IDynamicModID { }

    public readonly struct StaticModIntOperator<T> :
        IAddOperator<StaticModInt<T>>,
        ISubtractOperator<StaticModInt<T>>,
        IMultiplyOperator<StaticModInt<T>>,
        IDivideOperator<StaticModInt<T>>,
        IIncrementOperator<StaticModInt<T>>,
        IDecrementOperator<StaticModInt<T>>,
        INegateOperator<StaticModInt<T>>,
        IEqualityComparer<StaticModInt<T>>
        where T : struct, IStaticMod
    {
        public StaticModInt<T> Add(StaticModInt<T> x, StaticModInt<T> y) => x + y;
        public StaticModInt<T> Subtract(StaticModInt<T> x, StaticModInt<T> y) => x - y;
        public StaticModInt<T> Multiply(StaticModInt<T> x, StaticModInt<T> y) => x * y;
        public StaticModInt<T> Divide(StaticModInt<T> x, StaticModInt<T> y) => x / y;
        public StaticModInt<T> Negate(StaticModInt<T> x) => -x;
        public StaticModInt<T> Increment(StaticModInt<T> x) => ++x;
        public StaticModInt<T> Decrement(StaticModInt<T> x) => --x;
        public bool Equals(StaticModInt<T> x, StaticModInt<T> y) => x == y;
        public int GetHashCode(StaticModInt<T> obj) => obj.GetHashCode();
    }
#pragma warning restore CA1815
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
    public readonly struct StaticModInt<T> where T : struct, IStaticMod
    {
        private readonly uint _v;

        /// <summary>
        /// 格納されている値を返します。
        /// </summary>
        public int Value => (int)_v;

        /// <summary>
        /// mod を返します。
        /// </summary>
        public static int Mod => (int)default(T).Mod;

        /// <summary>
        /// <paramref name="v"/> に対して mod を取らずに StaticModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <para>定数倍高速化のための関数です。 <paramref name="v"/> に 0 未満または mod 以上の値を入れた場合の挙動は未定義です。</para>
        /// <para>制約: 0≤|<paramref name="v"/>|&lt;mod</para>
        /// </remarks>
        public static StaticModInt<T> Raw(int v)
        {
            var u = unchecked((uint)v);
            Debug.Assert(u < Mod);
            return new StaticModInt<T>(u);
        }

        /// <summary>
        /// StaticModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <paramref name="v"/>が 0 未満、もしくは mod 以上の場合、自動で mod を取ります。
        /// </remarks>
        public StaticModInt(long v) : this(Round(v)) { }

        private StaticModInt(uint v) => _v = v;

        private static uint Round(long v)
        {
            var x = v % default(T).Mod;
            if (x < 0)
            {
                x += default(T).Mod;
            }
            return (uint)x;
        }

        public static StaticModInt<T> operator ++(StaticModInt<T> value)
        {
            var v = value._v + 1;
            if (v == default(T).Mod)
            {
                v = 0;
            }
            return new StaticModInt<T>(v);
        }

        public static StaticModInt<T> operator --(StaticModInt<T> value)
        {
            var v = value._v;
            if (v == 0)
            {
                v = default(T).Mod;
            }
            return new StaticModInt<T>(v - 1);
        }

        public static StaticModInt<T> operator +(StaticModInt<T> lhs, StaticModInt<T> rhs)
        {
            var v = lhs._v + rhs._v;
            if (v >= default(T).Mod)
            {
                v -= default(T).Mod;
            }
            return new StaticModInt<T>(v);
        }

        public static StaticModInt<T> operator -(StaticModInt<T> lhs, StaticModInt<T> rhs)
        {
            unchecked
            {
                var v = lhs._v - rhs._v;
                if (v >= default(T).Mod)
                {
                    v += default(T).Mod;
                }
                return new StaticModInt<T>(v);
            }
        }

        public static StaticModInt<T> operator *(StaticModInt<T> lhs, StaticModInt<T> rhs)
        {
            return new StaticModInt<T>((uint)((ulong)lhs._v * rhs._v % default(T).Mod));
        }

        /// <summary>
        /// 除算を行います。
        /// </summary>
        /// <remarks>
        /// <para>- 制約: <paramref name="rhs"/> に乗法の逆元が存在する。（gcd(<paramref name="rhs"/>, mod) = 1）</para>
        /// <para>- 計算量: O(log(mod))</para>
        /// </remarks>
        public static StaticModInt<T> operator /(StaticModInt<T> lhs, StaticModInt<T> rhs) => lhs * rhs.Inv();

        public static StaticModInt<T> operator +(StaticModInt<T> value) => value;
        public static StaticModInt<T> operator -(StaticModInt<T> value) => new StaticModInt<T>() - value;
        public static bool operator ==(StaticModInt<T> lhs, StaticModInt<T> rhs) => lhs._v == rhs._v;
        public static bool operator !=(StaticModInt<T> lhs, StaticModInt<T> rhs) => lhs._v != rhs._v;
        public static implicit operator StaticModInt<T>(int value) => new StaticModInt<T>(value);
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
            Debug.Assert(0 <= n);
            var x = this;
            var r = new StaticModInt<T>(1u);

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
        public StaticModInt<T> Inv()
        {
            if (default(T).IsPrime)
            {
                Debug.Assert(_v > 0);
                return Pow(default(T).Mod - 2);
            }
            else
            {
                var (g, x) = Internal.InternalMath.InvGCD(_v, default(T).Mod);
                Debug.Assert(g == 1);
                return new StaticModInt<T>(x);
            }
        }

        public override string ToString() => _v.ToString();
        public override bool Equals(object obj) => obj is StaticModInt<T> && this == (StaticModInt<T>)obj;
        public override int GetHashCode() => _v.GetHashCode();
    }

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
    public readonly struct DynamicModInt<T> where T : struct, IDynamicModID
    {
        private readonly uint _v;
        private static Internal.Barrett bt;

        /// <summary>
        /// 格納されている値を返します。
        /// </summary>
        public int Value => (int)_v;

        /// <summary>
        /// mod を返します。
        /// </summary>
        public static int Mod
        {
            get => (int)bt.Mod;
            set
            {
                Debug.Assert(1 <= value);
                bt = new Internal.Barrett((uint)value);
            }
        }

        /// <summary>
        /// <paramref name="v"/> に対して mod を取らずに DynamicModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <para>定数倍高速化のための関数です。 <paramref name="v"/> に 0 未満または mod 以上の値を入れた場合の挙動は未定義です。</para>
        /// <para>制約: 0≤|<paramref name="v"/>|&lt;mod</para>
        /// </remarks>
        public static DynamicModInt<T> Raw(int v)
        {
            var u = unchecked((uint)v);
            Debug.Assert(bt != null, $"使用前に {nameof(DynamicModInt<T>)}<{nameof(T)}>.{nameof(Mod)} プロパティに mod の値を設定してください。");
            Debug.Assert(u < Mod);
            return new DynamicModInt<T>(u);
        }

        /// <summary>
        /// DynamicModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// <para>- 使用前に DynamicModInt&lt;<typeparamref name="T"/>&gt;.Mod に mod の値を設定する必要があります。</para>
        /// <para>- <paramref name="v"/> が 0 未満、もしくは mod 以上の場合、自動で mod を取ります。</para>
        /// </remarks>
        public DynamicModInt(long v) : this(Round(v)) { }

        private DynamicModInt(uint v) => _v = v;

        private static uint Round(long v)
        {
            Debug.Assert(bt != null, $"使用前に {nameof(DynamicModInt<T>)}<{nameof(T)}>.{nameof(Mod)} プロパティに mod の値を設定してください。");
            var x = v % bt.Mod;
            if (x < 0)
            {
                x += bt.Mod;
            }
            return (uint)x;
        }

        public static DynamicModInt<T> operator ++(DynamicModInt<T> value)
        {
            var v = value._v + 1;
            if (v == bt.Mod)
            {
                v = 0;
            }
            return new DynamicModInt<T>(v);
        }

        public static DynamicModInt<T> operator --(DynamicModInt<T> value)
        {
            var v = value._v;
            if (v == 0)
            {
                v = bt.Mod;
            }
            return new DynamicModInt<T>(v - 1);
        }

        public static DynamicModInt<T> operator +(DynamicModInt<T> lhs, DynamicModInt<T> rhs)
        {
            var v = lhs._v + rhs._v;
            if (v >= bt.Mod)
            {
                v -= bt.Mod;
            }
            return new DynamicModInt<T>(v);
        }

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
        public static DynamicModInt<T> operator /(DynamicModInt<T> lhs, DynamicModInt<T> rhs) => lhs * rhs.Inv();

        public static DynamicModInt<T> operator +(DynamicModInt<T> value) => value;
        public static DynamicModInt<T> operator -(DynamicModInt<T> value) => new DynamicModInt<T>() - value;
        public static bool operator ==(DynamicModInt<T> lhs, DynamicModInt<T> rhs) => lhs._v == rhs._v;
        public static bool operator !=(DynamicModInt<T> lhs, DynamicModInt<T> rhs) => lhs._v != rhs._v;
        public static implicit operator DynamicModInt<T>(int value) => new DynamicModInt<T>(value);
        public static implicit operator DynamicModInt<T>(long value) => new DynamicModInt<T>(value);

        /// <summary>
        /// 自身を x として、x^<paramref name="n"/> を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="n"/>|</para>
        /// <para>計算量: O(log(<paramref name="n"/>))</para>
        /// </remarks>
        public DynamicModInt<T> Pow(long n)
        {
            Debug.Assert(0 <= n);
            var x = this;
            var r = new DynamicModInt<T>(1u);

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
        public DynamicModInt<T> Inv()
        {
            var (g, x) = Internal.InternalMath.InvGCD(_v, bt.Mod);
            Debug.Assert(g == 1);
            return new DynamicModInt<T>(x);
        }

        public override string ToString() => _v.ToString();
        public override bool Equals(object obj) => obj is DynamicModInt<T> && this == (DynamicModInt<T>)obj;
        public override int GetHashCode() => _v.GetHashCode();
    }
}
