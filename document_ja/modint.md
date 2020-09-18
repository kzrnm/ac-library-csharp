# ModInt

自動でmodを取る構造体です。パフォーマンスの都合上、`StaticModInt<T>`と`DynamicModInt<T>`の2種類が用意されています。それぞれの特徴は以下の通りです。

|型                |パフォーマンス|modを決定するタイミング            |`T`の型（後述）|
|------------------|-------------|--------------------------------|--------------|
|`StaticModInt<T>` |良い         |コンパイル時に決定している必要がある|`IStaticMod`  |
|`DynamicModInt<T>`|やや劣る     |実行時でもよい                    |`IDynamicModID`|

実行時にならないとmodが決定できない一部のケースを除き、`StaticModInt<T>`の使用を推奨します。

## 基本的な使い方

### StaticModInt&lt;T&gt;

型引数`T`に`Mod1000000007`または`Mod998244353`のいずれかを設定します。`using`を用いて別名を付けると便利です。

それ以外のmodの設定方法については、Tipsをご参照ください。

```C#
using System;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod1000000007>;   // ModIntという別名を付ける

void StaticModIntTest()
{
    var a = new ModInt(100000);
    var b = new ModInt(100000);

    var add = a + b;
    var sub = a - b;
    var mul = a * b;
    var div = a / b;

    Console.WriteLine(add);   // 200000
    Console.WriteLine(sub);   // 0
    Console.WriteLine(mul);   // 999999937
    Console.WriteLine(div);   // 1
}
```

### DynamicModInt&lt;T&gt;

型引数`T`に`ModID0`, `ModID1`, `ModID2`のいずれかを設定します。C++版と同様、ID別にmodを設定できます。

IDを4つ以上使い分けたい場合の設定方法については、Tipsをご参照ください。

```C#
using System;
using ModInt = AtCoder.DynamicModInt<AtCoder.ModID0>;   // ModIntという別名を付ける

void DynamicModIntTest()
{
    ModInt.Mod = 1000000009;        // mod 1000000009に設定
    var a = new ModInt(100000);
    var b = new ModInt(100000);

    var add = a + b;
    var sub = a - b;
    var mul = a * b;
    var div = a / b;

    Console.WriteLine(add);   // 200000
    Console.WriteLine(sub);   // 0
    Console.WriteLine(mul);   // 999999919
    Console.WriteLine(div);   // 1
}
```

## メンバー

以下に示すメソッドおよびプロパティは、`DynamicModInt<T>.Mod`を除き、`StaticModInt<T>`と`DynamicModInt<T>`のどちらでも同じように使えます。以下、便宜上`StaticModInt<T>`と`DynamicModInt<T>`をまとめて`ModInt`と表記します。

基本的にC++版ACLと使い方は同様ですので、詳細については[そちらのドキュメント](https://atcoder.github.io/ac-library/document_ja/modint.html)をお読みください。C++版とC#版の対応表を以下に示します。

|C++版|C#版|
|-----|-----|
|`modint x()`|`new ModInt()`|
|`modint x<T>(T y)`|`new ModInt(long y)`|
|`modint::set_mod(int m)`|`DynamicModInt<T>.Mod`|
|`modint::mod()`|`DynamicModInt<T>.Mod`|
|`x.val();`|`x.Value`|
|`-x`|`-x`|
|`x++`|`x++`|
|`x--`|`x--`|
|`++x`|`++x`|
|`--x`|`--x`|
|`x + y`|`x + y`|
|`x - y`|`x - y`|
|`x * y`|`x * y`|
|`x / y`|`x / y`|
|`x += y`|`x += y`|
|`x -= y`|`x -= y`|
|`x *= y`|`x *= y`|
|`x /= y`|`x /= y`|
|`x == y`|`x == y`|
|`x != y`|`x != y`|
|`x.pow(ll n)`|`x.Pow(long n)`|
|`x.inv()`|`x.Inv()`|
|`modint::raw(int x)`|`ModInt.Raw(int x)`|

## Tips

### 1000000007, 998244353以外のmod設定方法

`StaticModInt<T>`には、型引数`T`として`IStaticMod`を実装した構造体を渡すことができます。`IStaticMod`は以下のような定義となっており、`Mod`プロパティ（modの値）と`IsPrime`プロパティ（modが素数かどうか）を持ちます。

```C#
public interface IStaticMod
{
    uint Mod { get; }
    bool IsPrime { get; }
}
```

よく使われるmodとしてあらかじめ`Mod1000000007`および`Mod998244353`が用意されていますが、それ以外のmodを使いたい場合、以下のような構造体を自作して`StaticModInt<T>`に渡せばよいです。`DynamicModInt<T>.Mod`にmodを設定する方法と比べ、高速に動作します。

```C#
public struct Mod13 : IStaticMod
{
    public uint Mod => 13;
    public bool IsPrime => true;
}

void OtherModTest()
{
    var x = new StaticModInt<Mod13>(20);

    Console.WriteLine(x);   // 7
}
```

### 4種類以上のDynamicModInt&lt;T&gt;の使い分け

`DynamicModInt<T>`には、型引数`T`として`IDynamicModID`を実装した構造体を渡すことができます。`IDynamicModID`は以下のような定義を持つ空のインターフェースです。

```C#
public interface IDynamicModID { }
```

デフォルトで`ModID0`, `ModID1`, `ModID2`の3種類が用意されていますが、4種類以上の`DynamicModInt<T>`を使い分けたい場合、以下のような構造体を自作して`DynamicModInt<T>`に渡せばよいです。

```C#
public struct ModID3 : IDynamicModID { }

void MultipleModTest()
{
    DynamicMod<ModID0>.Mod = 2;
    DynamicMod<ModID1>.Mod = 3;
    DynamicMod<ModID2>.Mod = 5;
    DynamicMod<ModID3>.Mod = 7;

    var a = new DynamicModInt<ModID0>(13);
    var b = new DynamicModInt<ModID1>(13);
    var c = new DynamicModInt<ModID2>(13);
    var d = new DynamicModInt<ModID3>(13);

    Console.WriteLine(a);   // 1
    Console.WriteLine(b);   // 1
    Console.WriteLine(c);   // 3
    Console.WriteLine(d);   // 6
}
```

なお、IDが異なるmod同士の計算はできません。

### 暗黙的な型変換

`ModInt`と他の整数型（`int`型等）を計算する場合、自動で`ModInt`型に変換されます。

```C#
var x = new ModInt(10);
var y = x + 1;  // var y = x + new ModInt(1); と同じ
```

また、`ToString()`がオーバーライドされており、`Console.WriteLine()`等に渡した場合は`Value`の値が表示されます。

```C#
var x = new ModInt(10);
Console.WriteLine(x);   // 10 と表示される
```
