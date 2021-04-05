using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

string CurrentPath([CallerFilePath] string path = "") => path;

var classes = new[]
{
    "Convolution",
    "FenwickTree",
    "LazySegtree",
    "LazySegtreeMaxRight",
    "McfGraph",
    "MfGraph",
    "MfGraph1",
    "SCC",
    "SatisfiableTwoSat",
    "Segtree",
    "SegtreeMaxRight",
    "SuffixArrayLong",
    "SuffixArrayString",
    "ZAlgorithm",
};

var current = CurrentPath();
var benchmarkDir = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(current)), "Benchmark");
var csharpFiles = Directory.GetFiles(Path.Combine(benchmarkDir, "Combined"), "*.csx", SearchOption.TopDirectoryOnly).ToDictionary(Path.GetFileNameWithoutExtension);
var cppFiles = Directory.GetFiles(Path.Combine(benchmarkDir, "C++"), "*.cpp", SearchOption.TopDirectoryOnly).ToDictionary(Path.GetFileNameWithoutExtension);

Console.WriteLine("Url encoded AtCoder cookie:");
var cookie = Console.ReadLine();

var handler = new HttpClientHandler()
{
    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
    UseCookies = true,
};
var client = new HttpClient(handler);
client.DefaultRequestHeaders.Add("Cookie", cookie);
client.DefaultRequestHeaders.AcceptEncoding.Add(new("gzip"));
client.DefaultRequestHeaders.AcceptEncoding.Add(new("deflate"));
var csrfToken = System.Text.RegularExpressions.Regex.Match(HttpUtility.UrlDecode(cookie), @"csrf_token:([^\0]+)").Groups[1].Value;

using var sw = new StreamWriter(Path.Combine(Path.GetDirectoryName(current), "Result.txt"));
Console.CancelKeyPress += (_, _) => sw.Close();

sw.WriteLine("name\tC++\tC#");
foreach (var name in classes)
{
    var cpp = await PostCustomTestAsync(cppFiles[name], 4003);
    Console.WriteLine($"C++: {name} {cpp}");
    var cs = await PostCustomTestAsync(csharpFiles[name], 4010);
    Console.WriteLine($"C#: {name} {cs}");
    sw.WriteLine($"{name}\t{cpp}\t{cs}");
}
return;

async Task<int> PostCustomTestAsync(string sourceFile, int langId)
{
    var content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "data.LanguageId", langId.ToString() },
        { "sourceCode", File.ReadAllText(sourceFile) },
        { "input", "16777216" },
        { "csrf_token", csrfToken },
    });
    var postres = await client.PostAsync("https://atcoder.jp/contests/arc116/custom_test/submit/json", content);
    await Task.Delay(15_000);
    var res = await client.GetAsync("https://atcoder.jp/contests/arc116/custom_test/json");
    var dec = await JsonSerializer.DeserializeAsync<CustomTestResponse>(await res.Content.ReadAsStreamAsync());

    return dec.Result.TimeConsumption;
}

class CustomTestResponse
{
    public CustomTestResult Result { set; get; }
}
class CustomTestResult
{
    public int TimeConsumption { set; get; }
}
