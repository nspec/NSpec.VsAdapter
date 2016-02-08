# NSpec.VsAdapter

A test adapter to run NSpec tests from Test Explorer in Visual Studio 2013 and later.

## Project status 

This project is currently in early stage of development.

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
|----------|--------|---------------------------------|-----------------------------------------------------------------------------------------|
| LogLevel | String | Trace, Debug, Info, Warn, Error | Minimum log level to show. Log messages with a level lower than this will not be shown. |

An example of such a file can be found in test source code at [src/NSpec.VsAdapter/Test/Samples/SolutionItems/samples.runsettings](https://github.com/BrainCrumbz/NSpec.VsAdapter/blob/master/src/NSpec.VsAdapter/Test/Samples/SolutionItems/samples.runsettings)

## Contributing

* Microsoft Visual Studio 2013 Community Edition at least is needed to open main solution.
* This project has unit and integration tests written in NUnit framework. NUnit Test Adapter is needed to run those tests under Visual Studio.
* Set *NSpec.VsAdapter.Vsix* project as startup project, so to install, run & debug this adapter in an experimental Visual Studio hive.
  * By default this will start a VS2013 Community Edition (experimental) instance. If you need to change that, go to *NSpec.VsAdapter.Vsix* project properties, *Debug* panel, *Start Action* section and change path in *Start external program:* accordingly.

## License

See [LICENSE.txt](https://github.com/BrainCrumbz/NSpec.VsAdapter/blob/master/LICENSE.txt) in this project.
