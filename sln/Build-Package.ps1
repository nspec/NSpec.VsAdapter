# Utilities

# Taken from psake https://github.com/psake/psake
<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec
{
	[CmdletBinding()]
	param(
		[Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
		[Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
	)
	$global:lastexitcode = 0
	& $cmd
	if ($lastexitcode -ne 0) {
		throw ("Exec: " + $errorMessage)
	}
}

function CleanContent([string]$path) {
	if (Test-Path $path) {
		$globPath = Join-Path $path *
		Remove-Item -Force -Recurse $globPath
	}
}

function CleanProject([string]$projectPath) {
	@(
		(Join-Path $projectPath bin\ ),
		(Join-Path $projectPath obj\ ),
		(Join-Path $projectPath publish\ )

	) | ForEach-Object { CleanContent $_ }
}

function GetVersionOptions() {
	$isContinuous = [bool]$env:APPVEYOR
	$isProduction = [bool]$env:APPVEYOR_REPO_TAG_NAME

	if ($isContinuous) {
		if ($isProduction) {
			Write-Host "Continuous Delivery, Production package, keeping nupkg version as is."
			$versionOpts = @()
		} else {
			# this should have already been updated to development version number (<nuspec vers>-dev-<build nr>)
			$devPackageVersion = $env:APPVEYOR_BUILD_VERSION

			Write-Host "Continuous Delivery, Development package, changing nupkg version to '$devPackageVersion'."

			$versionOpts = @( "-version", $devPackageVersion )
		}
	} else {
		Write-Host "Local machine, keeping nupkg version as is."
		$versionOpts = @()
	}

	return $versionOpts
}

# Toolchain commands

function RestoreProject([string]$projectPath) {
	$projName = Split-Path $projectPath -Leaf
	$csprojFile = Join-Path $projectPath "$projName.csproj"

	Exec { & nuget restore $csprojFile -SolutionDirectory ".\" } "Restoring $projectPath"
}

function BuildProject([string]$projectPath) {
	$projName = Split-Path $projectPath -Leaf
	$csprojFile = Join-Path $projectPath "$projName.csproj"

	Exec { & msbuild $csprojFile /t:build /p:Configuration=Release /v:minimal } "Building $projectPath"
}

function TestProject([string]$projectPath) {
	$projName = Split-Path $projectPath -Leaf
	$csprojFile = Join-Path $projectPath "$projName.csproj"

	$NUnitConsole = "packages\NUnit.ConsoleRunner.3.6.1\tools\nunit3-console"

	Exec { & $NUnitConsole $csprojFile --config=Release } "Testing $projectPath"
}

function PackageProject([string]$projectPath, [string[]]$versionOpts) {
	$projName = Split-Path $projectPath -Leaf
	$nuspecFile = Join-Path $projectPath "$projName.nuspec"
	$publishDir = Join-Path $projectPath "publish"

	Exec {
		& nuget pack $nuspecFile $versionOpts -outputdirectory $publishDir -properties Configuration=Release

	} "Packaging $_"
}

###

# Main

# add Visual Studio 2013 MSBuild to system path if needed
if ((Get-Command "msbuild.exe" -ErrorAction SilentlyContinue) -eq $null)
{
   $env:Path += ";C:\Program Files (x86)\MSBuild\12.0\Bin"
}

# move to global.json directory
Push-Location sln


# Clean
@(
	"src\NSpec.VsAdapter",
	"src\NSpec.VsAdapter.Vsix",
	"test\Samples\src\SampleSystem",
	"test\Samples\src\SampleAsyncSystem",
	"test\Samples\src\ConfigSampleSystem",
	"test\Samples\test\SampleSpecs",
	"test\Samples\test\SampleAsyncSpecs",
	"test\Samples\test\ConfigSampleSpecs",
	"test\Samples\test\AdHocConsoleRunner",
	"test\NSpec.VsAdapter.UnitTests",
	"test\NSpec.VsAdapter.IntegrationTests"

) | ForEach-Object { CleanProject $_ }


# Restore
# On AppVeyor VS 2013 image, there are issues when restoring single projects
# For time being, switch to restoring solution
@(
	".\NSpec.VsAdapter.sln"
) | ForEach-Object { Exec { & nuget restore $_ } "Restoring $_" }
<#
@(
	"src\NSpec.VsAdapter",
	"test\Samples\test\SampleSpecs",
	"test\Samples\test\SampleAsyncSpecs",
	"test\Samples\test\ConfigSampleSpecs",
	"test\Samples\test\AdHocConsoleRunner",
	"test\NSpec.VsAdapter.UnitTests",
	"test\NSpec.VsAdapter.IntegrationTests"

) | ForEach-Object { RestoreProject $_ }
#>

# Build
@(
	"src\NSpec.VsAdapter",
	"src\NSpec.VsAdapter.Vsix",
	"test\Samples\src\SampleSystem",
	"test\Samples\src\SampleAsyncSystem",
	"test\Samples\src\ConfigSampleSystem",
	"test\Samples\test\SampleSpecs",
	"test\Samples\test\SampleAsyncSpecs",
	"test\Samples\test\ConfigSampleSpecs",
	"test\Samples\test\AdHocConsoleRunner",
	"test\NSpec.VsAdapter.UnitTests",
	"test\NSpec.VsAdapter.IntegrationTests"

) | ForEach-Object { BuildProject $_ }


# Test
@(
	"test\NSpec.VsAdapter.UnitTests",
	"test\NSpec.VsAdapter.IntegrationTests"

) | ForEach-Object { TestProject $_ }


# Package
$versionOpts = GetVersionOptions

@(
	"src\NSpec.VsAdapter"

) | ForEach-Object { PackageProject $_ $versionOpts }


Pop-Location
