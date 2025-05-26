using AtCoder;

var s = Console.ReadLine();
var sa = StringLib.SuffixArray(s);

Console.WriteLine((long)(s.Length + 1) * s.Length / 2 - StringLib.LcpArray(s, sa).Select(i => (long)i).Sum());