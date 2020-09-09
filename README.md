AtCoderによって公開されている[AtCoder Library](https://atcoder.jp/posts/517)のC#移植版です。

## ジャッジへの提出

`expander.ps1`を用意しています。PowerShell Core 7.0以上が必要です。

```powershell
# AtCoderProgram/Program.csを元にライブラリを展開したAtCoderProgram/Combined.csを作成
pwsh expander.ps1

# AtCoderProgram/Program.csを元にライブラリを展開したAtCoderProgram/Combined.csを作成
pwsh expander.ps1 AtCoderProgram/Program2.cs

# AtCoderProgram/Program.csを元にライブラリを展開したAtCoderProgram/Combined.csを作成
pwsh expander.ps1 AtCoderProgram/Program2.cs AtCoderProgram/Program3.cs

# AtCoderProgram/Program.csを元にライブラリを展開したコードを標準出力に表示
pwsh expander.ps1
```