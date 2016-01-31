using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Logging
{
    public interface ILoggerFactory
    {
        IOutputLogger CreateOutput(IMessageLogger messageLogger);
    }
}
