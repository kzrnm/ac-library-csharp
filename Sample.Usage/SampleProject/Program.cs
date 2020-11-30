using System;
using AtCoder;

class Program
{
    static void Main()
    {
        SourceExpander.Expander.Expand();

        var line = Console.ReadLine().Split(' ');
        var n = int.Parse(line[0]);
        var Q = int.Parse(line[1]);

        var dsu = new DSU(n);

        for (int i = 0; i < Q; i++)
        {
            line = Console.ReadLine().Split(' ');
            int t = int.Parse(line[0]);
            int u = int.Parse(line[1]);
            int v = int.Parse(line[2]);
            if (t == 0)
            {
                dsu.Merge(u, v);
            }
            else
            {
                Console.WriteLine(dsu.Same(u, v) ? 1 : 0);
            }
        }
    }
}
