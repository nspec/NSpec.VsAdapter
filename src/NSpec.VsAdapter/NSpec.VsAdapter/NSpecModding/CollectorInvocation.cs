﻿using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.NSpecModding
{
    [Serializable]
    public class CollectorInvocation : ICollectorInvocation
    {
        public CollectorInvocation(string assemblyPath)
        {
            this.assemblyPath = assemblyPath;
        }

        public NSpecSpecification[] Collect()
        {
            var reflector = new Reflector(assemblyPath);

            var finder = new SpecFinder(reflector);

            var conventions = new DefaultConventions();

            var contextBuilder = new ContextBuilder(finder, conventions);

            var contexts = contextBuilder.Contexts();

            var builtContexts = contexts.Build();

            var examples = builtContexts.Examples();

            var debugInfoProvider = new DebugInfoProvider(assemblyPath);

            var exampleConverter = new ExampleConverter(assemblyPath, debugInfoProvider);

            var specifications = examples.Select(exampleConverter.Convert);

            return specifications.ToArray();
        }

        readonly string assemblyPath;
    }
}
