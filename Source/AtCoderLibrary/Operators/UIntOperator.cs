using System;
using System.Runtime.CompilerServices;
using AtCoder.Operators;

namespace AtCoder
{
    using static MethodImplOptions;
    public readonly struct UIntOperator : INumOperator<uint>, IShiftOperator<uint>
    {
        public uint MinValue => uint.MinValue;
        public uint MaxValue => uint.MaxValue;
        public uint MultiplyIdentity => 1U;

        [MethodImpl(AggressiveInlining)]
        public uint Add(uint x, uint y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public uint Subtract(uint x, uint y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public uint Multiply(uint x, uint y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public uint Divide(uint x, uint y) => x / y;
        [MethodImpl(AggressiveInlining)]
        public uint Modulo(uint x, uint y) => x % y;
        [MethodImpl(AggressiveInlining)]
        uint IUnaryNumOperator<uint>.Minus(uint x) => throw new InvalidOperationException("Uint type cannot be negative.");
        [MethodImpl(AggressiveInlining)]
        public uint Increment(uint x) => ++x;
        [MethodImpl(AggressiveInlining)]
        public uint Decrement(uint x) => --x;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThan(uint x, uint y) => x > y;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThanOrEqual(uint x, uint y) => x >= y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThan(uint x, uint y) => x < y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThanOrEqual(uint x, uint y) => x <= y;
        [MethodImpl(AggressiveInlining)]
        public int Compare(uint x, uint y) => x.CompareTo(y);
        [MethodImpl(AggressiveInlining)]
        public uint LeftShift(uint x, int y) => x << y;
        [MethodImpl(AggressiveInlining)]
        public uint RightShift(uint x, int y) => x >> y;
    }
}
