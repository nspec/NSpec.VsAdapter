namespace NSpec.VsAdapter.ProjectObservation.Projects
{
    public interface IProjectConverter
    {
        string ToTestDllPath(ProjectInfo projectInfo);
    }
}
