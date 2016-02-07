using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests
{
    class DummyTestException : ApplicationException
    {
        public DummyTestException() : base() { }

        public DummyTestException(string message) : base(message) { }
    }
}
