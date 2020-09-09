[CmdletBinding(DefaultParameterSetName = "OutFile")]
param(
    [Parameter(Position = 0)]
    [string]
    $ProgramPath = "$PSScriptRoot/AtCoderProgram/Program.cs",

    [Parameter(Position = 1)]
    [Parameter(ParameterSetName = "OutFile")]
    [string]$OutputPath = "$PSScriptRoot/AtCoderProgram/Combined.cs",
    
    [Parameter(ParameterSetName = "OutConsole")]
    [switch]$Console,
    [switch]$UseRelease
)
$csprojPath = "$PSScriptRoot/AtCoderLibrary/AtCoderLibrary.csproj"

$buildType = "Debug"
if ($UseRelease) { $buildType = "Release" }

$target = @(([xml](Get-Content $csprojPath)).GetElementsByTagName('TargetFramework'))[0].InnerText
$atcoderlibPath = "$PSScriptRoot/AtCoderLibrary/bin/$buildType/$target/AtCoderLibrary.dll"

if (-not (Test-Path $atcoderlibPath) ) {
    Get-Command dotnet -ErrorAction SilentlyContinue | Out-Null
    if ($?) {
        dotnet build "$csprojPath" -c "$buildType"
    }
    else {
        Write-Error "You need dotnet command."
        exit
    }
}

function Merge-Hashtable {
    [OutputType([hashtable])]
    param (
        [hashtable[]]
        $Hashtables
    )
    $res = @{}
    foreach ($ht in $Hashtables) {
        foreach ($key in $ht.Keys) {
            $res[$key] = $ht[$key]
        }
    }
    return $res
}
function Get-SemanticModel {
    [OutputType([Microsoft.CodeAnalysis.SemanticModel])]
    param(
        [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]
        [Parameter(Mandatory = $true, Position = 0)]
        $tree
    )
    $mscorlib = [Microsoft.CodeAnalysis.MetadataReference]::CreateFromFile([System.Object].GetTypeInfo().Assembly.Location)
    $atcoderlib = [Microsoft.CodeAnalysis.MetadataReference]::CreateFromFile($atcoderlibPath)

    $compilation = [Microsoft.CodeAnalysis.CSharp.CSharpCompilation]::Create(
        "SemanticModel", 
        [Microsoft.CodeAnalysis.SyntaxTree[]]@($tree), 
        [Microsoft.CodeAnalysis.MetadataReference[]]@($mscorlib, $atcoderlib), 
        $null);
    return $compilation.GetSemanticModel([Microsoft.CodeAnalysis.SyntaxTree]$tree)
}
function Get-AtCoder-Method-Symbols {
    [OutputType([Microsoft.CodeAnalysis.IMethodSymbol[]])]
    param(
        [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]
        [Parameter(Mandatory = $true, Position = 0)]
        $tree
    )
    $semanticModel = Get-SemanticModel ([Microsoft.CodeAnalysis.SyntaxTree]$tree)
    $root = $tree.GetRoot()
    $symbols = $root.DescendantNodes() | Where-Object { 
        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax]
    } | ForEach-Object {
        $semanticModel.GetSymbolInfo($_)
    } | Where-Object { 
        $_.Symbol -is [Microsoft.CodeAnalysis.IMethodSymbol]
    } | ForEach-Object {
        $_.Symbol
    }
    $symbols
}

function Split-Code {
    param (
        [string[]]
        $lines
    )
    $headUsing = @()
    $bodies = @()
    $inHead = $true

    foreach ($line in $lines) {
        if ($inHead) {
            if ($line.Trim().StartsWith("using")) {
                $headUsing += $line
            }
            elseif (-not [string]::IsNullOrWhiteSpace($line)) {
                $bodies += $line
                $inHead = $false
            } 
        }
        else {
            $bodies += $line
        }
    }

    $headUsing, $bodies
}

function Expand-AtCoder-Code {
    [OutputType([string])]
    param(
        [string]
        [Parameter(Mandatory = $true, Position = 0)]
        $code
    )
    $aclpath = Merge-Hashtable (
        Get-ChildItem "$PSScriptRoot/aclpath.*.json" | ForEach-Object { 
            Get-Content $_  | ConvertFrom-Json -AsHashtable
        })
    $lineBreak = $code.Contains("`r`n") ? "`r`n" : "`n";
    $addedFiles = [System.Collections.Generic.HashSet[string]]::new()
    $code += $lineBreak + "#region AtCoderLibrary" + $lineBreak
    while ($true) {
        $updated = $false
        $tree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($code)
        $symbols = Get-AtCoder-Method-Symbols $tree
        
        foreach ($symbol in $symbols) {
            $className = $symbol.ContainingType.ToDisplayString();
            
            if ($aclpath.ContainsKey($className)) {
                $updated = $true
                foreach ($file in $aclpath[$className]) {
                    if ($addedFiles.Add($file)) {
                        Write-Debug $file
                        $headUsing, $bodies = (Split-Code @(Get-Content $file))
                        $code = ($headUsing -join $lineBreak) + $lineBreak + $code + $lineBreak + ($bodies -join $lineBreak) 
                    }
                }
                $aclpath.Remove($className)
            }
        }
        if (-not $updated) { break }
    }
    $code += $lineBreak + "#endregion AtCoderLibrary"
    $lines = $code.Split($lineBreak)
    $headUsing, $bodies = (Split-Code $lines)
    $headUsing = [System.Collections.Generic.List[string]]$headUsing
    $headUsing.Sort([System.Comparison[string]] { param($a, $b); [System.StringComparer]::Ordinal.Compare($a.Trim(';'), $b.Trim(';')) })
    $headUsing = $headUsing | Get-Unique

    ($headUsing -join $lineBreak) + $lineBreak + $lineBreak + ($bodies -join $lineBreak)
}


$code = (Get-Content -Raw $ProgramPath)
$newCode = (Expand-AtCoder-Code $code)

if ($Console) {
    Write-Output $newCode
}
else {
    $newCode | Out-File -FilePath $OutputPath -Encoding ([System.Text.Encoding]::UTF8)
}