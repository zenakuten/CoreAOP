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
            var actual = testService.TestMethod(true);
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
            var actual = testService.TestMethod(true);
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
                testService.TestMethod(true);
            }
            catch
            {
            }
            Assert.AreEqual(typeof(ITestService).GetMethod(nameof(testService.TestMethod)), handler.MiEx);
            Assert.AreEqual(typeof(NotImplementedException), handler.Ex.GetType());
        }

        [Test]
        public void TestMethodCallAlwaysFalseOnCall()
        {
            var services = new ServiceCollection();
            services.AddTransient<ITestService, TestService>();
            services.AddAspects();
            var provider = services.BuildServiceProvider();
            var testService = provider.GetService<ITestService>();
            var actual = testService.TestMethodAlwaysFalseOnCall(true);
            Assert.AreEqual(false, actual);
        }

        [Test]
        public void TestMethodCallAlwaysFalseOnExit()
        {
            var services = new ServiceCollection();
            services.AddTransient<ITestService, TestService>();
            services.AddAspects();
            var provider = services.BuildServiceProvider();
            var testService = provider.GetService<ITestService>();
            var actual = testService.TestMethodAlwaysFalseOnExit(true);
            Assert.AreEqual(false, actual);
        }
    }
}