[CmdletBinding(DefaultParameterSetName = "OutFile")]
param(
    [Parameter(Position = 0)]
    [string]
    $ProgramPath = "$PSScriptRoot/AtCoderProgram/Program.cs",

    [Parameter(Position = 1, ParameterSetName = "OutFile")]
    [string]$OutputPath = "$PSScriptRoot/AtCoderProgram/Combined.cs",

    [Parameter(ParameterSetName = "OutConsole")]
    [switch]$Console,
    [switch]$UseRelease
)
$AclProjectPath = "$PSScriptRoot/AtCoderLibrary"
$csprojPath = "$AclProjectPath/AtCoderLibrary.csproj"

$buildType = "Debug"
if ($UseRelease) { $buildType = "Release" }

$target = @(([xml](Get-Content $csprojPath)).GetElementsByTagName('TargetFramework'))[0].InnerText
$atcoderlibPath = "$PSScriptRoot/AtCoderLibrary/bin/$buildType/$target/AtCoderLibrary.dll"

if (-not (Test-Path $atcoderlibPath) ) {
    Get-Command dotnet -ErrorAction SilentlyContinue | Out-Null
    if ($?) {
        dotnet build "$csprojPath" -c "$buildType"
        if (-not (Test-Path $atcoderlibPath) ) {
            Write-Error "not found: $atcoderlibPath"
            exit
        }
    }
    else {
        Write-Error "You need dotnet command."
        exit
    }
}

function Format-Usings {
    [OutputType([string[]])]
    param (
        [string[]]
        $usings
    )
    $list = [System.Collections.Generic.List[string]]$usings
    $list.Sort([System.Comparison[string]] { param($a, $b); [System.StringComparer]::Ordinal.Compare($a.Trim(';'), $b.Trim(';')) }) | Out-Null
    ($list | Get-Unique)
}

function Split-Code {
    param (
        [string]
        $code
    )
    $tree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($code)
    $usings = $tree.GetRoot().Usings
    @($usings | ForEach-Object { $_.ToFullString().Trim() }), $code.Substring($usings.FullSpan.End)
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

function Get-Method-Symbols {
    [OutputType([Microsoft.CodeAnalysis.IMethodSymbol[]])]
    param(
        [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]
        [Parameter(Mandatory = $true, Position = 0)]
        $tree
    )
    $semanticModel = Get-SemanticModel ([Microsoft.CodeAnalysis.SyntaxTree]$tree)
    $root = $tree.GetRoot()
    $nodes = $root.DescendantNodes()
    $symbols = $nodes  | Where-Object { 
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
function Get-ClassDeclaration-Symbols {
    [OutputType([Microsoft.CodeAnalysis.INamedTypeSymbol[]])]
    param(
        [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]
        [Parameter(Mandatory = $true, Position = 0)]
        $tree
    )
    $semanticModel = Get-SemanticModel ([Microsoft.CodeAnalysis.SyntaxTree]$tree)
    $root = $tree.GetRoot()
    $nodes = $root.DescendantNodes()
    $symbols = $nodes  | Where-Object { 
        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]
    } | ForEach-Object {
        $semanticModel.GetDeclaredSymbol($_)
    }
    $symbols
}

function Get-Acl-Paths {
    [OutputType([hashtable])]
    $res = @{}
    $aclFiles = (Get-ChildItem $AclProjectPath -Recurse "*.cs")
    $aclPaths = ($aclFiles | ForEach-Object {
            $c = Get-Content $_ -Raw
            $symbols = Get-ClassDeclaration-Symbols ([Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($c))
            foreach ($s in $symbols) {
                [pscustomobject]@{Path = $_; ClassName = $s.ToDisplayString() }
            }
        })
    foreach ($p in $aclPaths) {
        $res[$p.ClassName] += @($p.Path)
    }
    $res
}

function Expand-AtCoder-Code {
    [OutputType([string])]
    param(
        [string]
        [Parameter(Mandatory = $true, Position = 0)]
        $code
    )
    $aclPaths = (Get-Acl-Paths)
    $usings, $origBody = (Split-Code $code)

    $addedFiles = [System.Collections.Generic.HashSet[string]]::new()
    $addedFiles.Add($null) | Out-Null

    $atlBody = ""

    $i = 0
    while ($true) {
        $updated = $false
        $tree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($code)
        $symbols = (Get-Method-Symbols $tree)
        
        foreach ($symbol in $symbols) {
            $className = $symbol.ContainingType.ToDisplayString();
            
            if ($aclPaths.ContainsKey($className)) {
                $updated = $true
                Write-Debug "class: $className"
                foreach ($file in $aclPaths[$className]) {
                    if ($addedFiles.Add($file)) {
                        Write-Debug "file: $file"
                        $libUsings, $libBody = (Split-Code (Get-Content $file -Raw))
                        $code = ($libUsings -join "`n") + "`n" + $code + "`n" + ($libBody -join "`n")
                        
                        $usings += @($libUsings)
                        $atlBody += "`n" + $libBody.Trim()
                    }
                }
                $aclPaths.Remove($className)
            }
        }
        if (-not $updated) { break }
        if ($i++ -gt 1000) {
            Write-Error "Failed to expand"
            exit
        }
    }
    $usings = @(Format-Usings $usings)
    $res = $usings
    $res += @($origBody)
    $res += @("#region AtCoderLibrary")
    $res += @($atlBody)
    $res += @("#endregion AtCoderLibrary`n")
    $res = ($res -join "`n")
    $res
}

$code = (Get-Content -Raw $ProgramPath)
$newCode = (Expand-AtCoder-Code $code)

if ($Console) {
    Write-Output $newCode
}
else {
    $newCode | Out-File -FilePath $OutputPath -Encoding ([System.Text.Encoding]::UTF8) -NoNewline
}