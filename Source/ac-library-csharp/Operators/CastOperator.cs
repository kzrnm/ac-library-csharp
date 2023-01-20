using System.Runtime.CompilerServices;

namespace AtCoder.Operators
{
    [IsOperator]
#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public interface ICastOperator<in TFrom, out TTo>
    {
        TTo Cast(TFrom y);
    }

#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public struct SameTypeCastOperator<T> : ICastOperator<T, T>
    {
        [MethodImpl(256)]
        public T Cast(T y) => y;
    }

#if GENERIC_MATH
    [System.Obsolete("Use generic math")]
#endif
    public struct IntToLongCastOperator : ICastOperator<int, long>
    {
        [MethodImpl(256)]
        public long Cast(int y) => y;
    }
}
