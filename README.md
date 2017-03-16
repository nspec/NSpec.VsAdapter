# NSpec.VsAdapter

NSpec.VsAdapter is a test adapter to run NSpec tests from Test Explorer in Visual Studio 2013 and Visual Studio 2015 (integration with Visual Studio 2017 is in progress).

## Minimum requirements

It currently supports test projects based on .NET Framework 4.5 and later, and NSpec 1.0.x. It can be installed in Visual Studio 2013 and 2015.

## Usage

### Installation

Open *Extensions and Updates* window from within Visual Studio and type `nspec vs` in search field, then download this extension showing on top. That's it.

When you want to install this extension on more than one Visual Studio version at once, or when you don't have Visual Studio already open, you can browse to [this extension page](https://marketplace.visualstudio.com/items?itemName=GiuseppePiscopo.NSpecVSAdapter) on Visual Studio marketplace, download it locally to your machine and double click on grabbed VSIX file.

### Launch

Open a solution in Visual Studio with at least one NSpec test project, then build all. Open VS Test Explorer window and wait until list gets populated with test specifications from all projects. Click on *Run All* or select some tests to be run. You can also group specifications based on project, or `nspec`-derived class, or trait.

### Configuration

This Visual Studio test adapter can be configured by using a `.runsettings` file (see [MSDN](https://msdn.microsoft.com/en-us/library/jj635153.aspx)). Currently supported file format is:

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
