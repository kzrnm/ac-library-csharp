using System;
using System.Runtime.CompilerServices;
using AtCoder.Operators;

namespace AtCoder
{
#if GENERIC_MATH
    [Obsolete("Use generic math")]
#endif
    public readonly struct DynamicModIntOperator<T> : IArithmeticOperator<DynamicModInt<T>> where T : struct
    {
        public DynamicModInt<T> MultiplyIdentity => DynamicModInt<T>.Raw(1);
        [MethodImpl(256)]
        public DynamicModInt<T> Add(DynamicModInt<T> x, DynamicModInt<T> y) => x + y;
        [MethodImpl(256)]
        public DynamicModInt<T> Subtract(DynamicModInt<T> x, DynamicModInt<T> y) => x - y;
        [MethodImpl(256)]
        public DynamicModInt<T> Multiply(DynamicModInt<T> x, DynamicModInt<T> y) => x * y;
        [MethodImpl(256)]
        public DynamicModInt<T> Divide(DynamicModInt<T> x, DynamicModInt<T> y) => x / y;
        [MethodImpl(256)]
        DynamicModInt<T> IDivisionOperator<DynamicModInt<T>>.Modulo(DynamicModInt<T> x, DynamicModInt<T> y) => throw new NotSupportedException();
        [MethodImpl(256)]
        public DynamicModInt<T> Minus(DynamicModInt<T> x) => -x;
        [MethodImpl(256)]
        public DynamicModInt<T> Increment(DynamicModInt<T> x) => ++x;
        [MethodImpl(256)]
        public DynamicModInt<T> Decrement(DynamicModInt<T> x) => --x;
    }
}
