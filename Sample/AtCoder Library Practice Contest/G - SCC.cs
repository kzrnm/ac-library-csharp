using AtCoder;

var (n, m) = Console.ReadLine().Split().Select(int.Parse).ToTuple2();
var scc = new SccGraph(n);

while (--m >= 0)
{
    var (a, b) = Console.ReadLine().Split().Select(int.Parse).ToTuple2();
    scc.AddEdge(a, b);
}

var groups = scc.Scc();
Console.WriteLine(groups.Length);
foreach (var g in groups)
{
    Console.WriteLine(string.Join(' ', g.Prepend(g.Length)));
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