namespace CoreAOP.UnitTests
{
    using System;

    class TestService : ITestService
    {
        public bool TestMethod(bool testParam)
        {
            return testParam;
        }
        public bool TestMethodAlwaysFalseOnCall(bool testParam)
        {
            return testParam;
        }
        public bool TestMethodAlwaysFalseOnExit(bool testParam)
        {
            return testParam;
        }
    }

    class TestServiceWithThrow : ITestService
    {
        public bool TestMethod(bool testParam)
        {
            throw new NotImplementedException();
        }
        public bool TestMethodAlwaysFalseOnCall(bool testParam)
        {
            throw new NotImplementedException();
        }
        public bool TestMethodAlwaysFalseOnExit(bool testParam)
        {
            throw new NotImplementedException();
        }
    }
}
