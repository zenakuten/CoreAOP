using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAOP.UnitTests
{
    class TestService : ITestService
    {
        public bool TestMethod()
        {
            return true;
        }
    }
    class TestServiceWithThrow : ITestService
    {
        public bool TestMethod()
        {
            throw new NotImplementedException();
        }
    }
}
