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

        public override void OnEnter(MethodInfo mi)
        {
            handler.OnEnter(mi);
        }
        public override void OnExit(MethodInfo mi)
        {
            handler.OnExit(mi);
        }

        public override void OnException(MethodInfo mi, Exception ex)
        {
            handler.OnException(mi, ex); 
        }
    }
}
