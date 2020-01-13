namespace CoreAOP.UnitTests
{
    using System;
    using System.Reflection;

    public class TestAspectAttribute : AspectAttribute
    {
        public static IAspect handler = null;

        public override void OnCreate(Type createdType)
        {
            handler.OnCreate(createdType);
        }

        public override object[] OnEnter(MethodInfo mi, object[] args)
        {
            return handler.OnEnter(mi, args);
        }
        public override object OnExit(MethodInfo mi, object[] args, object retval)
        {
            return handler.OnExit(mi, args, retval);
        }

        public override void OnException(MethodInfo mi, Exception ex)
        {
            handler.OnException(mi, ex); 
        }
    }
}
