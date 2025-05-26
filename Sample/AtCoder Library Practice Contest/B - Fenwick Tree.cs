using AtCoder;

var (n, q) = Console.ReadLine().Split().Select(int.Parse).ToTuple2();
var fw = new FenwickTree<long>(n);
var line = Console.ReadLine().Split().Select(int.Parse).ToArray();
for (var i = 0; i < n; i++)
    fw.Add(i, line[i]);

while (--q >= 0)
{
    var (t, l, r) = Console.ReadLine().Split().Select(int.Parse).ToTuple3();
    if (t == 0)
    {
        fw.Add(l, r);
    }
    else
    {
        Console.WriteLine(fw[l..r]);
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