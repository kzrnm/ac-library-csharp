using AtCoder;

var (n, K) = Console.ReadLine().Split().Select(int.Parse).ToTuple2();

var g = new McfGraph<int, long>(2 * n + 2);
int s = 2 * n, t = 2 * n + 1;

// we can "waste" the flow
g.AddEdge(s, t, n * K, int.MaxValue);

for (int i = 0; i < n; i++)
{
    g.AddEdge(s, i, K, 0);
    g.AddEdge(n + i, t, K, 0);
}

for (int i = 0; i < n; i++)
{
    var line = Console.ReadLine().Split().Select(int.Parse).ToArray();
    for (int j = 0; j < line.Length; j++)
    {
        long a = line[j];
        g.AddEdge(i, n + j, 1, int.MaxValue - a);
    }
}
var result = g.Flow(s, t, n * K);
Console.WriteLine((long)n * K * int.MaxValue - result.cost);

var grid = Enumerable.Range(0, n).Select(MakeLine).ToArray();
foreach (var e in g.Edges())
{
    if (e.From == s || e.To == t || e.Flow == 0) continue;
    grid[e.From][e.To - n] = 'X';
}
foreach (var line in grid)
{
    Console.WriteLine(line);
}
char[] MakeLine(int _)
{
    var rt = new char[n];
    rt.AsSpan().Fill('.');
    return rt;
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