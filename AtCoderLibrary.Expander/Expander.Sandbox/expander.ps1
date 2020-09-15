mkdir "$PSScriptRoot/bin/watchcache/" -Force | Out-Null
Copy-Item "$PSScriptRoot/bin/Debug/netcoreapp3.1/*.dll" "$PSScriptRoot/bin/watchcache/" -ErrorAction Ignore
Add-Type -Path (Get-ChildItem "$PSScriptRoot/bin/watchcache/*.dll")

function Watch-File {
    param (
        [Parameter(Mandatory = $true, Position = 0)]
        [System.IO.FileInfo]
        $ProgramPath,
        [Parameter(Mandatory = $false, Position = 1)]
        [System.IO.FileInfo]
        $OutputPath
    )
    if (-not $OutputPath) {
        $OutputPath = (Join-Path $ProgramPath.DirectoryName "Combined.csx")
    }

    $lastUpdate = $null
    while ($ProgramPath.Exists) {
        if ($lastUpdate -ne $ProgramPath.LastWriteTime) {
            $lastUpdate = $ProgramPath.LastWriteTime
            Write-Host "Update Start  [$lastUpdate]: $OutputPath"
            [AtCoder.Expander]::Expand($ProgramPath, $OutputPath, $false , [AtCoder.ExpandMethod]::Strict)
            Write-Host "Update Finish [$lastUpdate]: $OutputPath"
        }
        Start-Sleep -Seconds 2
        $ProgramPath = (Get-ChildItem $ProgramPath)
    }
}
function Expand-File {
    param (
        [Parameter(Mandatory = $true, Position = 0)]
        [System.IO.FileInfo]
        $ProgramPath,
        [Parameter(Mandatory = $false, Position = 1)]
        [System.IO.FileInfo]
        $OutputPath
    )
    if (-not $OutputPath) {
        $OutputPath = (Join-Path $ProgramPath.DirectoryName "Combined.csx")
    }

    $lastUpdate = $null
    if ($ProgramPath.Exists) {
        $lastUpdate = $ProgramPath.LastWriteTime
        Write-Host "Update Start  [$lastUpdate]: $OutputPath"
        [AtCoder.Expander]::Expand($ProgramPath, $OutputPath, $false , [AtCoder.ExpandMethod]::Strict)
        Write-Host "Update Finish [$lastUpdate]: $OutputPath"
    }
}

# Expand-File "$PSScriptRoot/Program.cs"
Watch-File "$PSScriptRoot/Program.cs"