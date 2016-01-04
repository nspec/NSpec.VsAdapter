using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation.Projects
{
    public interface IProjectNotifier
    {
        IObservable<IEnumerable<ProjectInfo>> ProjectStream { get; }
    }
}
