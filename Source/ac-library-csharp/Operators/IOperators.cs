using System.Collections.Generic;

namespace AtCoder.Operators
{
    /// <summary>
    /// <typeparamref name="T"/> についての加法演算を定義します。
    /// </summary>
    [IsOperator]
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public interface IAdditionOperator<T>
    {
        /// <summary>
        /// Addition operator +
        /// </summary>
        /// <returns><paramref name="x"/> + <paramref name="y"/></returns>
        T Add(T x, T y);
    }

    /// <summary>
    /// <typeparamref name="T"/> についての減法演算を定義します。
    /// </summary>
    [IsOperator]
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public interface ISubtractOperator<T>
    {
        /// <summary>
        /// Subtraction operator -
        /// </summary>
        /// <returns><paramref name="x"/> - <paramref name="y"/></returns>
        T Subtract(T x, T y);
    }

    /// <summary>
    /// <typeparamref name="T"/> についての乗法演算を定義します。
    /// </summary>
    [IsOperator]
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public interface IMultiplicationOperator<T>
    {
        /// <summary>
        /// Multiplication operator *
        /// </summary>
        /// <returns><paramref name="x"/> * <paramref name="y"/></returns>
        T Multiply(T x, T y);
        /// <summary>
        /// 乗法単位元 (数値演算では 1 ) を返します。
        /// </summary>
        T MultiplyIdentity { get; }
    }

    /// <summary>
    /// <typeparamref name="T"/> についての除法演算を定義します。
    /// </summary>
    [IsOperator]
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public interface IDivisionOperator<T> : IMultiplicationOperator<T>
    {
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
    }

    /// <summary>
    /// <typeparamref name="T"/> についての加法演算を定義します。
    /// </summary>
    [IsOperator]
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public interface IUnaryNumOperator<T>
    {
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

    /// <summary>
    /// <typeparamref name="T"/> についての四則演算を定義します。
    /// </summary>
    [IsOperator]
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public interface IArithmeticOperator<T> : IAdditionOperator<T>, ISubtractOperator<T>, IMultiplicationOperator<T>, IDivisionOperator<T>, IUnaryNumOperator<T> { }

    [IsOperator]
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public interface ICompareOperator<T> : IComparer<T>
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

    [IsOperator]
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public interface IMinMaxValueOperator<T>
    {
        /// <summary>
        /// MinValue
        /// </summary>
        T MinValue { get; }
        /// <summary>
        /// MaxValue
        /// </summary>
        T MaxValue { get; }
    }

    [IsOperator]
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public interface INumOperator<T> : IArithmeticOperator<T>, ICompareOperator<T>, IMinMaxValueOperator<T>
    {
    }

    /// <summary>
    /// <typeparamref name="T"/> についてのシフト演算を定義します。
    /// </summary>
    [IsOperator]
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public interface IShiftOperator<T>
    {
        /// <summary>
        /// Left shift operator &lt;&lt;
        /// </summary>
        /// <returns><paramref name="x"/> &lt;&lt; <paramref name="y"/></returns>
        T LeftShift(T x, int y);
        /// <summary>
        /// Right shift operator &gt;&gt;
        /// </summary>
        /// <returns><paramref name="x"/> &gt;&gt; <paramref name="y"/></returns>
        T RightShift(T x, int y);
    }

}
