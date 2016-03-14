# Contributing

* [Microsoft Visual Studio 2013 Community Edition](https://www.visualstudio.com/it-it/downloads/download-visual-studio-vs.aspx) at least is needed to open main solution.
* [Microsoft Visual Studio 2013 SDK](https://www.microsoft.com/en-us/download/details.aspx?id=40758) must also be installed on development machine. That is **only** needed if you want to build the package from scratch, not if you just want to install the extension in VS and use it.
  * We have not tested yet if package can be built also against Visual Studio *2015* SDK. BTW, there's an [open issue](https://github.com/BrainCrumbz/NSpec.VsAdapter/issues/1) for that, if you feel like helping.
* This project has unit and integration tests written in NUnit framework. NUnit Test Adapter is needed to run those tests under Visual Studio.
* Set *NSpec.VsAdapter.Vsix* project as startup project, so to install, run & debug this adapter in an experimental Visual Studio hive.
  * By default this will start a VS2013 Community Edition (experimental) instance. If you need to change that, go to *NSpec.VsAdapter.Vsix* project properties, *Debug* panel, *Start Action* section and change path in *Start external program:* accordingly.
