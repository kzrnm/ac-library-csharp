using System;
using System.Runtime.CompilerServices;

namespace AtCoder
{
    using static MethodImplOptions;
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct ULongOperator : INumOperator<ulong>
    {
        public ulong MinValue => ulong.MinValue;
        public ulong MaxValue => ulong.MaxValue;

        [MethodImpl(AggressiveInlining)]
        public ulong Add(ulong x, ulong y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public ulong Subtract(ulong x, ulong y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public ulong Multiply(ulong x, ulong y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public ulong Divide(ulong x, ulong y) => x / y;
        [MethodImpl(AggressiveInlining)]
        public ulong Modulo(ulong x, ulong y) => x % y;
        [MethodImpl(AggressiveInlining)]
        public ulong Minus(ulong x) => throw new InvalidOperationException("Ulong type cannot be negative.");
        [MethodImpl(AggressiveInlining)]
        public ulong Increment(ulong x) => ++x;
        [MethodImpl(AggressiveInlining)]
        public ulong Decrement(ulong x) => --x;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThan(ulong x, ulong y) => x > y;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThanOrEqual(ulong x, ulong y) => x >= y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThan(ulong x, ulong y) => x < y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThanOrEqual(ulong x, ulong y) => x <= y;
        [MethodImpl(AggressiveInlining)]
        public int Compare(ulong x, ulong y) => x.CompareTo(y);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
