using AtCoder;
using static System.Math;

var (n, D) = Console.ReadLine().Split().Select(int.Parse).ToTuple2();
(int X, int Y)[] XY = Enumerable.Range(0, n).Select(_ => Console.ReadLine().Split().Select(int.Parse).ToTuple2()).ToArray();
var ts = new TwoSat(n);
for (int i = 0; i < XY.Length; i++)
{
    var (x1, y1) = XY[i];
    for (int j = i + 1; j < XY.Length; j++)
    {
        var (x2, y2) = XY[j];
        if (Abs(x1 - x2) < D) ts.AddClause(i, true, j, true);
        if (Abs(x1 - y2) < D) ts.AddClause(i, true, j, false);
        if (Abs(y1 - x2) < D) ts.AddClause(i, false, j, true);
        if (Abs(y1 - y2) < D) ts.AddClause(i, false, j, false);
    }
}
if (ts.Satisfiable())
{
    Console.WriteLine("Yes");
    var rt = ts.Answer();
    foreach (var v in rt.Zip(XY, (b, t) => b ? t.Y : t.X))
        Console.WriteLine(v);
}
else
{
    Console.WriteLine("No");
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