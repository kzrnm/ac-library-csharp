using AtCoder;

var (n, q) = Console.ReadLine().Split().Select(int.Parse).ToTuple2();
var dsu = new Dsu(n);

while (--q >= 0)
{
    var (t, u, v) = Console.ReadLine().Split().Select(int.Parse).ToTuple3();
    if (t == 0)
    {
        dsu.Merge(u, v);
    }
    else
    {
        Console.WriteLine(dsu.Same(u, v) ? 1 : 0);
    }
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