using NSpec.Domain;
using NSpec.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class RunnableContext
    {
        public RunnableContext(Context context)
        {
            this.context = context;
        }

        public string Name { get { return context.Name; } }

        public int ExampleCount { get { return context.AllExamples().Count(); } }

        public virtual void Run(ILiveFormatter formatter)
        {
            context.Run(formatter, false);

            context.AssignExceptions();
        }

        protected readonly Context context;
    }

    public class SelectedRunnableContext : RunnableContext
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
