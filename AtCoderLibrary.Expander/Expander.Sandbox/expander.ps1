[CmdletBinding()]
param (
    [Parameter()]
    [switch]
    $Watch
)

New-Item -ItemType Directory "$PSScriptRoot/bin/watchcache/" -Force | Out-Null
Copy-Item "$PSScriptRoot/bin/Debug/netcoreapp3.1/*.dll" "$PSScriptRoot/bin/watchcache/" -ErrorAction Ignore
Add-Type -Path (Get-ChildItem "$PSScriptRoot/bin/watchcache/*.dll")

function Timeout {
    param (
        [Parameter(Mandatory = $true, Position = 0)]
        [int]
        $Seconds,
        [Parameter(Mandatory = $true, Position = 1)]
        [scriptblock]
        $Body
    )
    $job = (Start-Job $Body)
    Wait-Job -Job $job -Timeout $Seconds
    Remove-Job $job -Force
}

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
            Write-Host "Update Start  [$([datetime]::Now)]: $ProgramPath"
            Timeout -Seconds 5 { [AtCoder.Expander]::Expand($ProgramPath, $OutputPath, $false , [AtCoder.ExpandMethod]::Strict) }
            Write-Host "Update Finish [$([datetime]::Now)]: $ProgramPath"
        }
        Start-Sleep -Seconds 5
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

    if ($ProgramPath.Exists) {
        Write-Host "Update Start  [$([datetime]::Now)]: $ProgramPath"
        [AtCoder.Expander]::Expand($ProgramPath, $OutputPath, $false , [AtCoder.ExpandMethod]::Strict)
        Write-Host "Update Finish [$([datetime]::Now)]: $ProgramPath"
    }
}

if ($Watch) {
    Watch-File "$PSScriptRoot/Program.cs"    
}
else {
    Expand-File "$PSScriptRoot/Program.cs" 
}