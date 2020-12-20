using System;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    using static MethodImplOptions;
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct UIntOperator : INumOperator<uint>
    {
        public uint MinValue => uint.MinValue;
        public uint MaxValue => uint.MaxValue;

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
        public uint Minus(uint x) => throw new InvalidOperationException("Uint type cannot be negative.");
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
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
