using AtCoder;

var (n, m) = Console.ReadLine().Split().Select(int.Parse).ToTuple2();
var S = Enumerable.Range(0, n).Select(_ => Console.ReadLine().ToCharArray()).ToArray();
ReadOnlySpan<(int, int)> moves = [
    (0, -1),
    (0, +1),
    (-1, 0),
    (+1, 0),
];

var mf = new MfGraph<int>(n * m + 2);

var start = n * m;
var goal = start + 1;
for (int h = 0; h < n; h++)
    for (int w = 0; w < m; w++)
    {
        var ix = h * m + w;
        if (S[h][w] != '.') continue;
        if ((h ^ w) % 2 != 0)
        {
            foreach (var (mh, mw) in moves)
            {
                var dh = h + mh;
                var dw = w + mw;

                if ((uint)dh < n && (uint)dw < m && S[dh][dw] == '.')
                {
                    var dx = dh * m + dw;
                    mf.AddEdge(ix, dx, 1);
                }
            }
            mf.AddEdge(start, ix, 1);
        }
        else
            mf.AddEdge(ix, goal, 1);
    }
Console.WriteLine(mf.Flow(start, goal));
foreach (var e in mf.Edges())
{
    if (e.From >= start || e.To >= start || e.Flow == 0) continue;
    var ix1 = e.From;
    var ix2 = e.To;
    if (ix1 > ix2) (ix1, ix2) = (ix2, ix1);
    if (ix1 + m != ix2)
    {
        S[ix1 / m][ix1 % m] = '>';
        S[ix2 / m][ix2 % m] = '<';
    }
    else
    {
        S[ix1 / m][ix1 % m] = 'v';
        S[ix2 / m][ix2 % m] = '^';
    }
}
foreach (var line in S)
{
    Console.WriteLine(line);
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