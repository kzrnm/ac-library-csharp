﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using AtCoder.Embedded;
using AtCoder.Internal;

namespace AtCoder
{
    public static class Expander
    {

        internal static readonly AclFileInfo ExpanderFileInfo
            = new AclFileInfo(
                "Expander",
                new string[] { "AtCoder.Expander" },
                new string[] { "using System.Diagnostics;" },
                Array.Empty<string>(),
                "namespace AtCoder { public static class Expander {[Conditional(\"DEBUG\")] public static void Expand(string a = \"\", string b = \"\", bool c = true) { } } }");

        internal static string ToCombinedFilePath(string origFilePath)
            => Path.Combine(Path.GetDirectoryName(origFilePath)!, "Combined.csx");

        public static void Expand([CallerFilePath] string? inputFilePath = null, string? outputFilePath = null, bool checkLastWriteTime = true)
        {
            if (inputFilePath == null) throw new ArgumentNullException(nameof(inputFilePath));

            var inputFileInfo = new FileInfo(inputFilePath);
            if (!inputFileInfo.Exists) throw new ArgumentException($"Not found: {inputFileInfo}", nameof(inputFileInfo));

            if (outputFilePath == null)
            {
                var dir = Path.GetDirectoryName(inputFilePath);
                if (dir == null) throw new ArgumentException("invalid path", nameof(inputFilePath));
                outputFilePath = Path.Combine(dir, "Combined.csx");
            }

            var head = $"// Last: {inputFileInfo.LastWriteTimeUtc.Ticks}";
            if (checkLastWriteTime)
            {
                using var fs = new FileStream(outputFilePath, FileMode.OpenOrCreate);
                using var sr = new StreamReader(fs);
                if (sr.ReadLine() == head) return;
            }
            var code = File.ReadAllText(inputFilePath);
            var newCode = ExpandCode(code);

            using var outFs = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            using var outTx = new StreamWriter(outFs);

            if (checkLastWriteTime)
            {
                outTx.WriteLine(head);
            }

            using var stringReader = new StringReader(newCode);
            string? line = stringReader.ReadLine();
            while (line != null)
            {
                outTx.WriteLine(line);
                line = stringReader.ReadLine();
            }
        }

        private static string ExpandCode(string origCode)
            => new PaseAndExpander(origCode,
                AutoGenerated__SourceInfo.FileInfo.Select(f => new AclFileInfoDetail(f)).Append(new AclFileInfoDetail(ExpanderFileInfo)))
            .Expand();
    }
}
