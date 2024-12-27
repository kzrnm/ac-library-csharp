using AtCoder;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

DynamicModInt<ValueTuple>.Mod = 998244353;

var (n, Q) = Console.ReadLine().Split().Select(int.Parse).ToTuple2();
var seg = new LazySegtree<S, Affine, Op>(Console.ReadLine().Split().Select(v => new S(int.Parse(v))).ToArray());
for (int q = 0; q < Q; q++)
{
    var line = Console.ReadLine().Split().Select(int.Parse).ToArray();
    int t = line[0];
    int l = line[1];
    int r = line[2];
    if (t == 0)
    {
        var b = line[3];
        var c = line[4];
        seg.Apply(l, r, new(b, c));
    }
    else
        Console.WriteLine(seg[l..r].Value);
}

readonly record struct Affine(DynamicModInt<ValueTuple> b, DynamicModInt<ValueTuple> c);
readonly record struct S(DynamicModInt<ValueTuple> Value, uint Length = 1);

readonly struct Op : ILazySegtreeOperator<S, Affine>
{
    [MethodImpl(256)]
    public S Operate(S x, S y) => new(x.Value + y.Value, x.Length + y.Length);
    [MethodImpl(256)]
    public S Mapping(Affine f, S x) => new(f.b * x.Value + f.c * x.Length, x.Length);
    [MethodImpl(256)]
    public Affine Composition(Affine f, Affine g) => new(f.b * g.b, f.b * g.c + f.c);
    public S Identity => new(0, 0);
    public Affine FIdentity => new(1, 0);
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
}