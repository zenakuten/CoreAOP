namespace CoreAOP.UnitTests
{
    [TestAspect]
    public interface ITestService 
    {
        [TestAspect]
        bool TestMethod(bool testParam);

        [AlwaysFalseOnCall]
        bool TestMethodAlwaysFalseOnCall(bool testParam);

        [AlwaysFalseOnExit]
        bool TestMethodAlwaysFalseOnExit(bool testParam);
    }
}
