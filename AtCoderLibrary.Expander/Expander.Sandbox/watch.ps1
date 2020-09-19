if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
   Write-Host "dotnet command could not be found" -ForegroundColor Red
   exit
}

function Timeout {
   param (
      [Parameter(Mandatory = $true, Position = 0)]
      [int]
      $Seconds,
      [Parameter(Mandatory = $true, Position = 1)]
      [scriptblock]
      $Body
   )
   $job = (Start-Job $Body -InitializationScript)
   Wait-Job -Job $job -Timeout $Seconds
   if ($job.State -eq "Completed") {
      Receive-Job $job
   }
   Remove-Job $job -Force
}

$PrevLastWriteTime = [datetime]::MinValue
$ProgramPath = "$PSScriptRoot/Program.cs"

while ($true) {
   $LastWriteTime = (Get-ChildItem $ProgramPath).LastWriteTime
   if ($LastWriteTime -ne $PrevLastWriteTime) {
      Write-Host "Update Start  [$([datetime]::Now)]: $ProgramPath"
      dotnet build "$PSScriptRoot" -c Release --verbosity q

      $job = (Start-Job { param($path) dotnet run -c Release --no-build -p $path } -ArgumentList $PSScriptRoot)
      Wait-Job -Job $job -Timeout 5
      if ($job.State -eq "Completed") {
         Receive-Job $job
      }
      Remove-Job $job -Force
      Write-Host "Update Finish [$([datetime]::Now)]: $ProgramPath"
      $PrevLastWriteTime = $LastWriteTime
   }
}