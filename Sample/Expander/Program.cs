using System;
using AtCoder;
using System.Reflection;
class Program
{
    static void Main()
    {
        SourceExpander.Expander.Expand(expandMethod: SourceExpander.ExpandMethod.Strict);
        var fw = new IntFenwickTree(10);
        for (int i = 0; i < 10; i++)
            fw.Add(i, i + 1);
        Console.WriteLine(fw.Sum(0, 10));
    }
}
