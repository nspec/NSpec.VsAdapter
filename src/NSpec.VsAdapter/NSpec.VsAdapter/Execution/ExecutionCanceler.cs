using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Execution
{
    [Serializable]
    public class ExecutionCanceler : IExecutionCanceler
    {
        public ExecutionCanceler(bool isCanceled)
        {
            IsCanceled = isCanceled;
        }

        public void CancelRun()
        {
            IsCanceled = true;
        }

        // IExecutionCanceler

        public bool IsCanceled { get; private set; }
    }
}
