# Usage of `ac-library-csharp`

## Installation

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
