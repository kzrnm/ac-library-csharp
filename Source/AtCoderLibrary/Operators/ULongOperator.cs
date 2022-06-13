using System;
using System.Runtime.CompilerServices;
using AtCoder.Operators;

namespace AtCoder
{
#if GENERIC_MATH
    [Obsolete("Use generic math")]
#endif
    public readonly struct ULongOperator : INumOperator<ulong>, IShiftOperator<ulong>
    {
        public ulong MinValue => ulong.MinValue;
        public ulong MaxValue => ulong.MaxValue;
        public ulong MultiplyIdentity => 1UL;

        [MethodImpl(256)]
        public ulong Add(ulong x, ulong y) => x + y;
        [MethodImpl(256)]
        public ulong Subtract(ulong x, ulong y) => x - y;
        [MethodImpl(256)]
        public ulong Multiply(ulong x, ulong y) => x * y;
        [MethodImpl(256)]
        public ulong Divide(ulong x, ulong y) => x / y;
        [MethodImpl(256)]
        public ulong Modulo(ulong x, ulong y) => x % y;
        [MethodImpl(256)]
        ulong IUnaryNumOperator<ulong>.Minus(ulong x) => throw new InvalidOperationException("Ulong type cannot be negative.");
        [MethodImpl(256)]
        public ulong Increment(ulong x) => ++x;
        [MethodImpl(256)]
        public ulong Decrement(ulong x) => --x;
        [MethodImpl(256)]
        public bool GreaterThan(ulong x, ulong y) => x > y;
        [MethodImpl(256)]
        public bool GreaterThanOrEqual(ulong x, ulong y) => x >= y;
        [MethodImpl(256)]
        public bool LessThan(ulong x, ulong y) => x < y;
        [MethodImpl(256)]
        public bool LessThanOrEqual(ulong x, ulong y) => x <= y;
        [MethodImpl(256)]
        public int Compare(ulong x, ulong y) => x.CompareTo(y);
        [MethodImpl(256)]
        public ulong LeftShift(ulong x, int y) => x << y;
        [MethodImpl(256)]
        public ulong RightShift(ulong x, int y) => x >> y;
    }
}
