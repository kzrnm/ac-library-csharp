using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    using static MethodImplOptions;
#pragma warning disable CA1815 // Override equals and operator equals on value types
    [IsOperator]
    public interface IArithmeticOperator<T> where T : struct
    {
        /// <summary>
        /// Addition operator +
        /// </summary>
        /// <returns><paramref name="x"/> + <paramref name="y"/></returns>
        T Add(T x, T y);
        /// <summary>
        /// Subtraction operator -
        /// </summary>
        /// <returns><paramref name="x"/> - <paramref name="y"/></returns>
        T Subtract(T x, T y);
        /// <summary>
        /// Multiplication operator *
        /// </summary>
        /// <returns><paramref name="x"/> * <paramref name="y"/></returns>
        T Multiply(T x, T y);
        /// <summary>
        /// Division operator /
        /// </summary>
        /// <returns><paramref name="x"/> / <paramref name="y"/></returns>
        T Divide(T x, T y);
        /// <summary>
        /// Remainder operator %
        /// </summary>
        /// <returns><paramref name="x"/> % <paramref name="y"/></returns>
        T Modulo(T x, T y);
        /// <summary>
        /// Unary minus operator -
        /// </summary>
        /// <returns>-<paramref name="x"/></returns>
        T Minus(T x);
        /// <summary>
        /// Increment operator ++
        /// </summary>
        /// <returns>++<paramref name="x"/></returns>
        T Increment(T x);
        /// <summary>
        /// Decrement operator --
        /// </summary>
        /// <returns>--<paramref name="x"/></returns>
        T Decrement(T x);
    }
    [IsOperator]
    public interface ICompareOperator<T> : IEqualityComparer<T>, IComparer<T> where T : struct
    {
        /// <summary>
        /// Greater than operator &gt;
        /// </summary>
        /// <returns><paramref name="x"/> &gt; <paramref name="y"/></returns>
        bool GreaterThan(T x, T y);
        /// <summary>
        /// Greater than or equal operator &gt;=
        /// </summary>
        /// <returns><paramref name="x"/> &gt;= <paramref name="y"/></returns>
        bool GreaterThanOrEqual(T x, T y);
        /// <summary>
        /// Less than operator &lt;
        /// </summary>
        /// <returns><paramref name="x"/> &lt; <paramref name="y"/></returns>
        bool LessThan(T x, T y);
        /// <summary>
        /// Less than or equal operator &lt;=
        /// </summary>
        /// <returns><paramref name="x"/> &lt;= <paramref name="y"/></returns>
        bool LessThanOrEqual(T x, T y);
    }
    public interface INumOperator<T> : IArithmeticOperator<T>, ICompareOperator<T> where T : struct
    {
        /// <summary>
        /// MinValue
        /// </summary>
        public T MinValue { get; }
        /// <summary>
        /// MaxValue
        /// </summary>
        public T MaxValue { get; }
    }
    public readonly struct IntOperator : INumOperator<int>
    {
        public int MinValue => int.MinValue;
        public int MaxValue => int.MaxValue;

        [MethodImpl(AggressiveInlining)]
        public int Add(int x, int y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public int Subtract(int x, int y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public int Multiply(int x, int y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public int Divide(int x, int y) => x / y;
        [MethodImpl(AggressiveInlining)]
        public int Modulo(int x, int y) => x % y;
        [MethodImpl(AggressiveInlining)]
        public int Minus(int x) => -x;
        [MethodImpl(AggressiveInlining)]
        public int Increment(int x) => ++x;
        [MethodImpl(AggressiveInlining)]
        public int Decrement(int x) => --x;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThan(int x, int y) => x > y;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThanOrEqual(int x, int y) => x >= y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThan(int x, int y) => x < y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThanOrEqual(int x, int y) => x <= y;
        [MethodImpl(AggressiveInlining)]
        public int Compare(int x, int y) => x.CompareTo(y);
        [MethodImpl(AggressiveInlining)]
        public bool Equals(int x, int y) => x == y;
        [MethodImpl(AggressiveInlining)]
        public int GetHashCode(int obj) => obj.GetHashCode();
    }
    public readonly struct LongOperator : INumOperator<long>
    {
        public long MinValue => long.MinValue;
        public long MaxValue => long.MaxValue;

        [MethodImpl(AggressiveInlining)]
        public long Add(long x, long y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public long Subtract(long x, long y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public long Multiply(long x, long y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public long Divide(long x, long y) => x / y;
        [MethodImpl(AggressiveInlining)]
        public long Modulo(long x, long y) => x % y;
        [MethodImpl(AggressiveInlining)]
        public long Minus(long x) => -x;
        [MethodImpl(AggressiveInlining)]
        public long Increment(long x) => ++x;
        [MethodImpl(AggressiveInlining)]
        public long Decrement(long x) => --x;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThan(long x, long y) => x > y;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThanOrEqual(long x, long y) => x >= y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThan(long x, long y) => x < y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThanOrEqual(long x, long y) => x <= y;
        [MethodImpl(AggressiveInlining)]
        public int Compare(long x, long y) => x.CompareTo(y);
        [MethodImpl(AggressiveInlining)]
        public bool Equals(long x, long y) => x == y;
        [MethodImpl(AggressiveInlining)]
        public int GetHashCode(long obj) => obj.GetHashCode();
    }
    public readonly struct UIntOperator : INumOperator<uint>
    {
        public uint MinValue => uint.MinValue;
        public uint MaxValue => uint.MaxValue;

        [MethodImpl(AggressiveInlining)]
        public uint Add(uint x, uint y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public uint Subtract(uint x, uint y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public uint Multiply(uint x, uint y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public uint Divide(uint x, uint y) => x / y;
        [MethodImpl(AggressiveInlining)]
        public uint Modulo(uint x, uint y) => x % y;
        [MethodImpl(AggressiveInlining)]
        public uint Minus(uint x) => throw new InvalidOperationException("Uint type cannot be negative.");
        [MethodImpl(AggressiveInlining)]
        public uint Increment(uint x) => ++x;
        [MethodImpl(AggressiveInlining)]
        public uint Decrement(uint x) => --x;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThan(uint x, uint y) => x > y;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThanOrEqual(uint x, uint y) => x >= y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThan(uint x, uint y) => x < y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThanOrEqual(uint x, uint y) => x <= y;
        [MethodImpl(AggressiveInlining)]
        public int Compare(uint x, uint y) => x.CompareTo(y);
        [MethodImpl(AggressiveInlining)]
        public bool Equals(uint x, uint y) => x == y;
        [MethodImpl(AggressiveInlining)]
        public int GetHashCode(uint obj) => obj.GetHashCode();
    }
    public readonly struct ULongOperator : INumOperator<ulong>
    {
        public ulong MinValue => ulong.MinValue;
        public ulong MaxValue => ulong.MaxValue;

        [MethodImpl(AggressiveInlining)]
        public ulong Add(ulong x, ulong y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public ulong Subtract(ulong x, ulong y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public ulong Multiply(ulong x, ulong y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public ulong Divide(ulong x, ulong y) => x / y;
        [MethodImpl(AggressiveInlining)]
        public ulong Modulo(ulong x, ulong y) => x % y;
        [MethodImpl(AggressiveInlining)]
        public ulong Minus(ulong x) => throw new InvalidOperationException("Ulong type cannot be negative.");
        [MethodImpl(AggressiveInlining)]
        public ulong Increment(ulong x) => ++x;
        [MethodImpl(AggressiveInlining)]
        public ulong Decrement(ulong x) => --x;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThan(ulong x, ulong y) => x > y;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThanOrEqual(ulong x, ulong y) => x >= y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThan(ulong x, ulong y) => x < y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThanOrEqual(ulong x, ulong y) => x <= y;
        [MethodImpl(AggressiveInlining)]
        public int Compare(ulong x, ulong y) => x.CompareTo(y);
        [MethodImpl(AggressiveInlining)]
        public bool Equals(ulong x, ulong y) => x == y;
        [MethodImpl(AggressiveInlining)]
        public int GetHashCode(ulong obj) => obj.GetHashCode();
    }
    public readonly struct DoubleOperator : INumOperator<double>
    {
        public double MinValue => double.MinValue;
        public double MaxValue => double.MaxValue;

        [MethodImpl(AggressiveInlining)]
        public double Add(double x, double y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public double Subtract(double x, double y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public double Multiply(double x, double y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public double Divide(double x, double y) => x / y;
        [MethodImpl(AggressiveInlining)]
        public double Modulo(double x, double y) => x % y;
        [MethodImpl(AggressiveInlining)]
        public double Minus(double x) => -x;
        [MethodImpl(AggressiveInlining)]
        public double Increment(double x) => ++x;
        [MethodImpl(AggressiveInlining)]
        public double Decrement(double x) => --x;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThan(double x, double y) => x > y;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThanOrEqual(double x, double y) => x >= y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThan(double x, double y) => x < y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThanOrEqual(double x, double y) => x <= y;
        [MethodImpl(AggressiveInlining)]
        public int Compare(double x, double y) => x.CompareTo(y);
        [MethodImpl(AggressiveInlining)]
        public bool Equals(double x, double y) => x == y;
        [MethodImpl(AggressiveInlining)]
        public int GetHashCode(double obj) => obj.GetHashCode();
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
