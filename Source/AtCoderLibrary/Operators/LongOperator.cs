using System.Runtime.CompilerServices;
using AtCoder.Operators;

namespace AtCoder
{
    using static MethodImplOptions;
    public readonly struct LongOperator : INumOperator<long>, IShiftOperator<long>
    {
        public long MinValue => long.MinValue;
        public long MaxValue => long.MaxValue;
        public long MultiplyIdentity => 1L;

        [MethodImpl(AggressiveInlining)]
        public long Add(long x, long y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public long Subtract(long x, long y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public long Multiply(long x, long y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public long Divide(long x, long y) => x / y;
        [MethodImpl(AggressiveInlining)]
        public long Modulo(long x, long y) => x % y;
        [MethodImpl(AggressiveInlining)]
        public long Minus(long x) => -x;
        [MethodImpl(AggressiveInlining)]
        public long Increment(long x) => ++x;
        [MethodImpl(AggressiveInlining)]
        public long Decrement(long x) => --x;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThan(long x, long y) => x > y;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThanOrEqual(long x, long y) => x >= y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThan(long x, long y) => x < y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThanOrEqual(long x, long y) => x <= y;
        [MethodImpl(AggressiveInlining)]
        public int Compare(long x, long y) => x.CompareTo(y);
        [MethodImpl(AggressiveInlining)]
        public long LeftShift(long x, int y) => x << y;
        [MethodImpl(AggressiveInlining)]
        public long RightShift(long x, int y) => x >> y;
    }
}
