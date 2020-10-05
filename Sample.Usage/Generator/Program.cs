using System;
using System.Reflection;
using AtCoder;


class Program
{
    public static void Main()
    {
        Console.WriteLine(default(Op).Operate(0, 3));
    }
}
struct Op : ISegtreeOperator<int>
{
    public int Identity => 0;
    public int Operate(int x, int y) => x + y;
}
