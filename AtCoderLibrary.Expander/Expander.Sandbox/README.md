## Program.cs

実際にコードを記述するファイルのサンプル。

`Main1` では、ファイルが呼ばれる度に Expand が実行される。

`Main2` では、Release ビルドでのみ Expand が実行される。`watch.sh`, `watch.cmd` とセットで使用するのが良い。

## expander.ps1

DLL を読み込んで、Program.cs を対象に Expand を実行する PowerShell スクリプト。PowerShell 7.0 以降を想定。

一度だけ実行する関数とファイル更新を監視する関数がある。

## watch.sh, watch.ps1

Program.cs を監視して Expand を実行するスクリプト。

`watch.ps1` は Windows PowerShell でも動作する。