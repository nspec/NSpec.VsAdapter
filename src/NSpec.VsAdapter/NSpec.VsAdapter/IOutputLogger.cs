using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    public interface IOutputLogger
    {
        void Debug(string message);

        void Info(string message);

        void Warn(string message);

        void Warn(Exception ex, string message);

        void Error(string message);

        void Error(Exception ex, string message);
    }
}
