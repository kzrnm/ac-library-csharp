partial class Program
{
    static void Main()
    {
#if EMBEDDING
        System.Console.WriteLine(SourceExpander.Testing.AtCoder.MathLib.PowMod(255, 1L << 52, 12));
#endif
    }
}
