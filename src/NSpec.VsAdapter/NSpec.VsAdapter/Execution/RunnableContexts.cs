using NSpec.Domain;
using NSpec.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public interface IRunnableContext
    {
        string Name { get; }

        int ExampleCount { get; }

        void Run(ILiveFormatter formatter);
    }

    public class RunnableContext : IRunnableContext
    {
        public RunnableContext(Context context)
        {
            this.context = context;
        }

        public virtual string Name { get { return context.Name; } }

        public virtual int ExampleCount { get { return context.AllExamples().Count(); } }

        public virtual void Run(ILiveFormatter formatter)
        {
            context.Run(formatter, false);

            context.AssignExceptions();
        }

        protected readonly Context context;
    }

    public class SelectedRunnableContext : RunnableContext, IRunnableContext
    {
        public SelectedRunnableContext(Context context)
            : base(context)
        {
            this.instance = context.GetInstance();
        }

        public override void Run(ILiveFormatter formatter)
        {
            context.Run(formatter, false, instance);

            context.AssignExceptions();
        }

        readonly nspec instance;
    }
}
