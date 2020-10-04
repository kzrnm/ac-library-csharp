using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;

class EntryPoint
{
    static void Main()
    {
        Program.Main();
        //var files = Expanded.Expanded.Files; // can build but error CS0103

        var files = (IReadOnlyDictionary<string, string>)Type.GetType("Expanded.Expanded").GetProperty("Files", BindingFlags.Public | BindingFlags.Static).GetValue(null);

        var expanded = files.First(p => p.Key.EndsWith("Program.cs")).Value;
        //Console.Error.WriteLine(expanded);
        File.WriteAllText(GetCurrentPath().Replace("EntryPoint.cs", "Combined.csx"), expanded);
    }

    static string GetCurrentPath([CallerFilePath] string path = "") => path;
}
