using System.Runtime.CompilerServices;

namespace AtCoder.Operators
{
    [IsOperator]
    [System.Obsolete("Use generic math")]
    public interface ICastOperator<in TFrom, out TTo>
    {
        TTo Cast(TFrom y);
    }

    [System.Obsolete("Use generic math")]
    public struct SameTypeCastOperator<T> : ICastOperator<T, T>
    {
        [MethodImpl(256)]
        public T Cast(T y) => y;
    }

    [System.Obsolete("Use generic math")]
    public struct IntToLongCastOperator : ICastOperator<int, long>
    {
        [MethodImpl(256)]
        public long Cast(int y) => y;
    }
}
