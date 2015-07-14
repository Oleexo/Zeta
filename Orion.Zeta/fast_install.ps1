Clear-Host
Write-Host "Fast compilation && installation of Zeta"

Write-Host "Step 1: Compilation"
$msbuild = "C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
$solutionName = $PSScriptRoot + "\Orion.Zeta.sln"
$destinationExecutable = "C:\Program Files\Zeta\"
$sourceExecutable = $PSScriptRoot + "\Orion.Zeta\bin\Release\"

& $msbuild $solutionName /p:Configuration=Release /t:rebuild /fl "/flp2:logfile=$PSScriptRoot\errors.txt;errorsonly;Append;"

Write-Host "Compilation done!"

Write-Host "Step 2: Installation"
if (!(Test-Path -Path $destinationExecutable)) {
	Try {
		New-Item -ItemType directory -Path $destinationExecutable
	}
	Catch {
		Write-Host "Run this script in administrator if you want fast installation."
		Break
	}
}

Copy-Item ($sourceExecutable + "*") $destinationExecutable -recurse

Write-Host "Installation done!"