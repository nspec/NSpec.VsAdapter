using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter
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
    }
}
