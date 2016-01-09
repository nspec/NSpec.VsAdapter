using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    [Serializable]
    public class LogRecorder : IOutputLogger
    {
        public LogRecorder()
        {
            this.writer = new StringWriter();
        }

        public string Replay()
        {
            writer.Flush();

            var content = writer.ToString();
            
            return content;
        }

        public void Debug(string message)
        {
            writer.WriteLine();
            writer.Write("<DEBUG> " + message);
        }

        public void Info(string message)
        {
            writer.WriteLine();
            writer.Write("<INFO>  " + message);
        }

        public void Warn(string message)
        {
            writer.WriteLine();
            writer.Write("<WARN>  " + message);
        }

        public void Warn(Exception ex, string message)
        {
            writer.WriteLine();
            writer.Write("<WARN>  " + message + " " + ex.ToString());
        }

        public void Error(string message)
        {
            writer.WriteLine();
            writer.Write("<ERROR> " + message);
        }

        public void Error(Exception ex, string message)
        {
            writer.WriteLine();
            writer.Write("<ERROR> " + message + " " + ex.ToString());
        }

        private StringWriter writer;
    }
}
