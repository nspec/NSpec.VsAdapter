using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.IntegrationTests
{
    [Serializable]
    public class ConsoleLogger : IMessageLogger
    {
        public ConsoleLogger()
        {
            messageLevelToTextMap = new Dictionary<TestMessageLevel, string>()
            {
                { TestMessageLevel.Informational, "I" },
                { TestMessageLevel.Warning, "W" },
                { TestMessageLevel.Error, "E" },
            };
        }

        public void SendMessage(TestMessageLevel testMessageLevel, string message)
        {
            var consoleOutput = String.Format("{0}) {1}", messageLevelToTextMap[testMessageLevel], message);

            Console.WriteLine(consoleOutput);
        }

        readonly Dictionary<TestMessageLevel, string> messageLevelToTextMap;
    }
}
