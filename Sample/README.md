AtCoderによって公開されている[AtCoder Library](https://atcoder.jp/posts/517)のC#移植版です。

[ac-library-cs](https://github.com/key-moon/ac-library-cs) にソースコード埋め込みなどを行った拡張をしています。

## Generator(recommend)

Source Generatorsを使用してライブラリに埋め込んだソースコードを展開します。

```xml:csproj
  <PropertyGroup>
    <RestoreAdditionalProjectSources>https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json ;$(RestoreAdditionalProjectSources)</RestoreAdditionalProjectSources>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Net.Compilers.Toolset" Version="3.8.0-4.20480.4">
      <!-- use Generators in LangVersion < 9 -->
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="SourceExpander.Generator" Version="1.1.0-beta.6" PrivateAssets="all" />
    <PackageReference Include="ac-library-csharp" Version="0.1.1" />
  </ItemGroup>
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

`Main` メソッド内で `SourceExpander.Expander.Expand` を呼び出すことで、`Combined.csx` を出力します。


```xml:csproj
  <ItemGroup>
    <PackageReference Include="ac-library-csharp" Version="0.1.1" />
    <PackageReference Include="SourceExpander" Version="1.1.0-beta.6" />
    <PackageReference Include="System.Runtime.CompilerServices.Unsafe" Version="4.7.0" />
  </ItemGroup>
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
