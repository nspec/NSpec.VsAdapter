using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.IntegrationTests
{
    class ConsoleLogger : IMessageLogger
    {
        public void SendMessage(TestMessageLevel testMessageLevel, string message)
        {
            var consoleOutput = String.Format("<{0}> {1}", testMessageLevel, message);

            Console.WriteLine(consoleOutput);
        }
    }
}
