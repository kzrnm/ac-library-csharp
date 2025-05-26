using AtCoder;

Console.ReadLine();
var a = Console.ReadLine().Split().Select(int.Parse).ToArray();
var b = Console.ReadLine().Split().Select(int.Parse).ToArray();

Console.WriteLine(string.Join(' ', MathLib.Convolution(a, b)));