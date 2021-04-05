using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 1 && args[0] == "expand")
        {
            foreach (var source in SourceExpander.Expanded.ExpandedContainer.Files.Values)
            {
                var dir = Path.GetDirectoryName(source.Path);
                var filename = Path.GetFileNameWithoutExtension(source.Path);
                var sb = new StringBuilder(source.Code);
                sb.AppendLine("class Program{static void Main()=>System.Console.WriteLine(" + filename + ".Calc(int.Parse(System.Console.ReadLine())));}");
                File.WriteAllText(Path.Combine(dir, "Combined", filename + ".csx"), sb.ToString());
            }
            return;
        }
        int n = 1 << 24;
        SatisfiableTwoSat.Calc(n);
    }
}
