using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AtCoder;
using static AtCoder.Internal.InternalMath;
using FenwickTree = AtCoder.IntFenwickTree;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();

            // Default
            Expander.Expand();
            //// Same
            // Expander.Expand(expandMethod: ExpandMethod.All);

            //// Parse SyntaxTree with Roslyn
            // Expander.Expand(expandMethod: ExpandMethod.NameSyntax);
            // Expander.Expand(expandMethod: ExpandMethod.Strict);


            //// do nothing if file is not updated.
            // Expander.Expand(checkLastWriteTime: true);


            Console.WriteLine($"expand time: {sw.ElapsedMilliseconds} ms");
            var fw = new FenwickTree(5);
            for (int i = 0; i < 5; i++) fw.Add(i, i + 1);
            Console.WriteLine(fw.Sum(0, 5));
            Console.WriteLine(CeilPow2(15));
            Console.WriteLine(nameof(AtCoder.String.ZAlgorithm));
            Console.WriteLine($"finish time: {sw.ElapsedMilliseconds} ms");
        }
    }
}

struct Monoid : IMonoidOperator<int>
{
    public int Identity => 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Operate(int x, int y) => x + y;
}
class Segtree : Segtree<int, Monoid> { public Segtree(int n) : base(n) { } }
