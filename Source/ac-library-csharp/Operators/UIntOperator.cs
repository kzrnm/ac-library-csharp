using System;
using System.Runtime.CompilerServices;
using AtCoder.Operators;

namespace AtCoder
{
#if GENERIC_MATH
    [Obsolete("Use generic math")]
#endif
    public readonly struct UIntOperator : INumOperator<uint>, IShiftOperator<uint>
    {
        public uint MinValue => uint.MinValue;
        public uint MaxValue => uint.MaxValue;
        public uint MultiplyIdentity => 1U;

        [MethodImpl(256)]
        public uint Add(uint x, uint y) => x + y;
        [MethodImpl(256)]
        public uint Subtract(uint x, uint y) => x - y;
        [MethodImpl(256)]
        public uint Multiply(uint x, uint y) => x * y;
        [MethodImpl(256)]
        public uint Divide(uint x, uint y) => x / y;
        [MethodImpl(256)]
        public uint Modulo(uint x, uint y) => x % y;
        [MethodImpl(256)]
        uint IUnaryNumOperator<uint>.Minus(uint x) => throw new InvalidOperationException("Uint type cannot be negative.");
        [MethodImpl(256)]
        public uint Increment(uint x) => ++x;
        [MethodImpl(256)]
        public uint Decrement(uint x) => --x;
        [MethodImpl(256)]
        public bool GreaterThan(uint x, uint y) => x > y;
        [MethodImpl(256)]
        public bool GreaterThanOrEqual(uint x, uint y) => x >= y;
        [MethodImpl(256)]
        public bool LessThan(uint x, uint y) => x < y;
        [MethodImpl(256)]
        public bool LessThanOrEqual(uint x, uint y) => x <= y;
        [MethodImpl(256)]
        public int Compare(uint x, uint y) => x.CompareTo(y);
        [MethodImpl(256)]
        public uint LeftShift(uint x, int y) => x << y;
        [MethodImpl(256)]
        public uint RightShift(uint x, int y) => x >> y;
    }
}
