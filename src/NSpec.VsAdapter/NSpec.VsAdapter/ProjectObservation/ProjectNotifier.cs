using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation
{
    public class ProjectNotifier : IProjectNotifier
    {
        public IObservable<IEnumerable<ProjectInfo>> ProjectStream
        {
            get { throw new NotImplementedException(); }
        }
    }
}
