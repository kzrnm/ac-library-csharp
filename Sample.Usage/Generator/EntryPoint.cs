using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

class EntryPoint
{
    static void Main()
    {
        Program.Main();
        //var files = Expanded.Expanded.Files; // can build but error CS0103

        var files = (dynamic)Type.GetType("Expanded.Expanded").GetProperty("Files", BindingFlags.Public | BindingFlags.Static).GetValue(null);

        foreach (var p in files)
        {
            if (p.Key.EndsWith("Program.cs"))
            {
                File.WriteAllText(GetCurrentPath().Replace("EntryPoint.cs", "Combined.csx"), p.Value.Code);
            }
        }

        //Console.Error.WriteLine(expanded);
    }

    static string GetCurrentPath([CallerFilePath] string path = "") => path;
}
