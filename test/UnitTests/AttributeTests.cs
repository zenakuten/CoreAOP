namespace CoreAOP.UnitTests
{
    using System;
    using NUnit.Framework;
    using Microsoft.Extensions.DependencyInjection;

    public class AttributeTests 
    {
        [Test]
        public void TestConstructorEvent()
        {
            var handler = new TestAspectHandler();
            TestAspectAttribute.handler = handler;
            var services = new ServiceCollection();
            services.AddTransient<ITestService, TestService>();
            services.AddAspects();
            var provider = services.BuildServiceProvider();
            var testService = provider.GetService<ITestService>();
            Assert.AreEqual(typeof(ITestService), handler.CreatedType);
        }

        [Test]
        public void TestMethodCall()
        {
            var handler = new TestAspectHandler();
            TestAspectAttribute.handler = handler;
            var services = new ServiceCollection();
            services.AddTransient<ITestService, TestService>();
            services.AddAspects();
            var provider = services.BuildServiceProvider();
            var testService = provider.GetService<ITestService>();
            var actual = testService.TestMethod();
            Assert.AreEqual(typeof(ITestService).GetMethod(nameof(testService.TestMethod)), handler.MiEnter);
            Assert.AreEqual(typeof(ITestService).GetMethod(nameof(testService.TestMethod)), handler.MiExit);
            Assert.AreEqual(true, actual);

            //Attribute is on type and method, but should only be called once
            Assert.AreEqual(1, handler.CallCount); 
        }

        [Test]
        public void TestMethodCallWithOtherAspect()
        {
            var handler = new TestAspectHandler();
            TestAspectAttribute.handler = handler;
            TestAspect.handler = handler;
            var services = new ServiceCollection();
            services.AddTransient<ITestService, TestService>();
            services.AddAspects();
            services.AddAspect<ITestService, TestAspect>();
            var provider = services.BuildServiceProvider();
            var testService = provider.GetService<ITestService>();
            var actual = testService.TestMethod();
            Assert.AreEqual(typeof(ITestService).GetMethod(nameof(testService.TestMethod)), handler.MiEnter);
            Assert.AreEqual(typeof(ITestService).GetMethod(nameof(testService.TestMethod)), handler.MiExit);
            Assert.AreEqual(true, actual);

            //once for the attributes, once for the aspect
            Assert.AreEqual(2, handler.CallCount); 
        }

        [Test]
        public void TestExceptionEvent()
        {
            var handler = new TestAspectHandler();
            TestAspectAttribute.handler = handler;
            var services = new ServiceCollection();
            services.AddTransient<ITestService, TestServiceWithThrow>();
            services.AddAspects();
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