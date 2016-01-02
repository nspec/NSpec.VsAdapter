using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter
{
    public interface IProjectWrapper
    {
        string OutputDirPath { get; }

        string OutputFileName { get; }
    }
}
