using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    public static class DisposableExtensions
    {
        public static void DisposeWith(this IDisposable disposableItem, CompositeDisposable disposables)
        {
            disposables.Add(disposableItem);
        }

        public static T DisposeWith<T>(this T disposableItem, CompositeDisposable disposables) where T : IDisposable
        {
            disposables.Add(disposableItem);

            return disposableItem;
        }
    }
}
