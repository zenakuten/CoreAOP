namespace CoreAOP.UnitTests
{
    using System;
    using System.Reflection;

    class TestAspectHandler : IAspect
    {
        public Type CreatedType;
        public MethodInfo MiEnter, MiExit, MiEx;
        public Exception Ex;
        public int CallCount = 0;
        public object[] ArgsEnter;
        public object[] ArgsExit;
        public object Retval;

        public void OnCreate(Type createdType)
        {
            CreatedType = createdType;
        }

        public void OnEnter(MethodInfo mi, object[] args)
        {
            MiEnter = mi;
            ArgsEnter = args;
            CallCount++;
        }

        public void OnException(MethodInfo mi, Exception ex)
        {
            MiEx = mi;
            Ex = ex;
        }

        public object OnExit(MethodInfo mi, object[] args, object retval)
        {
            MiExit = mi;
            ArgsExit = args;
            Retval = retval;
            return retval;
        }
    }

}
