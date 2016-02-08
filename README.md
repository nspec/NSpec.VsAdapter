# NSpec.VsAdapter

A test adapter to run NSpec tests from Test Explorer in Visual Studio 2013 and later.

## Project status 

This project is currently in early stage of development. It has been tried against real-life projects, but it might not be ready yet to be used as your *one and only* NSpec test runner.

## Setup

Download VSIX extension file from [Releases page](https://github.com/BrainCrumbz/NSpec.VsAdapter/releases), picking the most recent version available. For regular usage, downloading `NSpec.VsAdapter.Release.vsix` version is fine. Double click on VSIX file to install it in your Visual Studio environment. That's it.

Alternatively, if you want to notify issues you found while running tests, you could try downloading the debug version  `NSpec.VsAdapter.Debug.vsix` or go the full route and [run adapter from its source code](https://github.com/BrainCrumbz/NSpec.VsAdapter#contributing).

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

An example of such a file can be found in test source code at [src/NSpec.VsAdapter/Test/Samples/SolutionItems/samples.runsettings](https://github.com/BrainCrumbz/NSpec.VsAdapter/blob/master/src/NSpec.VsAdapter/Test/Samples/SolutionItems/samples.runsettings)

## Contributing

* [Microsoft Visual Studio 2013 Community Edition](https://www.visualstudio.com/it-it/downloads/download-visual-studio-vs.aspx) at least is needed to open main solution.
* [Microsoft Visual Studio 2013 SDK](https://www.microsoft.com/en-us/download/details.aspx?id=40758) must also be installed on development machine. That is **only** needed if you want to build the package from scratch, not if you just want to install the extension in VS and use it.
  * We have not tested yet if package can be built also against Visual Studio *2015* SDK.
* This project has unit and integration tests written in NUnit framework. NUnit Test Adapter is needed to run those tests under Visual Studio.
* Set *NSpec.VsAdapter.Vsix* project as startup project, so to install, run & debug this adapter in an experimental Visual Studio hive.
  * By default this will start a VS2013 Community Edition (experimental) instance. If you need to change that, go to *NSpec.VsAdapter.Vsix* project properties, *Debug* panel, *Start Action* section and change path in *Start external program:* accordingly.

## License

See [LICENSE.txt](https://github.com/BrainCrumbz/NSpec.VsAdapter/blob/master/LICENSE.txt) in this project.
