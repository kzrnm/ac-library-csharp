using System;
using System.Runtime.CompilerServices;
using AtCoder.Operators;

namespace AtCoder
{
    public readonly struct StaticModIntOperator<T> : IArithmeticOperator<StaticModInt<T>>
        where T : struct, IStaticMod
    {
        public StaticModInt<T> MultiplyIdentity => StaticModInt<T>.Raw(1);
        [MethodImpl(256)]
        public StaticModInt<T> Add(StaticModInt<T> x, StaticModInt<T> y) => x + y;
        [MethodImpl(256)]
        public StaticModInt<T> Subtract(StaticModInt<T> x, StaticModInt<T> y) => x - y;
        [MethodImpl(256)]
        public StaticModInt<T> Multiply(StaticModInt<T> x, StaticModInt<T> y) => x * y;
        [MethodImpl(256)]
        public StaticModInt<T> Divide(StaticModInt<T> x, StaticModInt<T> y) => x / y;
        [MethodImpl(256)]
        StaticModInt<T> IDivisionOperator<StaticModInt<T>>.Modulo(StaticModInt<T> x, StaticModInt<T> y) => throw new NotSupportedException();
        [MethodImpl(256)]
        public StaticModInt<T> Minus(StaticModInt<T> x) => -x;
        [MethodImpl(256)]
        public StaticModInt<T> Increment(StaticModInt<T> x) => ++x;
        [MethodImpl(256)]
        public StaticModInt<T> Decrement(StaticModInt<T> x) => --x;
    }
}
