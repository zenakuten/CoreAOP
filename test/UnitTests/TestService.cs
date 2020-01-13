namespace CoreAOP.UnitTests
{
    using System;

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
