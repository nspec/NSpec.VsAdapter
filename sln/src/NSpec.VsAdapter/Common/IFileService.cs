using System;

namespace NSpec.VsAdapter.Common
{
    public interface IFileService
    {
        bool Exists(string path);

        DateTime LastModified(string path);

        string ReadAllText(string path);
    }
}
