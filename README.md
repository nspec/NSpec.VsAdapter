# NSpec.VsAdapter

A test adapter to run NSpec tests from Test Explorer in Visual Studio 2013 and later.

## Project status 

This project is currently in early stage of development. It has been tried against real-life projects, but it might not be ready yet to be used as your *one and only* NSpec test runner.

## Setup

Download VSIX extension file from [Releases page](https://github.com/BrainCrumbz/NSpec.VsAdapter/releases), picking the most recent version available. For regular usage, downloading `NSpec.VsAdapter.Release.vsix` version is fine. Double click on VSIX file to install it in your Visual Studio environment. That's it.

Alternatively, if you want to notify issues you found while running tests, you could try downloading the debug version  `NSpec.VsAdapter.Debug.vsix` or go the full route and [run adapter from its source code](./CONTRIBUTING.md).

## Configuration

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

An example of such a file can be found in test source code at [src/NSpec.VsAdapter/Test/Samples/SolutionItems/samples.runsettings](./sln/test/Samples/samples.runsettings)

## Contributing

See [CONTRIBUTING](./CONTRIBUTING.md) doc page in this project.

## License

See [LICENSE.txt](./LICENSE.txt) in this project.
