`ac-library-csharp` の使用方法です。

## Generator(recommend)

Source Generatorsを使用してライブラリに埋め込んだソースコードを展開します。

```
Install-Package Microsoft.Net.Compilers.Toolset
Install-Package SourceExpander.Generator
Install-Package ac-library-csharp
```

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

```
Install-Package SourceExpander
Install-Package ac-library-csharp
```

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
