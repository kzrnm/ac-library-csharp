AtCoderによって公開されている[AtCoder Library](https://atcoder.jp/posts/517)のC#移植版です。

[ac-library-cs](https://github.com/key-moon/ac-library-cs) にソースコード埋め込みなどを行った拡張をしています。

## Generator(recommend)

Source Generatorsを使用してライブラリに埋め込んだソースコードを展開します。


```C#
using System;
using SourceExpander;

class Program
{
    static void Main()
    {
        Console.WriteLine(AtCoder.Math.PowMod(2, 524242, 177));
    }
}
```

## Expander

ライブラリに埋め込んだソースコードを実行時に展開します。

`Main` メソッド内で `SourceExpander.Expander.Expand` を呼び出すことで、`Combined.csx` を出力します。

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
