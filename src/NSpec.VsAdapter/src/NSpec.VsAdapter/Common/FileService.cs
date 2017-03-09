using System;
using System.IO;

namespace NSpec.VsAdapter.Common
{
    public class FileService : IFileService
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public DateTime LastModified(string path)
        {
            return File.GetLastWriteTime(path);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
