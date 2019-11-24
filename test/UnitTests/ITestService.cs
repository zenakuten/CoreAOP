using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAOP.UnitTests
{

    [TestAspect]
    public interface ITestService 
    {
        [TestAspect]
        bool TestMethod();
    }
}
