using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Logging
{
    public class LoggerFactory : ILoggerFactory
    {
        public LoggerFactory(IAdapterInfo adapterInfo)
        {
            this.adapterInfo = adapterInfo;
        }

        public OutputLogger CreateOutput(IMessageLogger messageLogger)
        {
            return new OutputLogger(messageLogger, adapterInfo);
        }

        readonly IAdapterInfo adapterInfo;
    }
}
