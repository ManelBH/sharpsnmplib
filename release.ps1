$msBuild = "msbuild"
try
{
    & $msBuild /version
    Write-Host "Likely on Linux/macOS."
}
catch
{
    Write-Host "MSBuild doesn't exist. Use VSSetup instead."

    Install-Module VSSetup -Scope CurrentUser -Force
    Update-Module VSSetup
    $instance = Get-VSSetupInstance -All
    $installDir = $instance.installationPath
    Write-Host "Found VS in " + $installDir
    $msBuild = $installDir + '\MSBuild\15.0\Bin\MSBuild.exe'
    if (![System.IO.File]::Exists($msBuild))
    {
        Write-Host "MSBuild doesn't exist. Exit."
        exit 1
    }

    Write-Host "Likely on Windows."
}

Write-Host "MSBuild found. Compile the projects."

$solution = "SharpSnmpLib.Samples.sln"

& $msBuild $solution /p:Configuration=Release /t:restore
& $msBuild $solution /p:Configuration=Release /t:clean
& $msBuild $solution /p:Configuration=Release
if ($LASTEXITCODE -ne 0)
{
    Write-Host "Compilation failed. Exit."
    exit $LASTEXITCODE
}

Write-Host "Compilation finished."
