using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    public class OutputLogger : IOutputLogger
    {
        public OutputLogger(IMessageLogger messageLogger)
        {
            this.MessageLogger = messageLogger;

            DebugMode = false;
        }

        public IMessageLogger MessageLogger { get; set; }

        public bool DebugMode { get; set; }

        public void Debug(string message)
        {
            if (DebugMode)
            {
                MessageLogger.SendMessage(TestMessageLevel.Informational, "[DEBUG] " + message);
            }
        }

        public void Info(string message)
        {
            MessageLogger.SendMessage(TestMessageLevel.Informational, message);
        }

        public void Warn(string message)
        {
            MessageLogger.SendMessage(TestMessageLevel.Warning, message);
        }

        public void Error(string message)
        {
            MessageLogger.SendMessage(TestMessageLevel.Error, message);
        }
    }
}
