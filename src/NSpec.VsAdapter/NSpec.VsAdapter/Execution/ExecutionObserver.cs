﻿using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Execution
{
    public class ExecutionObserver : IExecutionObserver
    {
        public ExecutionObserver(ITestExecutionRecorder testExecutionRecorder, ITestResultMapper testResultMapper)
        {
            this.testExecutionRecorder = testExecutionRecorder;
            this.testResultMapper = testResultMapper;
        }

        public void Write(ExampleBase example, int level)
        {
            // ignore level

            var testResult = testResultMapper.FromExample(example);

            testExecutionRecorder.RecordResult(testResult);
        }

        public void Write(Context context)
        {
            // do nothing
        }

        readonly ITestExecutionRecorder testExecutionRecorder;
        readonly ITestResultMapper testResultMapper;
    }
}
