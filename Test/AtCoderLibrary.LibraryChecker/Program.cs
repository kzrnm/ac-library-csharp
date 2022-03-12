using System;
using System.Reflection;
using Kzrnm.Competitive.LibraryChecker;

class Program
{
    static void Main(string[] args)
    {
        var name = args[0];
        CompetitiveSolvers.RunSolver(Assembly.GetExecutingAssembly(), name, Console.OpenStandardInput(), Console.OpenStandardOutput());
    }
}
