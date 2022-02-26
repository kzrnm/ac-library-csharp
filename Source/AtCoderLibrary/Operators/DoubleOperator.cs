using System.Runtime.CompilerServices;
using AtCoder.Operators;

namespace AtCoder
{
    public readonly struct DoubleOperator : INumOperator<double>
    {
        public double MinValue => double.MinValue;
        public double MaxValue => double.MaxValue;
        public double MultiplyIdentity => 1.0;

        [MethodImpl(256)]
        public double Add(double x, double y) => x + y;
        [MethodImpl(256)]
        public double Subtract(double x, double y) => x - y;
        [MethodImpl(256)]
        public double Multiply(double x, double y) => x * y;
        [MethodImpl(256)]
        public double Divide(double x, double y) => x / y;
        [MethodImpl(256)]
        public double Modulo(double x, double y) => x % y;
        [MethodImpl(256)]
        public double Minus(double x) => -x;
        [MethodImpl(256)]
        public double Increment(double x) => ++x;
        [MethodImpl(256)]
        public double Decrement(double x) => --x;
        [MethodImpl(256)]
        public bool GreaterThan(double x, double y) => x > y;
        [MethodImpl(256)]
        public bool GreaterThanOrEqual(double x, double y) => x >= y;
        [MethodImpl(256)]
        public bool LessThan(double x, double y) => x < y;
        [MethodImpl(256)]
        public bool LessThanOrEqual(double x, double y) => x <= y;
        [MethodImpl(256)]
        public int Compare(double x, double y) => x.CompareTo(y);
    }
}
