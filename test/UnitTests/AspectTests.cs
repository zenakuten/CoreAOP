namespace CoreAOP.UnitTests
{
    using NUnit.Framework;
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public class AspectTests 
    {
        [Test]
        public void TestConstructorEvent()
        {
            var handler = new TestAspectHandler();
            TestAspect.handler = handler;
            var services = new ServiceCollection();
            services.AddTransient<ITestService, TestService>();
            services.AddAspect<ITestService, TestAspect>();
            var provider = services.BuildServiceProvider();
            var testService = provider.GetService<ITestService>();
            Assert.AreEqual(typeof(ITestService), handler.CreatedType);
        }

        [Test]
        public void TestConstructorEventUsingFactory()
        {
            var handler = new TestAspectHandler();
            TestAspect.handler = handler;
            var services = new ServiceCollection();
            services.AddTransient<ITestService, TestService>((sp) => new TestService());
            services.AddAspect<ITestService, TestAspect>();
            var provider = services.BuildServiceProvider();
            var testService = provider.GetService<ITestService>();
            Assert.AreEqual(typeof(TestService), handler.CreatedType);
        }

        [Test]
        public void TestMethodCall()
        {
            var handler = new TestAspectHandler();
            TestAspect.handler = handler;
            var services = new ServiceCollection();
            services.AddTransient<ITestService, TestService>();
            services.AddAspect<ITestService, TestAspect>();
            var provider = services.BuildServiceProvider();
            var testService = provider.GetService<ITestService>();
            var actual = testService.TestMethod();
            Assert.AreEqual(typeof(ITestService).GetMethod(nameof(testService.TestMethod)), handler.MiEnter);
            Assert.AreEqual(typeof(ITestService).GetMethod(nameof(testService.TestMethod)), handler.MiExit);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void TestExceptionEvent()
        {
            var handler = new TestAspectHandler();
            TestAspect.handler = handler;
            var services = new ServiceCollection();
            services.AddTransient<ITestService, TestServiceWithThrow>();
            services.AddAspect<ITestService, TestAspect>();
            var provider = services.BuildServiceProvider();
            var testService = provider.GetService<ITestService>();
            try
            {
                testService.TestMethod();
            }
            catch
            {
            }
            Assert.AreEqual(typeof(ITestService).GetMethod(nameof(testService.TestMethod)), handler.MiEx);
            Assert.AreEqual(typeof(NotImplementedException), handler.Ex.GetType());
        }
    }
}
