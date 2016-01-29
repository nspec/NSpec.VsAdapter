﻿using NSpec;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdHocConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblies = new Assembly[] 
            { 
                typeof(SampleSpecs.DummyPublicClass).Assembly,
            };

            //types that should be considered for testing
            var types = assemblies.SelectMany(asm => asm.GetTypes()).ToArray();

            //now that we have our types, set up a finder so that NSpec
            //can determine the inheritance hierarchy
            var finder = new SpecFinder(types);

            //we've got our inheritance hierarchy,
            //now we can build our test tree using default conventions
            var builder = new ContextBuilder(finder, new DefaultConventions());

            //create the nspec runner with a
            //live formatter so we get console output
            var runner = new ContextRunner(
                           builder,
                           new ConsoleFormatter(),
                           false);

            //create our final collection of concrete tests
            var testCollection = builder.Contexts().Build();

            //run the tests and get results (to do whatever you want with)
            var results = runner.Run(testCollection);

            //console write line to pause the exe
            System.Console.ReadLine();
        }
    }
}