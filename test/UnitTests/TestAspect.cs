namespace CoreAOP.UnitTests
{
    using System;
    using System.Reflection;

    public class TestAspect : IAspect
    {
        public static IAspect handler = null;
        public void OnCreate(Type createdType)
        {
            handler.OnCreate(createdType);
        }

        public void OnEnter(MethodInfo mi, object[] args)
        {
            handler.OnEnter(mi,args);
        }

        public void OnException(MethodInfo mi, Exception ex)
        {
            handler.OnException(mi, ex);
        }

        public object OnExit(MethodInfo mi, object[] args, object retval)
        {
            return handler.OnExit(mi, args, retval);
        }
    }
}
