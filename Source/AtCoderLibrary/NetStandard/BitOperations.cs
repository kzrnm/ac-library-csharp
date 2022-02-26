#if !NETCOREAPP3_0_OR_GREATER
using System.Runtime.CompilerServices;

namespace AtCoder.Internal
{
    public static class BitOperations
    {
        [MethodImpl(256)]
        public static int PopCount(uint value)
        {
            value -= (value >> 1) & 0x_55555555u;
            value = (value & 0x_33333333u) + ((value >> 2) & 0x_33333333u);
            value = (((value + (value >> 4)) & 0x_0F0F0F0Fu) * 0x_01010101u) >> 24;
            return (int)value;
        }

        [MethodImpl(256)]
        public static int TrailingZeroCount(uint value)
        {
            value |= value << 1;
            value |= value << 2;
            value |= value << 4;
            value |= value << 8;
            value |= value << 16;
            return 32 - PopCount(value);
        }
        [MethodImpl(256)]
        public static int Log2(uint value)
        {
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            return PopCount(value) - 1;
        }
    }
}
#endif
