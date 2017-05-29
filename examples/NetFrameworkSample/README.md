# NetFrameworkSample

Sample solution showing how a NSpec test project targeting .NET Framework is setup.

Highlights:

- Test project is a class library targeting .NET Framework 4.5.2 (or above)
- NSpec is installed into test project as a NuGet package
- NSpec.VsAdapter is installed into test project as a NuGet package
- Tests can be run from Visual Studio Test Explorer
- To keep consistency with other samples, solution has the typical directory structure found in ASP.NET Core project template,
where all application projects are under a `src\` directory, while test projects are in a `test\` directory.
