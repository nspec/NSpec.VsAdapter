# NSpec.VsAdapter

[![NuGet Version and Downloads count](https://buildstats.info/nuget/NSpec.VsAdapter)](https://www.nuget.org/packages/NSpec.VsAdapter) 
[![Visual Studio Marketplace](https://vsmarketplacebadge.apphb.com/version-short/BrainCrumbz.NSpecVSAdapter.svg)](https://marketplace.visualstudio.com/items?itemName=BrainCrumbz.NSpecVSAdapter) 
[![AppVeyor Build status](https://ci.appveyor.com/api/projects/status/5mmtg044ds5xx8xr/branch/master?svg=true)](https://ci.appveyor.com/project/BrainCrumbz/nspec-vsadapter/branch/master)

NSpec.VsAdapter is a test adapter to run NSpec tests from Test Explorer in 
Visual Studio. It runs tests from projects targeting classic .NET Framework. 
It is available both as a [Visual Studio Extension](https://marketplace.visualstudio.com/items?itemName=BrainCrumbz.NSpecVSAdapter) 
(for VS 2013, 2015) as well as a [NuGet Package](https://www.nuget.org/packages/NSpec.VsAdapter) 
(for VS 2017). VS Extension for 2017 is under development.

## Minimum requirements

It currently supports test projects targeting .NET Framework 4.5 and later, and 
NSpec 1.0.x.

As an extension, it can work in Visual Studio 2013 and 2015, Community 
Edition and above. As a NuGet package, it can work in Visual Studio 2017, 
Community Edition and above.

**NOTE:** To work with old *project.json*-based .NET Core projects, please try 
[NSpec dotnet test runner](https://www.nuget.org/packages/dotnet-test-nspec). 
Support for *MSBuild*-based .NET Core projects is under development. 
Again, support for VS 2017 Extension is under development as well.

## Usage

### Installation

#### Extension

Open *Extensions and Updates* window from within Visual Studio and type 
`nspec vs` in search field, then download this extension showing on top. 
That's it.

When you want to install this extension on more than one Visual Studio version 
at once, or when you don't have Visual Studio already open, you can browse to 
[this extension page](https://marketplace.visualstudio.com/items?itemName=BrainCrumbz.NSpecVSAdapter) 
on Visual Studio marketplace, download it locally to your machine and double 
click on grabbed VSIX file.

#### NuGet Package

Proceed as for any other package. You can open *Manage NuGet Packages ...* 
from a project or solution and look for `NSpec.VsAdapter` in search field. 
Or you can open *Package Manager Console* and run `Install-Package NSpec.VsAdapter -ProjectName <YourProjectName>`.

In order to pick tests for whole solution, it is enough to install this package 
in just one of your projects.

### Launch

Open a solution in Visual Studio with at least one NSpec test project, then 
build all. Open VS Test Explorer window and wait until list gets populated 
with test specifications from all projects. Click on *Run All* or select some 
tests to be run. You can also group specifications based on project, or 
`nspec`-derived class, or trait.

## Examples

See under [examples/](./examples):

- [NetFrameworkSample](./examples/NetFrameworkSample)  
Sample solution showing how a NSpec test project targeting .NET Framework is setup

### Configuration

This Visual Studio test adapter can be configured by using a `.runsettings` 
file (see [MSDN](https://msdn.microsoft.com/en-us/library/jj635153.aspx)). 
Currently supported file format is:

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>

  <!-- other configurations -->

  <!-- NSpec VS Adapter -->
  <NSpec.VsAdapter>
    <LogLevel>Debug</LogLevel>
  </NSpec.VsAdapter>

</RunSettings>
```

Currently supported settings are:

| Name | Type | Values | Description |
|------|------|--------|-------------|
| LogLevel | String | Trace, Debug, Info, Warn, Error | Minimum log level to show. Log messages with a level lower than this will not be shown. |

An example of such a file can be found in test source code at [sln/test/Samples/samples.runsettings](./sln/test/Samples/samples.runsettings).

## Breaking changes

To check for potential breaking changes, see [BREAKING-CHANGES.md](./BREAKING-CHANGES.md).

## Contributing

See [CONTRIBUTING](./CONTRIBUTING.md) doc page.

## License

[MIT](./LICENSE.txt).

## Credits

NSpec.VsAdapter is written by [BrainCrumbz](http://www.braincrumbz.com). It's shaped and
benefited by hard work from our [contributors](https://github.com/nspec/NSpec.VsAdapter/contributors).
