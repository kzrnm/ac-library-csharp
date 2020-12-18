using System;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    using static MethodImplOptions;
#pragma warning disable CA1815 // Override equals and operator equals on value types
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
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
