AtCoderによって公開されている[AtCoder Library](https://atcoder.jp/posts/517)のC#移植版です。

[ac-library-cs](https://github.com/key-moon/ac-library-cs) にソースコード埋め込みなどを行った拡張をしています。

## Usage

see [AtCoderLibrary.Sample](/AtCoderLibrary.Sample/).

```C#
using System;
using SourceExpander;

class Program
{
    static void Main()
    {
        Expander.Expand(expandMethod: ExpandMethod.Strict);
        Console.WriteLine(AtCoder.Math.PowMod(2, 524242, 177));
    }
}
```