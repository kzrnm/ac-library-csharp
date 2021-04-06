using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
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
var currentDir = Path.GetDirectoryName(current);
var benchmarkDir = Path.Combine(Path.GetDirectoryName(currentDir), "Benchmark");
var csharpFiles = Directory.GetFiles(Path.Combine(benchmarkDir, "Combined"), "*.csx", SearchOption.TopDirectoryOnly).ToDictionary(Path.GetFileNameWithoutExtension);
var cppFiles = Directory.GetFiles(Path.Combine(benchmarkDir, "C++"), "*.cpp", SearchOption.TopDirectoryOnly).ToDictionary(Path.GetFileNameWithoutExtension);

var cancellationTokenSource = new CancellationTokenSource();

string cookie = File.Exists(Path.Combine(currentDir, "Cookie.txt")) ? File.ReadAllText(Path.Combine(currentDir, "Cookie.txt")) : null;
if (cookie is null)
{
    Console.WriteLine("Url encoded AtCoder cookie:");
    cookie = Console.ReadLine();
}

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

Console.CancelKeyPress += (_, _) =>
{
    cancellationTokenSource.Cancel();
};

foreach (var name in classes)
{
    bool cpp = false, cs = false;
    cpp = await PostPracticeAsync(cppFiles[name], 4003);
    Console.WriteLine($"C++: {name} {(cpp ? "OK" : "NG")}");
    await Task.Delay(15_000);
    cs = await PostPracticeAsync(csharpFiles[name], 4010);
    Console.WriteLine($"C#: {name} {(cs ? "OK" : "NG")}");
    await Task.Delay(15_000);
}
return;

async Task<bool> PostPracticeAsync(string sourceFile, int langId)
{
    var content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "data.TaskScreenName", "practice_1" },
        { "data.LanguageId", langId.ToString() },
        { "sourceCode", File.ReadAllText(sourceFile) },
        { "csrf_token", csrfToken },
    });
    var res = await client.PostAsync("https://atcoder.jp/contests/practice/submit", content, cancellationTokenSource.Token);

    if (!res.IsSuccessStatusCode)
        return false;
    var resContent = await res.Content.ReadAsStringAsync(cancellationTokenSource.Token);
    if (resContent.Contains("href=\"/reset_password\""))
        return false;
    return true;
}

class CustomTestResponse
{
    public CustomTestResult Result { set; get; }
}
class CustomTestResult
{
    public int TimeConsumption { set; get; }
}
