using System.Collections.Generic;

namespace AtCoder
{
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
}
