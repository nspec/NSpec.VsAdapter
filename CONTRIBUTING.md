# Contributing

* [Microsoft Visual Studio 2013 Community Edition](https://www.visualstudio.com/it-it/downloads/download-visual-studio-vs.aspx) at least is needed to open main solution.
* [Microsoft Visual Studio 2013 SDK](https://www.microsoft.com/en-us/download/details.aspx?id=40758) must also be installed on development machine. That is **only** needed if you want to build the package from scratch, not if you just want to install the extension in VS and use it.
  * We have not tested yet if package can be built also against Visual Studio *2015* SDK. BTW, there's an [open issue](https://github.com/BrainCrumbz/NSpec.VsAdapter/issues/1) for that, if you feel like helping.
* This project has unit and integration tests written in NUnit framework. NUnit Test Adapter is needed to run those tests under Visual Studio.
* Set *NSpec.VsAdapter.Vsix* project as startup project, so to install, run & debug this adapter in an experimental Visual Studio hive.
  * By default this will start a VS2013 Community Edition (experimental) instance. If you need to change that, go to *NSpec.VsAdapter.Vsix* project properties, *Debug* panel, *Start Action* section and change path in *Start external program:* accordingly.

## Branch housekeeping

If you are a direct contributor to the project, please keep an eye on your past development or features branches and think about archiving them once they're no longer needed. 
No worries, their commits will still be available under named tags, it's just that they will not pollute the branch list.

If you're running on a Windows OS, there's a an [archive-branch](https://github.com/BrainCrumbz/NSpec.VsAdapter/blob/master/src/scripts/archive-branch.bat) batch script available. Otherwise, the command sequence to run in a *nix shell is the following:

```bash
# Get local branch from remote, if needed
git checkout <your-branch-name>

# Go back to master
git checkout master

# Create local tag
git tag archive/<your-branch-name> <your-branch-name>

# Create remote tag
git push origin archive/<your-branch-name>

# Delete local branch
git branch -d <your-branch-name>

# Delete remote branch
git push origin --delete <your-branch-name>
```

If you need to later retrieve an archived branch, just run the following commands:

```bash
# Checkout archive tag
git checkout archive/<your-branch-name>

# (Re)Create branch
git checkout -b <some-branch-name>
```
