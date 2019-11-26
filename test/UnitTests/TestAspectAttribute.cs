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

        public override void OnEnter(MethodInfo mi, object[] args)
        {
            handler.OnEnter(mi, args);
        }
        public override void OnExit(MethodInfo mi, object[] args)
        {
            handler.OnExit(mi, args);
        }

        public override void OnException(MethodInfo mi, Exception ex)
        {
            handler.OnException(mi, ex); 
        }
    }
}
