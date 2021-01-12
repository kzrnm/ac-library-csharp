using System.Runtime.CompilerServices;

namespace AtCoder
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    [IsOperator]
    public interface ICastOperator<in TFrom, out TTo>
    {
        TTo Cast(TFrom y);
    }

    public struct SameTypeCastOperator<T> : ICastOperator<T, T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Cast(T y) => y;
    }

    public struct IntToLongCastOperator : ICastOperator<int, long>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Cast(int y) => y;
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
