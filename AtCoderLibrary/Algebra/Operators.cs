using System.Collections.Generic;

namespace AtCoder
{
#pragma warning disable CA1815
    public interface IAddOperator<T>
    {
        /// <summary>
        /// Addition operator +
        /// </summary>
        /// <returns><paramref name="x"/> + <paramref name="y"/></returns>
        T Add(T x, T y);
    }
    public interface ISubtractOperator<T>
    {
        /// <summary>
        /// Subtraction operator -
        /// </summary>
        /// <returns><paramref name="x"/> - <paramref name="y"/></returns>
        T Subtract(T x, T y);
    }
    public interface IMultiplyOperator<T>
    {
        /// <summary>
        /// Multiplication operator *
        /// </summary>
        /// <returns><paramref name="x"/> * <paramref name="y"/></returns>
        T Multiply(T x, T y);
    }
    public interface IDivideOperator<T>
    {
        /// <summary>
        /// Division operator /
        /// </summary>
        /// <returns><paramref name="x"/> / <paramref name="y"/></returns>
        T Divide(T x, T y);
    }
    public interface IModuloOperator<T>
    {
        /// <summary>
        /// Remainder operator %
        /// </summary>
        /// <returns><paramref name="x"/> % <paramref name="y"/></returns>
        T Modulo(T x, T y);
    }
    public interface INegateOperator<T>
    {
        /// <summary>
        /// Unary minus operator -
        /// </summary>
        /// <returns>-<paramref name="x"/></returns>
        T Negate(T x);
    }
    public interface IIncrementOperator<T> : IAddOperator<T>
    {
        /// <summary>
        /// Increment operator ++
        /// </summary>
        /// <returns>++<paramref name="x"/></returns>
        T Increment(T x);
    }
    public interface IDecrementOperator<T> : ISubtractOperator<T>
    {
        /// <summary>
        /// Decrement operator --
        /// </summary>
        /// <returns>--<paramref name="x"/></returns>
        T Decrement(T x);
    }
    public interface IComparisonOperator<T> : IComparer<T>
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
    public interface IRangedType<T>
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

    public interface IUnsignedNumOperator<T> :
        IAddOperator<T>,
        ISubtractOperator<T>,
        IMultiplyOperator<T>,
        IDivideOperator<T>,
        IModuloOperator<T>,
        IIncrementOperator<T>,
        IDecrementOperator<T>,
        IComparisonOperator<T>,
        IRangedType<T>,
        IEqualityComparer<T>
    {
    }
    public interface ISignedNumOperator<T> :
        IUnsignedNumOperator<T>,
        INegateOperator<T>
    {
    }
    public readonly struct IntOperator : ISignedNumOperator<int>
    {
        public int MinValue => int.MinValue;
        public int MaxValue => int.MaxValue;
        public int Add(int x, int y) => x + y;
        public int Subtract(int x, int y) => x - y;
        public int Multiply(int x, int y) => x * y;
        public int Divide(int x, int y) => x / y;
        public int Modulo(int x, int y) => x % y;
        public int Negate(int x) => -x;
        public int Increment(int x) => ++x;
        public int Decrement(int x) => --x;
        public bool GreaterThan(int x, int y) => x > y;
        public bool GreaterThanOrEqual(int x, int y) => x >= y;
        public bool LessThan(int x, int y) => x < y;
        public bool LessThanOrEqual(int x, int y) => x <= y;
        public int Compare(int x, int y) => x.CompareTo(y);
        public bool Equals(int x, int y) => x == y;
        public int GetHashCode(int obj) => obj.GetHashCode();
    }
    public readonly struct LongOperator : ISignedNumOperator<long>
    {
        public long MinValue => long.MinValue;
        public long MaxValue => long.MaxValue;
        public long Add(long x, long y) => x + y;
        public long Subtract(long x, long y) => x - y;
        public long Multiply(long x, long y) => x * y;
        public long Divide(long x, long y) => x / y;
        public long Modulo(long x, long y) => x % y;
        public long Negate(long x) => -x;
        public long Increment(long x) => ++x;
        public long Decrement(long x) => --x;
        public bool GreaterThan(long x, long y) => x > y;
        public bool GreaterThanOrEqual(long x, long y) => x >= y;
        public bool LessThan(long x, long y) => x < y;
        public bool LessThanOrEqual(long x, long y) => x <= y;
        public int Compare(long x, long y) => x.CompareTo(y);
        public bool Equals(long x, long y) => x == y;
        public int GetHashCode(long obj) => obj.GetHashCode();
    }
    public readonly struct UIntOperator : IUnsignedNumOperator<uint>
    {
        public uint MinValue => uint.MinValue;
        public uint MaxValue => uint.MaxValue;
        public uint Add(uint x, uint y) => x + y;
        public uint Subtract(uint x, uint y) => x - y;
        public uint Multiply(uint x, uint y) => x * y;
        public uint Divide(uint x, uint y) => x / y;
        public uint Modulo(uint x, uint y) => x % y;
        public uint Increment(uint x) => ++x;
        public uint Decrement(uint x) => --x;
        public bool GreaterThan(uint x, uint y) => x > y;
        public bool GreaterThanOrEqual(uint x, uint y) => x >= y;
        public bool LessThan(uint x, uint y) => x < y;
        public bool LessThanOrEqual(uint x, uint y) => x <= y;
        public int Compare(uint x, uint y) => x.CompareTo(y);
        public bool Equals(uint x, uint y) => x == y;
        public int GetHashCode(uint obj) => obj.GetHashCode();
    }

    public readonly struct ULongOperator : IUnsignedNumOperator<ulong>
    {
        public ulong MinValue => ulong.MinValue;
        public ulong MaxValue => ulong.MaxValue;
        public ulong Add(ulong x, ulong y) => x + y;
        public ulong Subtract(ulong x, ulong y) => x - y;
        public ulong Multiply(ulong x, ulong y) => x * y;
        public ulong Divide(ulong x, ulong y) => x / y;
        public ulong Modulo(ulong x, ulong y) => x % y;
        public ulong Increment(ulong x) => ++x;
        public ulong Decrement(ulong x) => --x;
        public bool GreaterThan(ulong x, ulong y) => x > y;
        public bool GreaterThanOrEqual(ulong x, ulong y) => x >= y;
        public bool LessThan(ulong x, ulong y) => x < y;
        public bool LessThanOrEqual(ulong x, ulong y) => x <= y;
        public int Compare(ulong x, ulong y) => x.CompareTo(y);
        public bool Equals(ulong x, ulong y) => x == y;
        public int GetHashCode(ulong obj) => obj.GetHashCode();
    }
    public readonly struct DoubleOperator : ISignedNumOperator<double>
    {
        public double MinValue => double.MinValue;
        public double MaxValue => double.MaxValue;
        public double Add(double x, double y) => x + y;
        public double Subtract(double x, double y) => x - y;
        public double Multiply(double x, double y) => x * y;
        public double Divide(double x, double y) => x / y;
        public double Modulo(double x, double y) => x % y;
        public double Negate(double x) => -x;
        public double Increment(double x) => ++x;
        public double Decrement(double x) => --x;
        public bool GreaterThan(double x, double y) => x > y;
        public bool GreaterThanOrEqual(double x, double y) => x >= y;
        public bool LessThan(double x, double y) => x < y;
        public bool LessThanOrEqual(double x, double y) => x <= y;
        public int Compare(double x, double y) => x.CompareTo(y);
        public bool Equals(double x, double y) => x == y;
        public int GetHashCode(double obj) => obj.GetHashCode();
    }
}
