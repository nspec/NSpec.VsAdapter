using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    public interface IFileService
    {
        bool Exists(string path);

        DateTime LastModified(string path);

        string ReadAllText(string path);
    }
}
