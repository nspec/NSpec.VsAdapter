using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Core.Execution
{
    [Serializable]
    public class ExecutedExample
    {
        public string FullName { get; set; }

        public bool Pending { get; set; }

        public bool Failed { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionStackTrace { get; set; }
    }
}
