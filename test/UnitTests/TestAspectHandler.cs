using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAOP.UnitTests
{
    using System.Reflection;
    class TestAspectHandler : IAspect
    {
        public Type CreatedType;
        public MethodInfo MiEnter, MiExit, MiEx;
        public Exception Ex;
        public int CallCount = 0;

        public void OnCreate(Type createdType)
        {
            CreatedType = createdType;
        }

        public void OnEnter(MethodInfo mi)
        {
            MiEnter = mi;
            CallCount++;
        }

        public void OnException(MethodInfo mi, Exception ex)
        {
            MiEx = mi;
            Ex = ex;
        }

        public void OnExit(MethodInfo mi)
        {
            MiExit = mi;
        }
    }

}
