using System.Runtime.CompilerServices;

namespace AtCoder.Operators
{
    [IsOperator]
    public interface ICastOperator<in TFrom, out TTo>
    {
        TTo Cast(TFrom y);
    }

    public struct SameTypeCastOperator<T> : ICastOperator<T, T>
    {
        [MethodImpl(256)]
        public T Cast(T y) => y;
    }

    public struct IntToLongCastOperator : ICastOperator<int, long>
    {
        [MethodImpl(256)]
        public long Cast(int y) => y;
    }
}
