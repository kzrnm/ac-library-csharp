using System.Diagnostics;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace AtCoder
{
    public class IntFenwickTree : FenwickTree<int, IntOperator> { public IntFenwickTree(int n) : base(n) { } }
    public class UIntFenwickTree : FenwickTree<uint, UIntOperator> { public UIntFenwickTree(int n) : base(n) { } }
    public class LongFenwickTree : FenwickTree<long, LongOperator> { public LongFenwickTree(int n) : base(n) { } }
    public class ULongFenwickTree : FenwickTree<ulong, ULongOperator> { public ULongFenwickTree(int n) : base(n) { } }
    // TODO: public class ModIntFenwickTree : FenwickTree<ModInt, ModIntOperator> { public ModIntFenwickTree(int n) : base(n) { } }


    public class FenwickTree<TValue, TOp>
        where TValue : struct
        where TOp : struct, INumOperator<TValue>
    {
        private static readonly TOp op = default;
        private readonly TValue[] data;
        public FenwickTree(int n)
        {
            data = new TValue[n + 1];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(int p, TValue x)
        {
            Debug.Assert(unchecked((uint)p < data.Length));
            for (p++; p < data.Length; p += InternalBit.ExtractLowestSetBit(p))
            {
                data[p] = op.Add(data[p], x);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue Sum(int l, int r)
        {
            Debug.Assert(0 <= l && l <= r && r < data.Length);
            return op.Subtract(Sum(r), Sum(l));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TValue Sum(int r)
        {
            TValue s = default;
            for (; r > 0; r -= InternalBit.ExtractLowestSetBit(r))
            {
                s = op.Add(s, data[r]);
            }
            return s;
        }
    }
}
