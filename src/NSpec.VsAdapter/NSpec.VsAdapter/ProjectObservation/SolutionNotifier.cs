using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation
{
    public class SolutionNotifier : ISolutionNotifier, IDisposable
    {
        public SolutionNotifier(ISolutionProvider solutionProvider)
        {
            var solution = solutionProvider.Provide();
        }

        public IObservable<SolutionInfo> SolutionOpenedStream
        {
            get { throw new NotImplementedException(); }
        }

        public IObservable<Unit> SolutionClosingStream
        {
            get { throw new NotImplementedException(); }
        }

        public IObservable<ProjectInfo> ProjectAddedStream
        {
            get { throw new NotImplementedException(); }
        }

        public IObservable<ProjectInfo> ProjectRemovingtream
        {
            get { throw new NotImplementedException(); }
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        readonly CompositeDisposable disposables = new CompositeDisposable();
    }
}
