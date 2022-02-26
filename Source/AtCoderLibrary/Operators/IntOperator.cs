using System.Runtime.CompilerServices;
using AtCoder.Operators;

namespace AtCoder
{
    public readonly struct IntOperator : INumOperator<int>, IShiftOperator<int>
    {
        public int MinValue => int.MinValue;
        public int MaxValue => int.MaxValue;
        public int MultiplyIdentity => 1;

        [MethodImpl(256)]
        public int Add(int x, int y) => x + y;
        [MethodImpl(256)]
        public int Subtract(int x, int y) => x - y;
        [MethodImpl(256)]
        public int Multiply(int x, int y) => x * y;
        [MethodImpl(256)]
        public int Divide(int x, int y) => x / y;
        [MethodImpl(256)]
        public int Modulo(int x, int y) => x % y;
        [MethodImpl(256)]
        public int Minus(int x) => -x;
        [MethodImpl(256)]
        public int Increment(int x) => ++x;
        [MethodImpl(256)]
        public int Decrement(int x) => --x;
        [MethodImpl(256)]
        public bool GreaterThan(int x, int y) => x > y;
        [MethodImpl(256)]
        public bool GreaterThanOrEqual(int x, int y) => x >= y;
        [MethodImpl(256)]
        public bool LessThan(int x, int y) => x < y;
        [MethodImpl(256)]
        public bool LessThanOrEqual(int x, int y) => x <= y;
        [MethodImpl(256)]
        public int Compare(int x, int y) => x.CompareTo(y);
        [MethodImpl(256)]
        public int LeftShift(int x, int y) => x << y;
        [MethodImpl(256)]
        public int RightShift(int x, int y) => x >> y;
    }
}
