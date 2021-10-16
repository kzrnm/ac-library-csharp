using System.Runtime.CompilerServices;
using AtCoder.Operators;

namespace AtCoder
{
    using static MethodImplOptions;
    public readonly struct IntOperator : INumOperator<int>, IShiftOperator<int>
    {
        public int MinValue => int.MinValue;
        public int MaxValue => int.MaxValue;
        public int MultiplyIdentity => 1;

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
        public int LeftShift(int x, int y) => x << y;
        [MethodImpl(AggressiveInlining)]
        public int RightShift(int x, int y) => x >> y;
    }
}
