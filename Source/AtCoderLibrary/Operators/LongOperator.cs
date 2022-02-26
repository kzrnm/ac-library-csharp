using System.Runtime.CompilerServices;
using AtCoder.Operators;

namespace AtCoder
{
    public readonly struct LongOperator : INumOperator<long>, IShiftOperator<long>
    {
        public long MinValue => long.MinValue;
        public long MaxValue => long.MaxValue;
        public long MultiplyIdentity => 1L;

        [MethodImpl(256)]
        public long Add(long x, long y) => x + y;
        [MethodImpl(256)]
        public long Subtract(long x, long y) => x - y;
        [MethodImpl(256)]
        public long Multiply(long x, long y) => x * y;
        [MethodImpl(256)]
        public long Divide(long x, long y) => x / y;
        [MethodImpl(256)]
        public long Modulo(long x, long y) => x % y;
        [MethodImpl(256)]
        public long Minus(long x) => -x;
        [MethodImpl(256)]
        public long Increment(long x) => ++x;
        [MethodImpl(256)]
        public long Decrement(long x) => --x;
        [MethodImpl(256)]
        public bool GreaterThan(long x, long y) => x > y;
        [MethodImpl(256)]
        public bool GreaterThanOrEqual(long x, long y) => x >= y;
        [MethodImpl(256)]
        public bool LessThan(long x, long y) => x < y;
        [MethodImpl(256)]
        public bool LessThanOrEqual(long x, long y) => x <= y;
        [MethodImpl(256)]
        public int Compare(long x, long y) => x.CompareTo(y);
        [MethodImpl(256)]
        public long LeftShift(long x, int y) => x << y;
        [MethodImpl(256)]
        public long RightShift(long x, int y) => x >> y;
    }
}
