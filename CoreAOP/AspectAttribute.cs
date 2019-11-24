namespace CoreAOP
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Base class for aspect attributes
    /// </summary>
    public abstract class AspectAttribute : Attribute, IAspect
    {
        public virtual void OnCreate(Type createdType)
        {
        }

        public virtual void OnEnter(MethodInfo mi)
        {
        }

        public virtual void OnException(MethodInfo mi, Exception ex)
        {
        }

        public virtual void OnExit(MethodInfo mi)
        {
        }
    }
}
