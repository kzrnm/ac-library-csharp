using System;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    using static MethodImplOptions;
#pragma warning disable CA1815 // Override equals and operator equals on value types
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
