using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        if (args.SequenceEqual(new[] { "expand" }))
            foreach (var source in SourceExpander.Expanded.ExpandedContainer.Files.Values)
            {
                Console.WriteLine(source.Path);
                var dir = Path.GetDirectoryName(source.Path);
                var filename = Path.GetFileNameWithoutExtension(source.Path);
                var sb = new StringBuilder(source.Code);
                sb.AppendLine("class Program{static void Main()=>System.Console.WriteLine(" + filename + ".Calc(1<<24));}");
                File.WriteAllText(Path.Combine(dir, "Combined", filename + ".csx"), sb.ToString());
            }
        SuffixArrayLong.Calc(n);
    }
    public const int n = 1 << 24;
}
