namespace CoreAOP.UnitTests
{
    [TestAspect]
    public interface ITestService 
    {
        [TestAspect]
        bool TestMethod();
    }
}
