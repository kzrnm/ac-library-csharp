using AtCoder;

int t = int.Parse(Console.ReadLine());

while (--t >= 0)
{
    var (n, m, a, b) = Console.ReadLine().Split().Select(int.Parse).ToTuple4();

    Console.WriteLine(MathLib.FloorSum(n, m, a, b));
}

static class Extension
{
    public static (T, T, T, T) ToTuple4<T>(this IEnumerable<T> t)
    {
        var e = t.GetEnumerator();
        e.MoveNext(); var v0 = e.Current;
        e.MoveNext(); var v1 = e.Current;
        e.MoveNext(); var v2 = e.Current;
        e.MoveNext(); var v3 = e.Current;
        e.Dispose();
        return (v0, v1, v2, v3);
    }
}