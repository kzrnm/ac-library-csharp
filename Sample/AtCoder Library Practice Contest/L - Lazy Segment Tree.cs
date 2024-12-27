using AtCoder;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

var (n, Q) = Console.ReadLine().Split().Select(int.Parse).ToTuple2();
var A = Console.ReadLine().Split().Select(c => c == "1" ? new P(0, 1) : new P(1, 0)).ToArray();
var seg = new LazySegtree<P, bool, Op>(A);

while (--Q >= 0)
{
    var (t, l, r) = Console.ReadLine().Split().Select(int.Parse).ToTuple3();
    --l;

    if (t == 1)
        seg.Apply(l, r, true);
    else
        Console.WriteLine(seg[l..r].Sum);
}

readonly record struct P(int Zero, int One, long Sum = 0, long Inv = 0);

readonly record struct Op : ILazySegtreeOperator<P, bool>
{
    [MethodImpl(256)]
    public P Operate(P x, P y) => new(x.Zero + y.Zero, x.One + y.One, x.Sum + y.Sum + (long)x.One * y.Zero, x.Inv + y.Inv + (long)y.One * x.Zero);
    [MethodImpl(256)]
    public P Mapping(bool f, P x) => f ? new(x.One, x.Zero, x.Inv, x.Sum) : x;
    [MethodImpl(256)]
    public bool Composition(bool nf, bool cf) => nf != cf;

    public P Identity => default;
    public bool FIdentity => default;
}
static class Extension
{
    public static (T, T) ToTuple2<T>(this IEnumerable<T> t)
    {
        var e = t.GetEnumerator();
        e.MoveNext(); var v0 = e.Current;
        e.MoveNext(); var v1 = e.Current;
        e.Dispose();
        return (v0, v1);
    }
    public static (T, T, T) ToTuple3<T>(this IEnumerable<T> t)
    {
        var e = t.GetEnumerator();
        e.MoveNext(); var v0 = e.Current;
        e.MoveNext(); var v1 = e.Current;
        e.MoveNext(); var v2 = e.Current;
        e.Dispose();
        return (v0, v1, v2);
    }
}